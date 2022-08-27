using System;
using System.Diagnostics;
using System.IO;

namespace Project
{
    class Algorithm
    {

        private const int MaxDifference = 50000000;
        private const int Iterations = 10;

        private readonly string fileName;
        private readonly string exeName;
        private int inputSize = 0;
        private int difference = 1;
        private long performedIterations = 0;
        private long totalSize;

        public int PercentComplete { get; private set; }
        public bool IsComplete { get; private set; }
        public Exception ExitException { get; private set; }
        public bool HasCalibratedData { get; private set; }
        public bool Abort { get; private set; }
        public int[] Times { get; private set; }
        public int[] Inputs { get; private set; }

        public Algorithm(string fileName)
        {
            this.fileName = fileName + ".cs";
            exeName = fileName + ".exe";
            Times = new int[Iterations];
            Inputs = new int[Iterations];
        }

        /// <summary>
        /// Builds and runs this algorithm
        /// </summary>
        /// <exception cref="CompilationException"> 
        /// Thrown when the algorithm fails to build or encounters a fatal error when running
        /// </exception>
        public void Run()
        {
            try
            {
                // Building the .cs
                using (Process build = new Process())
                {
                    build.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    build.StartInfo.FileName = "cmd.exe";
                    build.StartInfo.RedirectStandardOutput = true;
                    build.StartInfo.Arguments = $"/C SET \"PATH=%PATH%;C:\\Windows\\Microsoft.NET\\Framework\\v4.0.30319\" && cd \"{Directory.GetCurrentDirectory()}\" && csc \"{fileName}\"";

                    build.Start();
                    string outputOfBuild = build.StandardOutput.ReadToEnd();

                    if (outputOfBuild.Contains("error"))
                    {
                        throw new CompilationException(outputOfBuild.Substring(outputOfBuild.IndexOf(fileName)));
                    }
                    build.WaitForExit();
                    build.Close();
                }

                Stopwatch stopwatch = new Stopwatch();

                // Finding a suitable size of data
                bool isInTime = true;
                do
                {
                    difference = Math.Min(difference * 2, MaxDifference);

                    using (Process algorithm = new Process())
                    {
                        algorithm.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                        algorithm.StartInfo.FileName = "cmd.exe";
                        algorithm.StartInfo.RedirectStandardError = true;
                        algorithm.StartInfo.Arguments = $"/C \"{Path.GetFullPath(exeName)}\" {difference * Iterations}";

                        string error = "";

                        algorithm.Start();
                        isInTime = algorithm.WaitForExit(7500);
                        if (isInTime)
                        {
                            error = algorithm.StandardError.ReadToEnd();
                        }
                        else
                        {
                            // Kill all child processes to avoid memory leaks
                            algorithm.Kill(true);
                            difference /= 2;
                        }
                        algorithm.Close();

                        if (error != "")
                        {
                            difference /= 2;
                            isInTime = false;
                            if (error != "\nUnhandled Exception: OutOfMemoryException.\n")
                                throw new Exception(error);
                        }
                    }

                } while (isInTime && difference != MaxDifference);

                totalSize = (long)(0.5f * Iterations * (difference * (Iterations - 1)));

                HasCalibratedData = true;

                // Running the .exe
                for (int j = 0; j < Iterations; j++)
                {
                    using (Process algorithm = new Process())
                    {
                        algorithm.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                        algorithm.StartInfo.FileName = "cmd.exe";
                        algorithm.StartInfo.Arguments = $"/C \"{Path.GetFullPath(exeName)}\" {inputSize}";

                        // Priming CPU
                        for (int k = 0; k < 1000; k++)
                        {
                            Math.Exp(Math.PI);
                        }

                        stopwatch.Reset();

                        stopwatch.Start();

                        algorithm.Start();
                        algorithm.WaitForExit();
                        stopwatch.Stop();

                        algorithm.Close();

                        Times[j] = (int)stopwatch.ElapsedMilliseconds;
                        Inputs[j] = inputSize;
                        performedIterations += inputSize;
                        PercentComplete = (int)((float)performedIterations / totalSize * 100);
                        PercentComplete = Math.Min(PercentComplete, 100);
                        inputSize += difference;
                    }
                }

                IsComplete = true;
            }
            catch (Exception ex)
            {
                Abort = true;
                ExitException = ex;
            }
        }
    }
}