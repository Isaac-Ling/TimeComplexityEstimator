using System;

namespace Project
{
    static class MathHelper
    {

        public static double StandardDeviation(int[] nums)
        {

            double meanDeviationSquared = SumOfDeviationsSquared(nums, Mean(nums));

            return Math.Sqrt(meanDeviationSquared / nums.Length);
        }

        private static double SumOfDeviationsSquared(int[] nums, double mean)
        {

            double total = 0;

            for (int i = 0; i < nums.Length; i++)
            {
                total += Math.Pow(nums[i] - mean, 2);
            }

            return total;
        }

        public static double Mean(int[] nums)
        {

            double total = 0;

            for (int i = 0; i < nums.Length; i++)
            {
                total += nums[i];
            }

            return total / nums.Length;
        }
    }
}
