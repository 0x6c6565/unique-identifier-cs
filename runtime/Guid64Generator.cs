using System;
using System.Linq;
using System.Net.NetworkInformation;

namespace UniqueIdentifiers
{
	/// <summary>
	/// 64 bit guid simplified version of snowflake without the data centers...
	/// </summary>
	internal class Guid64Generator
	{
		/// <summary>The mask for the bits used to capture the milliseconds since epoch.</summary>
		public const ulong epochMask = ulong.MaxValue >> (64 - epochBits);
		const int epochBits = 41;

		/// <summary>The value by which to mod the mac address.</summary>
		public const ulong macAddressModulus = 1 << macAddressBits;
		const int macAddressBits = 11;

		/// <summary>The mask to capture the millisecond-wise incrementor.</summary>
		public const int counterMask = 1 << counterBits;
		const int counterBits = 12;

		/// <summary>The starting time for all guids is January, 1, 2020 @midnight.</summary>
		public static readonly long epochStart
			= new DateTimeOffset(new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).ToUnixTimeMilliseconds();

		/// <summary>Captures the first valid mac address for use in guid generation.</summary>
		public static readonly string firstValidMacAddress
			= (from nic in NetworkInterface.GetAllNetworkInterfaces()
			   where nic.OperationalStatus == OperationalStatus.Up
			   select nic.GetPhysicalAddress().ToString()).FirstOrDefault() ?? string.Empty;

		static readonly ulong firstValidMacAddressValue = Convert.ToUInt64(firstValidMacAddress, 0x10);

		static readonly ulong hashedFirstValidMacAddressValue = firstValidMacAddressValue % macAddressModulus;

		static readonly ulong hardwareId = hashedFirstValidMacAddressValue << counterBits;

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
		static Guid64Generator instance { get; set; } = null;
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

		int counter = 0;
		long lastCounterMillisecond = 0;

		long GetMilliseconds() => DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
		long GetMillisecondsSinceEpoch() => GetMilliseconds() - epochStart;
		long GetMillisecondsSinceEpoch(in long milliseconds) => milliseconds - epochStart;
		ulong GetMillisecondsSinceEpochMasked()
		{
			unchecked
			{
				return (ulong)GetMillisecondsSinceEpoch() & epochMask;
			}
		}
		ulong GetMillisecondsSinceEpochMasked(in long milliseconds)
		{
			unchecked
			{
				return (ulong)GetMillisecondsSinceEpoch(milliseconds) & epochMask;
			}
		}

		ulong GetMillisecondsSinceEpochMaskedAndShifted(in long milliseconds)
			=> GetMillisecondsSinceEpochMasked(milliseconds) << (macAddressBits + counterBits);

		/// <summary>
		/// Generates a new Guid64 object.<br />
		/// Use <see cref="Guid64.NewGuid64"/> instead.<br />
		/// <br />
		///	41 bits for timestamp - milliseconds since epoch<br />
		///	11 bits mac address<br />
		///	12 bits sequence number
		/// </summary>
		/// <returns>A reasonably unique identifier.</returns>
		public Guid64 GenerateGuid()
		{
			long currentMillisecond = GetMilliseconds();

			if (lastCounterMillisecond < currentMillisecond)
			{
				counter = 0;

				lastCounterMillisecond = currentMillisecond;
			}

			ulong milliseconds = GetMillisecondsSinceEpochMaskedAndShifted(currentMillisecond);

			ulong index = (ulong)counter++;

			return new Guid64(milliseconds | hardwareId | index);
		}

		/// <summary>
		/// Generates a new Guid64 object.<br />
		/// Use <see cref="Guid64.NewGuid64"/> instead.<br />
		/// <br />
		/// </summary>
		/// <returns>The newly generated Guid64.</returns>
		public static Guid64 Generate() => GetInstance().GenerateGuid();

		internal static Guid64Generator GetInstance()
		{
			if (null == instance)
			{
				instance = new Guid64Generator();
			}

			return instance;
		}
	}
}
