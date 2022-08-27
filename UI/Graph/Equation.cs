using System;

namespace Project
{
    abstract class Equation
    {

        protected double[] coefficients;
        protected double[] derivativeCoefficients;
        
        public Equation(Result result)
        {
            coefficients = CalculateCoefficient(result);
            derivativeCoefficients = CalculateDerivativeCoefficient();
        }

        public virtual double SolveForY(double x)
        {
            throw new NotImplementedException();
        }

        public virtual double SolveForX(double y)
        {
            throw new NotImplementedException();
        }

        public virtual double SolveDerivative(double x)
        {
            throw new NotImplementedException();
        }

        public virtual string Print()
        {
            throw new NotImplementedException();
        }

        public virtual double[] CalculateCoefficient(Result result)
        {
            throw new NotImplementedException();
        }

        public virtual double[] CalculateDerivativeCoefficient()
        {
            throw new NotImplementedException();
        }
    }

    class ConstantEquation : Equation
    {

        // y = [0]

        public ConstantEquation(Result result) : base(result)
        {

        }

        public override double SolveForY(double x)
        {
            return coefficients[0];
        }

        public override double SolveForX(double y)
        {
            return double.NaN;
        }

        public override double SolveDerivative(double x)
        {
            return derivativeCoefficients[0];
        }

        public override string Print()
        {
            return $"y = {Math.Round(coefficients[0], 5)}";
        }

        public override double[] CalculateCoefficient(Result result)
        {
            return new double[1] { MathHelper.Mean(result.times) };
        }

        public override double[] CalculateDerivativeCoefficient()
        {
            return new double[1] { 0 };
        }
    }

    class LinearEquation : Equation
    {

        // y = [0]x + [1]

        public LinearEquation(Result result) : base(result)
        {

        }

        public override double SolveForY(double x)
        {
            return coefficients[0] * x + coefficients[1];
        }

        public override double SolveForX(double y)
        {
            return (y - coefficients[1]) / coefficients[0];
        }

        public override double SolveDerivative(double x)
        {
            return derivativeCoefficients[0];
        }

        public override string Print()
        {
            return $"y = {coefficients[0]:E1}x + {Math.Round(coefficients[1], 2)}";
        }

        public override double[] CalculateCoefficient(Result result)
        {

            double c = result.times[0];

            double totalGradient = 0;
            for (int i = 1; i < result.times.Length; i++)
            {
                totalGradient += (double)(result.times[i] - result.times[i - 1]) / (result.inputs[i] - result.inputs[i - 1]);
            }
            double gradient = totalGradient / (result.times.Length - 1);

            return new double[2] { gradient, c };
        }

        public override double[] CalculateDerivativeCoefficient()
        {
            return new double[1] { coefficients[0] };
        }
    }

    class QuadraticEquation : Equation
    {

        // y = [0]x² + [1]
        // There is no x term as the graph will always have its turning point at x = 0

        public QuadraticEquation(Result result) : base(result)
        {

        }

        public override double SolveForY(double x)
        {
            return coefficients[0] * Math.Pow(x, 2) + coefficients[1];
        }

        public override double SolveForX(double y)
        {
            return Math.Sqrt((y - coefficients[1]) / coefficients[0]);
        }

        public override double SolveDerivative(double x)
        {
            return coefficients[0] * x;
        }

        public override string Print()
        {
            return $"y = {coefficients[0]:E2}x² + {coefficients[1]}";
        }

        public override double[] CalculateCoefficient(Result result)
        {

            double c = result.times[0];

            double totalA = 0;
            for (int i = 1; i < result.times.Length; i++)
            {
                totalA += (double)(result.times[i] - c) / Math.Pow(result.inputs[i], 2);
            }
            double a = totalA / (result.times.Length - 1);

            return new double[2] { a, c };
        }

        public override double[] CalculateDerivativeCoefficient()
        {
            return new double[1] { 2 * coefficients[0] };
        }
    }

