using System;
using System.Buffers.Binary;
using System.Runtime.InteropServices;

// Used for Unsafe<> which was removed in Unity 6.?
//using System.Runtime.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;

namespace UniqueIdentifiers
{
	/// <summary>
	/// This is, for all intents and purposes, a Unity-serializable 
	///		version of <see cref="System.Guid"/>.
	/// </summary>
	[Serializable]
	[StructLayout(LayoutKind.Explicit, Pack = 1, Size = 16)]
	public struct Guid128 : IEquatable<Guid128>, IEquatable<Guid>
	{
		/// <summary>Used for comparisons with a known invalid guid.</summary>
		public static readonly Guid128 Empty = Guid.Empty;

		/// <summary>The first four bytes; if big endian it will be reversed on array assignment.</summary>
		[FieldOffset(0)] public int _a;
		/// <summary>The third two bytes; if big endian it will be reversed on array assignment.</summary>
		[FieldOffset(4)] public short _b;
		/// <summary>The fourth two bytes bytes; if big endian it will be reversed on array assignment.</summary>
		[FieldOffset(6)] public short _c;
		/// <summary>9th</summary>
		[FieldOffset(8)] public byte _d;
		/// <summary>10th</summary>
		[FieldOffset(9)] public byte _e;
		/// <summary>11th</summary>
		[FieldOffset(10)] public byte _f;
		/// <summary>12th</summary>
		[FieldOffset(11)] public byte _g;
		/// <summary>13th</summary>
		[FieldOffset(12)] public byte _h;
		/// <summary>14th</summary>
		[FieldOffset(13)] public byte _i;
		/// <summary>15th</summary>
		[FieldOffset(14)] public byte _j;
		/// <summary>16th</summary>
		[FieldOffset(15)] public byte _k;

		/// <summary></summary>
		/// <param name="bytes"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public Guid128(byte[] bytes)
			: this(new ReadOnlySpan<byte>(bytes ?? throw new ArgumentNullException(nameof(bytes))))
		{ }

		/// <summary></summary>
		/// <param name="bytes"></param>
		public Guid128(ReadOnlySpan<byte> bytes)
		{
			this = MemoryMarshal.Read<Guid128>(bytes);

			if (!BitConverter.IsLittleEndian)
			{
				_a = BinaryPrimitives.ReverseEndianness(_a);
				_b = BinaryPrimitives.ReverseEndianness(_b);
				_c = BinaryPrimitives.ReverseEndianness(_c);
			}
		}

		/// <summary></summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <param name="c"></param>
		/// <param name="d"></param>
		/// <param name="e"></param>
		/// <param name="f"></param>
		/// <param name="g"></param>
		/// <param name="h"></param>
		/// <param name="i"></param>
		/// <param name="j"></param>
		/// <param name="k"></param>
		public Guid128(int a, short b, short c, byte d, byte e, byte f, byte g, byte h, byte i, byte j, byte k)
		{
			_a = a;
			_b = b;
			_c = c;
			_d = d;
			_e = e;
			_f = f;
			_g = g;
			_h = h;
			_i = i;
			_j = j;
			_k = k;
		}

		/// <summary></summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <param name="c"></param>
		/// <param name="d"></param>
		public Guid128(int a, short b, short c, byte[] d) : this(a, b, c, d[0], d[1], d[2], d[3], d[4], d[5], d[6], d[7]) { }

		/// <summary>Reinterprets a <see cref="System.Guid"/> to a <see cref="Guid128"/>.</summary>
		/// <param name="guid">The Guid to reinterpret.</param>
		public Guid128(Guid guid)
		{
			this = UnsafeUtility.As<Guid, Guid128>(ref guid);
			
			// Not available in Unity 6 era...
			//this = Unsafe.As<Guid, Guid128>(ref guid);
		}

		/// <summary>Implementation to mirror <see cref="Guid.GetHashCode"/>.</summary>
		/// <returns>The Guid128's hash.</returns>
		public override int GetHashCode()
		{
			return _a ^ (((int)_b << 16) | (int)(ushort)_c) ^ (((int)_f << 24) | _k);
		}

		/// <summary></summary>
		/// <param name="obj"></param>
		/// <returns>True, if the values are identical; false, otherwise.</returns>
		public override bool Equals(object obj) => obj is Guid128 guid && Equals(guid);

		/// <summary>Compares the two guids, byte-by-byte for equality.</summary>
		/// <param name="other">The other guid against which to compare.</param>
		/// <returns>True, if the values are identical; false, otherwise.</returns>
		public bool Equals(Guid128 other)
		{
			return _a == other._a &&
				   _b == other._b &&
				   _c == other._c &&
				   _d == other._d &&
				   _e == other._e &&
				   _f == other._f &&
				   _g == other._g &&
				   _h == other._h &&
				   _i == other._i &&
				   _j == other._j &&
				   _k == other._k;
		}

		/// <summary>Compares this guid against <paramref name="other"/>.</summary>
		/// <param name="other">The <see cref="System.Guid"/> to compare for equality.</param>
		/// <returns>True, if the values are identical; false, otherwise.</returns>
		public bool Equals(Guid other) => Equals(new Guid128(other));

