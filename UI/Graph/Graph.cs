using System;
using System.Collections.Generic;

namespace Project
{
    class Graph : IDrawable
    {

        public const int NumOfNodes = 10;
        public const int DefaultXAxisLength = 50;
        public const int DefaultYAxisLength = 20;

        public static readonly ConsoleColor[] Colors = new ConsoleColor[5] { ConsoleColor.White, ConsoleColor.Magenta, ConsoleColor.Green, ConsoleColor.Yellow, ConsoleColor.Gray };

        private readonly List<Node[]> nodes = new List<Node[]>();
        private int[] xAxis;
        private int[] yAxis;
        private Coordinate[,] points;
        private float xInterval;
        private int yInterval;
        private int xAxisScaleFactor;
        public int numOfResults;

        public int XAxisLength { get; private set; }
        public int YAxisLength { get; private set; }
        public int NodeIndex { private get; set; }
        public int ResultIndex { private get; set; }
        public List<Equation> Equations { get; private set; } = new List<Equation>();
        public List<Result> Results { get; private set; } = new List<Result>();

        public Graph(List<Result> results, int xAxisLength = DefaultXAxisLength, int yAxisLength = DefaultYAxisLength)
        {
            Results = results;
            numOfResults = results.Count;
            XAxisLength = xAxisLength;
            YAxisLength = yAxisLength;
            SetNodes(results);
            SetXAxis(GreatestX());
            SetYAxis(GreatestY());
            AddEquation(results);
            SetPoints();
        }

        public void Draw()
        {

            Graphics.PrintLine($"Time (ms)", ConsoleColor.Cyan);

            for (int y = 0; y < YAxisLength; y++)
            {

                Graphics.Print($"{yAxis[^(y + 1)].ToString().PadRight(yAxis[^1].ToString().Length)}│", ConsoleColor.DarkCyan);

                for (int x = 0; x < XAxisLength; x++)
                {
                    points[y, x].Draw();
                }

                Console.WriteLine();
            }

            Graphics.Print($"{new string(' ', yAxis[^1].ToString().Length)}└{new string('─', XAxisLength)}", ConsoleColor.DarkCyan);
            Console.WriteLine();
            Console.Write(new string(' ', yAxis[^1].ToString().Length + 1));

            for (int x = 0; x < XAxisLength; x += 3)
            {
                string num = xAxis[x].ToString().PadLeft(Math.Max(xAxis[^1].ToString().Length, 2), '0');
                Graphics.Print(num.ToCharArray()[0].ToString() + num.ToCharArray()[1].ToString() + " ", ConsoleColor.DarkCyan);
            }

            Console.WriteLine();
            Graphics.PrintLine($"{new string(' ', yAxis[^1].ToString().Length + 1 + XAxisLength / 2 - (8 + Convert.ToDouble(xAxisScaleFactor).ToString("E1").Length) / 2)}Input (x{Convert.ToDouble(xAxisScaleFactor):E1})", ConsoleColor.Cyan);
            Console.WriteLine("\n");
        }

        private void RefreshNodesOfCurrentResult()
        {
            foreach (Node node in nodes[ResultIndex])
            {
                if (node.isVisible)
                {
                    Console.SetCursorPosition(yAxis[^1].ToString().Length + 1 + (int)(node.position.X / xInterval), 5 + YAxisLength - (int)(node.position.Y / yInterval));
                    node.Draw();
                }
            }
        }

        public void RefreshInterrogateDraw()
        {
            RefreshNodesOfCurrentResult();
            Console.SetCursorPosition(6, 11 + YAxisLength);
            nodes[ResultIndex][NodeIndex].PrintTime();
            Console.SetCursorPosition(12, 12 + YAxisLength);
            nodes[ResultIndex][NodeIndex].PrintInput();
            Console.SetCursorPosition(0, 13 + YAxisLength);
            Graphics.Print($"From algorithm: ", ConsoleColor.White);
            Graphics.Print($"'{Results[ResultIndex].complexity}'{new string(' ', Console.WindowWidth - Results[ResultIndex].complexity.ToString().Length - 18)}", Colors[ResultIndex]);
            Console.SetCursorPosition(0, 15 + YAxisLength);
        }