    class CubicEquation : Equation
    {

        // y = [0]x³ + [1]
        // There is no x² or x term as the graph will always have its turning point at x = 0

        public CubicEquation(Result result) : base(result)
        {

        }

        public override double SolveForY(double x)
        {
            return coefficients[0] * Math.Pow(x, 3) + coefficients[1];
        }

        public override double SolveForX(double y)
        {
            return Math.Cbrt((y - coefficients[1]) / coefficients[0]);
        }

        public override double SolveDerivative(double x)
        {
            return derivativeCoefficients[0] * Math.Pow(x, 2);
        }

        public override string Print()
        {
            return $"y = {coefficients[0]:E2}x³ + {coefficients[1]}";
        }

        public override double[] CalculateCoefficient(Result result)
        {

            double c = result.times[0];

            // Uses later coordinates for accuracy

            double totalA = 0;
            for (int i = 3; i < result.times.Length; i++)
            {
                totalA += (double)(result.times[i] - c) / Math.Pow(result.inputs[i], 3);
            }
            double a = totalA / (result.times.Length - 3);

            return new double[2] { a, c };
        }

        public override double[] CalculateDerivativeCoefficient()
        {
            return new double[1] { 3 * coefficients[0] };
        }
    }

    class LogarithmicEquation : Equation
    {

        // y = [0] log(x) + [1]

        public LogarithmicEquation(Result result) : base(result)
        {

        }

        public override double SolveForY(double x)
        {
            return coefficients[0] * Math.Log2(x) + coefficients[1];
        }

        public override double SolveForX(double y)
        {
            return Math.Pow(2, (y - coefficients[1]) / coefficients[0]);
        }

        public override double SolveDerivative(double x)
        {
            return derivativeCoefficients[0] / x;
        }

        public override string Print()
        {
            return $"y = {Math.Round(coefficients[0], 2)}log(x) + {Math.Round(coefficients[1], 2)}";
        }

        public override double[] CalculateCoefficient(Result result)
        {

            double[] logInputs = new double[result.inputs.Length];

            for (int i = 1; i < result.inputs.Length; i++)
            {
                logInputs[i] = Math.Log2(result.inputs[i]);
            }

            double gradient = 0;
            for (int i = 2; i < result.times.Length; i++)
            {
                gradient += (result.times[i] - result.times[i - 1]) / (logInputs[i] - logInputs[i - 1]);
            }
            gradient /= result.times.Length - 2;

            double c = 0;
            for (int i = 1; i < result.times.Length; i++)
            {
                c += result.times[i] - (gradient * logInputs[i]);
            }
            c /= result.times.Length - 1;

            return new double[2] { gradient, c };
        }

        public override double[] CalculateDerivativeCoefficient()
        {
            return new double[1] { coefficients[0] / Math.Log(2) };
        }
    }

    class ExponentialEquation : Equation
    {

        // y = [0] * 2^x + [1]

        public ExponentialEquation(Result result) : base(result)
        {

        }

        public override double SolveForY(double x)
        {
            return coefficients[0] * Math.Pow(2, x) + coefficients[1];
        }

        public override double SolveForX(double y)
        {
            return Math.Log2((y - coefficients[1]) / coefficients[0]);
        }

        public override double SolveDerivative(double x)
        {
            return Math.Pow(2, x) * derivativeCoefficients[0];
        }

        public override string Print()
        {
            return $"y = {coefficients[0]:E2} * 2^x + {coefficients[1]}";
        }

        public override double[] CalculateCoefficient(Result result)
        {

            double c = result.times[0];

            double a = (double)(result.times[^1] - c) / Math.Pow(2, result.inputs[^1]);

            return new double[2] { a, c };
        }

        public override double[] CalculateDerivativeCoefficient()
        {
            return new double[1] { Math.Log(2) * coefficients[0] };
        }
    }
}