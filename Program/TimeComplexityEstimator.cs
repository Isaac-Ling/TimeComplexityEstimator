using System;
using System.Collections.Generic;

namespace Project
{
    class TimeComplexityEstimator
    {

        public readonly Algorithm algorithm;
        private readonly Complexity[] complexity = new Complexity[]
        {
            Complexity.Constant,
            Complexity.Linear,
            Complexity.Quadratic,
            Complexity.Cubic,
            Complexity.Logarithmic,
            Complexity.Exponential
        };
        private readonly Dictionary<Complexity, string> complexityToBigO = new Dictionary<Complexity, string>()
        {
            { Complexity.Constant, "1" },
            { Complexity.Logarithmic, "log(n)"},
            { Complexity.Linear, "n"},
            { Complexity.Quadratic, "n²"},
            { Complexity.Cubic, "n³"},
            { Complexity.Exponential, "2^n"},
        };

        public Result ResultOfAlgorithm { get; private set; }
        public bool IsComplete { get; private set; }

        public TimeComplexityEstimator(Algorithm algorithm)
        {
            this.algorithm = algorithm;
        }

        /// <summary>
        /// Will gather results on the inputted algorithm and will put the results in the ResultOfAlgorithm field
        /// </summary>
        public void Calculate()
        {

            int order = Order(algorithm.Times);

            if (IsConstant(order))
            {
                ResultOfAlgorithm = new Result(Complexity.Constant, complexityToBigO[Complexity.Constant], algorithm.Times, algorithm.Inputs);
            }
            else if (IsExponential(order))
            {
                ResultOfAlgorithm = new Result(Complexity.Exponential, complexityToBigO[Complexity.Exponential], algorithm.Times, algorithm.Inputs);
            }
            else if (IsLogarithmic(algorithm.Times))
            {
                ResultOfAlgorithm = new Result(Complexity.Logarithmic, complexityToBigO[Complexity.Logarithmic], algorithm.Times, algorithm.Inputs);
            }
            else
            {
                int depth = Math.Min(order, 3);
                ResultOfAlgorithm = new Result(complexity[depth], complexityToBigO[complexity[depth]], algorithm.Times, algorithm.Inputs);
            }

            IsComplete = true;
        }

        private bool IsConstant(int order)
        {
            return order == 0;
        }

        private bool IsLogarithmic(int[] times)
        {

            // By raising a constant to the power of the times, a logarithmic function
            // will reduce to a linear function

            float expBase = 1.01f;
            int[] expOftimes = new int[times.Length];
            times.CopyTo(expOftimes, 0);

            // Calibrating expBase to avoid overflow

            int result;
            int input = LargestValueInArray(times);

            if ((int)Math.Pow(expBase, input) < 0)
            {
                do
                {
                    result = (int)Math.Pow(expBase, input);
                    expBase -= 0.001f;

                } while (result < 0);
            }

            for (int i = 0; i < expOftimes.Length; i++)
            {
                expOftimes[i] = (int)Math.Pow(expBase, times[i]);
            }

            return Order(expOftimes) == 1;
        }

        private bool IsExponential(int order)
        {

            // Since the differences between subsequent y values for an exponential graph 
            // are also exponential, the depth would be big compared to other algorithms

            return order > 3;
        }

        private int LargestValueInArray(int[] times)
        {
            int largest = times[0];

            for (int i = 1; i < times.Length; i++)
            {
                if (times[i] > largest)
                {
                    largest = times[i];
                }
            }

            return largest;
        }

        /// <summary>
        /// Returns the order of times
        /// </summary>
        private int Order(int[] times, int depth = 0, double previousDeviation = 0)
        {

            double deviation = MathHelper.StandardDeviation(times);
            if (depth == 0)
                previousDeviation = deviation;

            //Base Case
            if (deviation > previousDeviation || times.Length == 1)
            {
                return depth - 1;
            }
            else
            {
                int[] derivativeOftimes = Differentiate(times);

                depth++;

                previousDeviation = deviation;

                return Order(derivativeOftimes, depth, previousDeviation);
            }
        }

        /// <summary>
        /// Returns the derivative of times, given that the inputs have a constant difference
        /// </summary>
        private int[] Differentiate(int[] times)
        {

            int[] derivativeOftimes = new int[times.Length - 1];

            for (int i = 0; i < times.Length - 1; i++)
            {
                derivativeOftimes[i] = times[i + 1] - times[i];
            }

            return derivativeOftimes;
        }
    }

    /// <summary>
    /// Immutable wrapper for the result generated from TimeComplexityEstimator calculations
    /// </summary>
    struct Result
    {

        public readonly Complexity complexity;
        public readonly string bigO;
        public readonly int[] times;
        public readonly int[] inputs;

        public Result(Complexity complexity, string bigO, int[] times, int[] inputs)
        {
            this.complexity = complexity;
            this.bigO = "O(" + bigO + ")";
            this.times = times;
            this.inputs = inputs;
        }
    }

    enum Complexity
    {
        Constant,
        Logarithmic,
        Linear,
        Quadratic,
        Cubic,
        Exponential,
        Factorial
    }
}