        /// <summary>
        /// Changes the currently selected node
        /// </summary>
        /// <param name="resultIndex"> Pass '-1' to remove the selected node </param>
        public void ChangeSelectedNode(int resultIndex, int nodeIndex = 0)
        {
            nodes[ResultIndex][NodeIndex].IsSelected = false;
            if (resultIndex != -1)
            {
                ResultIndex = resultIndex;
                NodeIndex = nodeIndex;
                nodes[ResultIndex][NodeIndex].IsSelected = true;
            }

            RefreshNodesOfCurrentResult();
        }

        public bool IsNodeVisible(int resultIndex, int nodeIndex)
        {
            return nodes[resultIndex][nodeIndex].isVisible;
        }

        private void SetNodes(List<Result> results)
        {
            int colorCount = 0;

            foreach (Result result in results)
            {
                nodes.Add(new Node[result.times.Length]);
                for (int i = 0; i < result.times.Length; i++)
                {
                    nodes[^1][i] = new Node(new Vector(result.inputs[i], result.times[i]), Colors[colorCount % 5]);
                }
                colorCount++;
            }
        }

        public void SetXAxis(int end)
        {

            xAxis = new int[XAxisLength];

            xInterval = (float)end / (XAxisLength - 1);
            float totalX = 0;

            for (int i = 0; i < XAxisLength; i++)
            {
                xAxis[i] = (int)totalX;
                totalX += xInterval;
            }

            xAxisScaleFactor = (int)Math.Pow(10, Math.Max(0, xAxis[^1].ToString().Length - 2));

            SetPoints();
        }

        public void SetYAxis(int end)
        {

            yAxis = new int[YAxisLength];

            yInterval = (int)Math.Ceiling((float)end / (YAxisLength - 1));
            float totalY = 0;

            for (int i = 0; i < YAxisLength; i++)
            {
                yAxis[i] = (int)totalY;
                totalY += yInterval;
            }

            SetPoints();
        }

        public void SetXAxisLength(int XAxisLength)
        {
            this.XAxisLength = XAxisLength;
            SetXAxis(xAxis[^1]);
            SetPoints();
        }

        public void SetYAxisLength(int YAxisLength)
        {
            this.YAxisLength = YAxisLength;
            SetYAxis(yAxis[^1]);
            SetPoints();
        }

        private void SetPoints()
        {

            int i = 0;
            int xIndex;
            int yIndex;

            points = new Coordinate[YAxisLength, XAxisLength];

            for (int y = 1; y <= YAxisLength; y++)
            {
                for (int x = 0; x < XAxisLength; x++)
                {
                    points[YAxisLength - y, x] = new Coordinate(new Vector((int)(x * xInterval), (int)(y * yInterval)), " ", ConsoleColor.Black);
                }
            }

            foreach (Equation equation in Equations)
            {
                xIndex = 0;
                yIndex = 0;
                List<Node> nodesToDraw = new List<Node>();

                foreach (Node node in nodes[i])
                {
                    node.isVisible = false;
                    nodesToDraw.Add(node);
                }

                for (float y = yAxis[0]; y <= yAxis[^1]; y += yInterval)
                {
                    for (float x = xAxis[0]; x <= xAxis[^1]; x += xInterval)
                    {
                        string symbol = " ";
                        double solveForY = equation.SolveForY(x);
                        double solveForYDecimal = (solveForY % yInterval) / yInterval;
                        double solveForX = equation.SolveForX(y);
                        double solveForXDecimal = (solveForX % xInterval) / xInterval;

                        if ((int)(solveForY / yInterval) == (int)(y / yInterval))
                        {
                            if (solveForYDecimal < 0.33)
                            {
                                symbol = ".";
                            }
                            else if (solveForYDecimal > 0.66)
                            {
                                symbol = "'";
                            }
                            else
                            {
                                symbol = "·";
                            }

                            points[YAxisLength - 1 - yIndex, xIndex] = new Coordinate(new Vector((int)x, (int)y), symbol, Colors[i]);
                        }
                        else if (Math.Ceiling(solveForX / xInterval) == xIndex)
                        {
                            symbol = "·";

                            points[YAxisLength - 1 - yIndex, xIndex] = new Coordinate(new Vector((int)x, (int)y), symbol, Colors[i]);
                        }

                        foreach (Node node in nodesToDraw)
                        {
                            if ((int)(node.position.X / xInterval) == xIndex && node.position.Y / yInterval == yIndex)
                            {
                                node.isVisible = true;
                                nodesToDraw.Remove(node);
                                points[YAxisLength - 1 - yIndex, xIndex] = node;
                                break;
                            }
                        }

                        xIndex++;
                    }

                    xIndex = 0;
                    yIndex++;
                }

                i++;
            }
        }

