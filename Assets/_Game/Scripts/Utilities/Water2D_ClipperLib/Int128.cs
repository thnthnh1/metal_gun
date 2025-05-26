using System;

namespace Water2D_ClipperLib
{
	internal struct Int128
	{
		private long hi;

		private ulong lo;

		public Int128(long _lo)
		{
			this.lo = (ulong)_lo;
			if (_lo < 0L)
			{
				this.hi = -1L;
			}
			else
			{
				this.hi = 0L;
			}
		}

		public Int128(long _hi, ulong _lo)
		{
			this.lo = _lo;
			this.hi = _hi;
		}

		public Int128(Int128 val)
		{
			this.hi = val.hi;
			this.lo = val.lo;
		}

		public bool IsNegative()
		{
			return this.hi < 0L;
		}

		public static bool operator ==(Int128 val1, Int128 val2)
		{
			return val1 == val2 || (val1 != null && val2 != null && val1.hi == val2.hi && val1.lo == val2.lo);
		}

		public static bool operator !=(Int128 val1, Int128 val2)
		{
			return !(val1 == val2);
		}

		public override bool Equals(object obj)
		{
			if (obj == null || !(obj is Int128))
			{
				return false;
			}
			Int128 @int = (Int128)obj;
			return @int.hi == this.hi && @int.lo == this.lo;
		}

		public override int GetHashCode()
		{
			return this.hi.GetHashCode() ^ this.lo.GetHashCode();
		}

		public static bool operator >(Int128 val1, Int128 val2)
		{
			if (val1.hi != val2.hi)
			{
				return val1.hi > val2.hi;
			}
			return val1.lo > val2.lo;
		}

		public static bool operator <(Int128 val1, Int128 val2)
		{
			if (val1.hi != val2.hi)
			{
				return val1.hi < val2.hi;
			}
			return val1.lo < val2.lo;
		}

		public static Int128 operator +(Int128 lhs, Int128 rhs)
		{
			lhs.hi += rhs.hi;
			lhs.lo += rhs.lo;
			if (lhs.lo < rhs.lo)
			{
				lhs.hi += 1L;
			}
			return lhs;
		}

		public static Int128 operator -(Int128 lhs, Int128 rhs)
		{
			return lhs + -rhs;
		}

		public static Int128 operator -(Int128 val)
		{
			if (val.lo == 0uL)
			{
				return new Int128(-val.hi, 0uL);
			}
			return new Int128(~val.hi, ~val.lo + 1uL);
		}

		public static explicit operator double(Int128 val)
		{
            const double shift64 = 18446744073709551616.0; // 2^64

            if (val.hi >= 0) return val.lo + val.hi * shift64;

            if (val.lo == 0)
            {
                return val.hi * shift64;
            }

            return -(~val.lo + ~val.hi * shift64);
        }

		public static Int128 Int128Mul(long lhs, long rhs)
		{
            var negate = lhs < 0 != rhs < 0;
            if (lhs < 0) lhs = -lhs;
            if (rhs < 0) rhs = -rhs;
            var int1Hi = (ulong)lhs >> 32;
            var int1Lo = (ulong)lhs & 0xFFFFFFFF;
            var int2Hi = (ulong)rhs >> 32;
            var int2Lo = (ulong)rhs & 0xFFFFFFFF;

            // nb: see comments in clipper.pas
            var a = int1Hi * int2Hi;
            var b = int1Lo * int2Lo;
            var c = int1Hi * int2Lo + int1Lo * int2Hi;

            ulong lo;
            var hi = (long)(a + (c >> 32));

            unchecked { lo = (c << 32) + b; }
            if (lo < b) hi++;
            var result = new Int128(hi, lo);
            return negate ? -result : result;
        }
	}
}