		/// <summary>For implict equality comparisons between two guid.</summary>
		/// <param name="lhs">The left hand side Guid128.</param>
		/// <param name="rhs">The right hand side Guid128</param>
		/// <returns>True, if the values are identical; false, otherwise.</returns>
		public static bool operator ==(Guid128 lhs, Guid128 rhs) { return lhs.Equals(rhs); }

		/// <summary>For implict inequality comparisons between two guid.</summary>
		/// <param name="lhs">The left hand side Guid128.</param>
		/// <param name="rhs">The right hand side Guid128</param>
		/// <returns>True, if the values are identical; false, otherwise.</returns>
		public static bool operator !=(Guid128 lhs, Guid128 rhs) { return !(lhs == rhs); }

		/// <summary>Outputs to the default hex string of x8-x4-x4-x4-x8.</summary>
		/// <returns>The formatted hexidecimal string.</returns>
		public override string ToString() => ToString(_a, _b, _c, _d, _e, _f, _g, _h, _i, _j, _k);

		/// <summary>dddddddd-dddd-dddd-dddd-dddddddddddd</summary>
		/// <returns>The formating id.</returns>
		internal static string ToString(int _a, short _b, short _c, byte _d, byte _e, byte _f, byte _g, byte _h, byte _i, byte _j, byte _k)
		{
			//                      A  A  A  A  A  A  A  A - B  B   B   B -  C   C   C   C -  D   D   E   E -  F   F   G   G   H   H   I  I    J   J   K   K
			const string format = "{0}{1}{2}{3}{4}{5}{6}{7}-{8}{9}{10}{11}-{12}{13}{14}{15}-{16}{17}{18}{19}-{20}{21}{22}{23}{24}{25}{26}{27}{28}{29}{30}{31}";

			char _00 = HexToChar(_a >> 28);
			char _01 = HexToChar(_a >> 24);
			char _02 = HexToChar(_a >> 20);
			char _03 = HexToChar(_a >> 16);
			char _04 = HexToChar(_a >> 12);
			char _05 = HexToChar(_a >> 8);
			char _06 = HexToChar(_a >> 4);
			char _07 = HexToChar(_a);
			//char _08 = '-';
			char _09 = HexToChar(_b >> 12);
			char _10 = HexToChar(_b >> 8);
			char _11 = HexToChar(_b >> 4);
			char _12 = HexToChar(_b);
			//char _13 = '-';
			char _14 = HexToChar(_c >> 12);
			char _15 = HexToChar(_c >> 8);
			char _16 = HexToChar(_c >> 4);
			char _17 = HexToChar(_c);
			//char _18 = '-';
			char _19 = HexToChar(_d >> 4);
			char _20 = HexToChar(_d);
			char _21 = HexToChar(_e >> 4);
			char _22 = HexToChar(_e);
			//char _23 = '-';
			char _24 = HexToChar(_f >> 4);
			char _25 = HexToChar(_f);
			char _26 = HexToChar(_g >> 4);
			char _27 = HexToChar(_g);
			char _28 = HexToChar(_h >> 4);
			char _29 = HexToChar(_h);
			char _30 = HexToChar(_i >> 4);
			char _31 = HexToChar(_i);
			char _32 = HexToChar(_j >> 4);
			char _33 = HexToChar(_j);
			char _34 = HexToChar(_k >> 4);
			char _35 = HexToChar(_k);

			return string.Format(format, _00, _01, _02, _03, _04, _05, _06, _07, _09, _10, _11, _12, _14, _15, _16, _17, _19, _20, _21, _22, _24, _25, _26, _27, _28, _29, _30, _31, _32, _33, _34, _35);
		}

		internal static char HexToChar(int a)
		{
			a = a & 0xF;

			return (char)((a > 9) ? a - 10 + 0x61 : a + 0x30);
		}

		/// <summary>Attempts to parse the Guid128 using <see cref="Guid.TryParse(string, out Guid)"/>.</summary>
		/// <param name="value">The string value of the guid.</param>
		/// <param name="guid128">The resultant Guid128.</param>
		/// <returns>True, if successful; false, otherwise.</returns>
		public static bool TryParse(string value, out Guid128 guid128)
		{
			if (Guid.TryParse(value, out Guid guid))
			{
				guid128 = guid;
				return true;
			}

			guid128 = default;
			return false;
		}

		/// <summary></summary>
		/// <param name="guid"></param>
		public static implicit operator Guid128(Guid guid) => new Guid128(guid);

		/// <summary></summary>
		/// <param name="guid"></param>
		public static implicit operator Guid(Guid128 guid)
			=> new Guid(guid._a, guid._b, guid._c, 
						guid._d, guid._e, guid._f, guid._g, 
						guid._h, guid._i, guid._j, guid._k);

		/// <summary></summary>
		/// <returns></returns>
		public static Guid128 NewGuid128() => Guid.NewGuid();
	}
}