        public void AddEquation(List<Result> results)
        {
            foreach (Result result in results)
            {
                switch (result.complexity)
                {
                    case Complexity.Constant:
                        Equations.Add(new ConstantEquation(result));
                        break;
                    case Complexity.Logarithmic:
                        Equations.Add(new LogarithmicEquation(result));
                        break;
                    case Complexity.Linear:
                        Equations.Add(new LinearEquation(result));
                        break;
                    case Complexity.Quadratic:
                        Equations.Add(new QuadraticEquation(result));
                        break;
                    case Complexity.Cubic:
                        Equations.Add(new CubicEquation(result));
                        break;
                    case Complexity.Exponential:
                        Equations.Add(new ExponentialEquation(result));
                        break;
                }
            }
        }

        public Vector FindIntersection(Equation equation1, Equation equation2)
        {

            double xIntersect = 1;

            for (int i = 0; i < 100; i++)
            {
                xIntersect = xIntersect - (equation1.SolveForY(xIntersect) - equation2.SolveForY(xIntersect)) / (equation1.SolveDerivative(xIntersect) - equation2.SolveDerivative(xIntersect));
            }

            return new Vector((int)xIntersect, (int)equation1.SolveForY(xIntersect));
        }

        public int GreatestX()
        {
            
            int greatestX = nodes[0][0].position.X;

            foreach (Node[] nodeArray in nodes)
            {
                for (int i = 1; i < nodeArray.Length; i++)
                {
                    if (nodeArray[i].position.X > greatestX)
                    {
                        greatestX = nodeArray[i].position.X;
                    }
                }
            }

            return greatestX;
        }

        public int GreatestY()
        {
            int greatestY = nodes[0][0].position.Y;

            foreach (Node[] nodeArray in nodes)
            {
                for (int i = 1; i < nodeArray.Length; i++)
                {
                    if (nodeArray[i].position.Y > greatestY)
                    {
                        greatestY = nodeArray[i].position.Y;
                    }
                }
            }

            return greatestY;
        }

        public struct Vector
        {
            public int X { get; set; }
            public int Y { get; set; }

            public Vector(int x, int y)
            {
                X = x;
                Y = y;
            }
        }

        private class Coordinate : IDrawable
        {

            public readonly Vector position;
            public readonly ConsoleColor color;
            protected readonly string symbol;

            public Coordinate(Vector position, string symbol, ConsoleColor color)
            {
                this.position = position;
                this.symbol = symbol;
                this.color = color;
            }

            public virtual void Draw()
            {
                Graphics.Print(symbol, color);
            }
        }

        private class Node : Coordinate
        {

            public bool IsSelected { private get; set; }
            public bool isVisible;

            public Node(Vector position, ConsoleColor color) : base(position, "X", color)
            {

            }

            public override void Draw()
            {
                Graphics.Print(symbol, IsSelected ? ConsoleColor.Red : color);
            }

            public void PrintInput()
            {
                Graphics.Print($"{position.X}{new string(' ', Console.WindowWidth - position.X.ToString().Length - 12)}", ConsoleColor.White);
            }

            public void PrintTime()
            {
                Graphics.Print($"{position.Y}ms{new string(' ', Console.WindowWidth - position.Y.ToString().Length - 8)}", ConsoleColor.White);
            }
        }

        public enum Axis
        {
            input,
            time
        }
    }
}