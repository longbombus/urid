using System;
using System.Runtime.InteropServices;

namespace URID
{
	[StructLayout(LayoutKind.Explicit, Size = BytesCount)]
	public unsafe struct URID128 : IEquatable<URID128>, IComparable<URID128>
	{
		public const int BytesCount = 16;
		public const int BitsCount = BytesCount * 8;

		[FieldOffset(0)] public fixed byte Bytes[16];
		[FieldOffset(0)] public ulong High;
		[FieldOffset(8)] public ulong Low;

		public URID128(Guid guid)
		{
			Low = default;
			High = default;
			fixed (byte* BytesPtr = Bytes)
			{
				guid.TryWriteBytes(new Span<byte>(BytesPtr, BytesCount));
			}
		}

		public static implicit operator Guid(URID128 urid)
			=> new Guid(new ReadOnlySpan<byte>(urid.Bytes, BytesCount));

        public int CompareTo(URID128 other)
			=> High != other.High
				? High.CompareTo(other.High)
				: Low.CompareTo(other.Low);

        public readonly bool Equals(URID128 other)
			=> Low == other.Low && High == other.High;

		public override readonly bool Equals(object obj)
			=> obj is URID128 other && Low == other.Low && High == other.High;

		public override readonly int GetHashCode()
			=> (int)(Low ^ (Low >> 32) ^ High ^ (High >> 32));
	}
}