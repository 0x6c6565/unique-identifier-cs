using System;
using System.Buffers.Binary;
using System.Runtime.InteropServices;

namespace UniqueIdentifiers
{
	/// <summary>
	/// A 64 bit guid that is serializable in Unity.
	/// 'Borrows' heavily from Snowflake Twitter (C) 2012, save that the 
	///		data center and dc-machine id have been merged into a 
	///		hash of the local PC's mac address.
	///		
	/// Useful for instances in a running application. For longer term 
	///		serializable id storage use <see cref="Guid128"/>.
	/// </summary>
	[Serializable]
	[StructLayout(LayoutKind.Explicit, Pack = 1, Size = 8)]
	public struct Guid64 : IEquatable<Guid64>
	{
		/// <summary>This is for comparisons (same as default.)</summary>
		public static readonly Guid64 Empty = default;

		/// <summary>Allows for consistency when doing checks as this holds the entire guid.</summary>
		[FieldOffset(0)] public ulong value;

		[FieldOffset(0)] uint firstFour;
		[FieldOffset(4)] uint secondFour;

		[FieldOffset(0)] ushort firstTwo;
		[FieldOffset(2)] ushort secondTwo;
		[FieldOffset(4)] ushort thirdTwo;
		[FieldOffset(6)] ushort fourthTwo;

		[FieldOffset(0)] byte a;
		[FieldOffset(1)] byte b;
		[FieldOffset(2)] byte c;
		[FieldOffset(3)] byte d;
		[FieldOffset(4)] byte e;
		[FieldOffset(5)] byte f;
		[FieldOffset(6)] byte g;
		[FieldOffset(7)] byte h;

		/// <summary>Set the guid directly from its core storage type.</summary>
		/// <param name="value">The value of the guid.</param>
		public Guid64(ulong value) : this() { this.value = value; }

		/// <summary>Copies the bytes directly to <see cref="value"/>.</summary>
		/// <param name="bytes">The bytes to copy.</param>
		/// <exception cref="ArgumentNullException">If bytes is null.</exception>
		public Guid64(byte[] bytes)
			: this(new ReadOnlySpan<byte>(bytes ?? throw new ArgumentNullException(nameof(bytes))))
		{ }

		/// <summary>Copies the span of bytes to <see cref="value"/>.</summary>
		/// <param name="bytes">The array of bytes.</param>
		public Guid64(ReadOnlySpan<byte> bytes)
		{
			this = MemoryMarshal.Read<Guid64>(bytes);

			if (!BitConverter.IsLittleEndian) // Consistency with Guid128
			{
				firstFour = BinaryPrimitives.ReverseEndianness(firstFour);
				thirdTwo = BinaryPrimitives.ReverseEndianness(thirdTwo);
				fourthTwo = BinaryPrimitives.ReverseEndianness(fourthTwo);
			}
		}

		/// <summary>If the object is a Guid64 then calls <see cref="Equals(Guid64)"/>.</summary>
		/// <param name="obj">The object that will be interpreted as a Guid64.</param>
		/// <returns>True, if <paramref name="obj"/> is the same Guid64; false, otherwise.</returns>
#nullable enable
		public override bool Equals(object? obj) => obj is Guid64 guid && Equals(guid);
#nullable restore

		/// <summary>Compares the two Guid64s for equality.</summary>
		/// <param name="other">Compares the direct unsigned long values.</param>
		/// <returns>True, if equal; false, otherwise.</returns>
		public bool Equals(Guid64 other) => value == other.value;

		/// <summary>Passes along the hash of <see cref="value"/>.</summary>
		/// <returns>The generated hash code of <see cref="value"/>.</returns>
		public override int GetHashCode() => HashCode.Combine(value);

		/// <summary>Outputs the guid into a hexidecimal string, formatted as 00000000-0000-0000.</summary>
		/// <returns>The Guid64 as a hex string.</returns>
		public override string ToString()
		{
			return $"{firstFour.ToString("x8")}-{thirdTwo.ToString("x4")}-{fourthTwo.ToString("x4")}";
		}

		/// <summary>
		/// Currently, this implementation expects the hexidecimal string
		///		in the 00000000-0000-0000 format.
		///	</summary>
		/// <param name="value">The string to parse.</param>
		/// <param name="guid">The resultant Guid64, if successful.</param>
		/// <returns>True, if successful; false, otherwise.</returns>
		public static bool TryParse(string value, out Guid64 guid)
		{
			guid = default;

			value = value.Trim();
			if (18 == value.Length)
			{
				if ('-' == value[8] && '-' == value[13])
				{
					bool success = false;
					guid.a = DecodeByte(value, 6, 7, ref success);
					guid.b = DecodeByte(value, 4, 5, ref success);
					guid.c = DecodeByte(value, 2, 3, ref success);
					guid.d = DecodeByte(value, 0, 1, ref success);

					guid.e = DecodeByte(value, 11, 12, ref success);
					guid.f = DecodeByte(value, 9, 10, ref success);

					guid.g = DecodeByte(value, 16, 17, ref success);
					guid.h = DecodeByte(value, 14, 15, ref success);

					if (success)
					{
						if (!BitConverter.IsLittleEndian) // Consistency with Guid128
						{
							guid.firstFour = BinaryPrimitives.ReverseEndianness(guid.firstFour);
							guid.thirdTwo = BinaryPrimitives.ReverseEndianness(guid.thirdTwo);
							guid.fourthTwo = BinaryPrimitives.ReverseEndianness(guid.fourthTwo);
						}

						return true;
					}
				}
			}

			return false;
		}

		static byte DecodeByte(in string value, int a, int b, ref bool success)
			=> DecodeByte(value[a], value[b], ref success);
		static byte DecodeByte(char a, char b, ref bool success)
		{
			byte firstNibble = DecodeHex(a);
			if (16 > firstNibble)
			{
				byte secondNibble = DecodeHex(b);
				if (16 > secondNibble)
				{
					byte result = (byte)((firstNibble << 4) | secondNibble);
					success = true;

					return result;
				}
			}

			return 0;
		}
		static byte DecodeHex(char value)
		{
			const int zeroValue = 48;
			const int aOffsetByZeroValue = 65 - zeroValue - 10;

			int result = Convert.ToByte(char.ToUpper(value)) - zeroValue;
			return (byte)(10 > result ? result : result - aOffsetByZeroValue);
		}

		/// <summary>For implicit equality comparisons.</summary>
		/// <param name="lhs">The left hand side Guid.</param>
		/// <param name="rhs">The one on the right.</param>
		/// <returns>True, if equal; false, otherwise.</returns>
		public static bool operator ==(Guid64 lhs, Guid64 rhs) => lhs.Equals(rhs);
		/// <summary>For implicit inequality comparisons.</summary>
		/// <param name="lhs">The left hand side Guid.</param>
		/// <param name="rhs">The one on the right.</param>
		/// <returns>True, if equal; false, otherwise.</returns>
		public static bool operator !=(Guid64 lhs, Guid64 rhs) => !(lhs == rhs);

		/// <summary>Static helper for creating guids.</summary>
		/// <returns>The newly created Guid64.</returns>
		public static Guid64 NewGuid64() => Guid64Generator.Generate();
	}
}
