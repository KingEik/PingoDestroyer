using System;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;

namespace PingoDestroyer
{
    class IPSpoofing
    {

        private class IPRange
        {
            readonly AddressFamily addressFamily;
            readonly byte[] lowerBytes;
            readonly byte[] upperBytes;

            public IPRange(byte[] lower, byte[] upper)
            {
                this.lowerBytes = lower;
                this.upperBytes = upper;
                this.addressFamily = new IPAddress(lower).AddressFamily;
            }

            public IPRange(IPAddress lower, IPAddress upper)
            {
                this.addressFamily = lower.AddressFamily;
                this.lowerBytes = lower.GetAddressBytes();
                this.upperBytes = upper.GetAddressBytes();
            }

            public bool IsInRange(IPAddress ip)
            {
                if (ip.AddressFamily != addressFamily) return false;

                byte[] addressBytes = ip.GetAddressBytes();

                bool lowerBoundary = true, upperBoundary = true;

                for (int i = 0; i < lowerBytes.Length && (lowerBoundary || upperBoundary); i++)
                {
                    if ((lowerBoundary && addressBytes[i] < lowerBytes[i]) ||
                        (upperBoundary && addressBytes[i] > upperBytes[i]))
                        return false;

                    lowerBoundary &= (addressBytes[i] == lowerBytes[i]);
                    upperBoundary &= (addressBytes[i] == upperBytes[i]);
                }
                return true;
            }

            public static bool IsReservedIP(IPAddress ip)
            {
                // all reserved IP ranges
                IPRange[] ranges =
                {
                    new IPRange(new byte[] {000,000,000,000}, new byte[] {002,255,255,255}),
                    new IPRange(new byte[] {010,000,000,000}, new byte[] {010,255,255,255}),
                    new IPRange(new byte[] {100,064,000,000}, new byte[] {100,127,255,255}),
                    new IPRange(new byte[] {127,000,000,000}, new byte[] {127,255,255,255}),
                    new IPRange(new byte[] {169,254,000,000}, new byte[] {169,254,255,255}),
                    new IPRange(new byte[] {172,016,000,000}, new byte[] {172,031,255,255}),
                    new IPRange(new byte[] {192,000,000,000}, new byte[] {192,000,000,255}),
                    new IPRange(new byte[] {192,000,002,000}, new byte[] {192,000,002,255}),
                    new IPRange(new byte[] {192,031,196,000}, new byte[] {192,031,196,255}),
                    new IPRange(new byte[] {192,052,193,000}, new byte[] {192,052,193,255}),
                    new IPRange(new byte[] {192,088,099,000}, new byte[] {192,088,099,255}),
                    new IPRange(new byte[] {192,168,000,000}, new byte[] {192,168,255,255}),
                    new IPRange(new byte[] {192,175,048,000}, new byte[] {192,175,048,255}),
                    new IPRange(new byte[] {198,018,000,000}, new byte[] {198,019,255,255}),
                    new IPRange(new byte[] {198,051,100,000}, new byte[] {198,051,100,255}),
                    new IPRange(new byte[] {203,000,113,000}, new byte[] {203,000,113,255}),
                    new IPRange(new byte[] {224,000,000,000}, new byte[] {255,255,255,255})
                };

                foreach (IPRange range in ranges)
                    if (range.IsInRange(ip))
                        return true;

                return false;
            }
        }

        // ==++==
        // 
        //   Copyright (c) Microsoft Corporation.  All rights reserved.
        // 
        // ==--==
        /*============================================================
        **
        ** Class:  Random
        **
        **
        ** Purpose: A random number generator.
        **
        ** 
        ===========================================================*/
        [System.Runtime.InteropServices.ComVisible(true)]
        [Serializable]
        public class MSRandom
        {
            //
            // Private Constants 
            //
            private const int MBIG = Int32.MaxValue;
            private const int MSEED = 161803398;
            private const int MZ = 0;


            //
            // Member Variables
            //
            private int inext;
            private int inextp;
            private int[] SeedArray = new int[56];

            //
            // Public Constants
            //

            //
            // Native Declarations
            //

            //
            // Constructors
            //

            public MSRandom()
              : this(Environment.TickCount)
            {
            }

            public MSRandom(int Seed)
            {
                int ii;
                int mj, mk;

                //Initialize our Seed array.
                //This algorithm comes from Numerical Recipes in C (2nd Ed.)
                int subtraction = (Seed == Int32.MinValue) ? Int32.MaxValue : Math.Abs(Seed);
                mj = MSEED - subtraction;
                SeedArray[55] = mj;
                mk = 1;
                for (int i = 1; i < 55; i++)
                {  //Apparently the range [1..55] is special (Knuth) and so we're wasting the 0'th position.
                    ii = (21 * i) % 55;
                    SeedArray[ii] = mk;
                    mk = mj - mk;
                    if (mk < 0) mk += MBIG;
                    mj = SeedArray[ii];
                }
                for (int k = 1; k < 5; k++)
                {
                    for (int i = 1; i < 56; i++)
                    {
                        SeedArray[i] -= SeedArray[1 + (i + 30) % 55];
                        if (SeedArray[i] < 0) SeedArray[i] += MBIG;
                    }
                }
                inext = 0;
                inextp = 21;
                Seed = 1;
            }

            //
            // Package Private Methods
            //

            /*====================================Sample====================================
            **Action: Return a new random number [0..1) and reSeed the Seed array.
            **Returns: A double [0..1)
            **Arguments: None
            **Exceptions: None
            ==============================================================================*/
            protected virtual double Sample()
            {
                //Including this division at the end gives us significantly improved
                //random number distribution.
                return (InternalSample() * (1.0 / MBIG));
            }

            private int InternalSample()
            {
                int retVal;
                int locINext = inext;
                int locINextp = inextp;

                if (++locINext >= 56) locINext = 1;
                if (++locINextp >= 56) locINextp = 1;

                retVal = SeedArray[locINext] - SeedArray[locINextp];

                if (retVal == MBIG) retVal--;
                if (retVal < 0) retVal += MBIG;

                SeedArray[locINext] = retVal;

                inext = locINext;
                inextp = locINextp;

                return retVal;
            }

            //
            // Public Instance Methods
            // 


            /*=====================================Next=====================================
            **Returns: An int [0..Int32.MaxValue)
            **Arguments: None
            **Exceptions: None.
            ==============================================================================*/
            public virtual int Next()
            {
                return InternalSample();
            }

            private double GetSampleForLargeRange()
            {
                // The distribution of double value returned by Sample 
                // is not distributed well enough for a large range.
                // If we use Sample for a range [Int32.MinValue..Int32.MaxValue)
                // We will end up getting even numbers only.

                int result = InternalSample();
                // Note we can't use addition here. The distribution will be bad if we do that.
                bool negative = (InternalSample() % 2 == 0) ? true : false;  // decide the sign based on second sample
                if (negative)
                {
                    result = -result;
                }
                double d = result;
                d += (Int32.MaxValue - 1); // get a number in range [0 .. 2 * Int32MaxValue - 1)
                d /= 2 * (uint)Int32.MaxValue - 1;
                return d;
            }


            /*=====================================Next=====================================
            **Returns: An int [minvalue..maxvalue)
            **Arguments: minValue -- the least legal value for the Random number.
            **           maxValue -- One greater than the greatest legal return value.
            **Exceptions: None.
            ==============================================================================*/
            public virtual int Next(int minValue, int maxValue)
            {
                if (minValue > maxValue)
                {
                    throw new ArgumentOutOfRangeException("minValue");
                }

                long range = (long)maxValue - minValue;
                if (range <= (long)Int32.MaxValue)
                {
                    return ((int)(Sample() * range) + minValue);
                }
                else
                {
                    return (int)((long)(GetSampleForLargeRange() * range) + minValue);
                }
            }


            /*=====================================Next=====================================
            **Returns: An int [0..maxValue)
            **Arguments: maxValue -- One more than the greatest legal return value.
            **Exceptions: None.
            ==============================================================================*/
            public virtual int Next(int maxValue)
            {
                if (maxValue < 0)
                {
                    throw new ArgumentOutOfRangeException("maxValue", "maxValue must be positive");
                }
                return (int)(Sample() * maxValue);
            }


            /*=====================================Next=====================================
            **Returns: A double [0..1)
            **Arguments: None
            **Exceptions: None
            ==============================================================================*/
            public virtual double NextDouble()
            {
                return Sample();
            }


            /*==================================NextBytes===================================
            **Action:  Fills the byte array with random bytes [0..0x7f].  The entire array is filled.
            **Returns:Void
            **Arugments:  buffer -- the array to be filled.
            **Exceptions: None
            ==============================================================================*/
            public virtual void NextBytes(byte[] buffer)
            {
                if (buffer == null) throw new ArgumentNullException("buffer");
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer[i] = (byte)(InternalSample() % (Byte.MaxValue + 1));
                }
            }
        }

        public static int getHashCode(String input) {
            MD5 md5 = MD5.Create();
            byte[] hashed = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
            return BitConverter.ToInt32(hashed, 0);
        }

        public static IPAddress GenerateIPAddress()
        {
            return GenerateIPAddress(Environment.TickCount);
        }

        public static IPAddress GenerateIPAddress(String userName)
        {
            return GenerateIPAddress(getHashCode(userName.ToLower()));
        }

        public static IPAddress GenerateIPAddress(int seed)
        {
            byte[] result = new byte[4];
            MSRandom rand = new MSRandom(seed);

            bool validAddressFound = false;
            IPAddress r = IPAddress.None;

            while (!validAddressFound)
            {
                rand.NextBytes(result);
                r = new IPAddress(result);

                validAddressFound = !IPRange.IsReservedIP(r);
            }

            return r;
        }
    }
}
