using System;
using System.IO;
using System.Threading;

namespace Project
{
    class GetAlgorithm : State
    {

        public GetAlgorithm()
        {

        }

        public override void Draw()
        {
            base.Draw();
            Graphics.PrintLine("Enter the filename of the algorithm or enter 'quit' to go back: ", ConsoleColor.Cyan);
        }

        /// <exception cref="FileNotFoundException"> 
        /// Thrown when the requested file is not found
        /// </exception>
        public override void Update()
        {
            base.Update();

            string fileName;

            try
            {
                fileName = Console.ReadLine();
                if (fileName.ToLower() == "quit")
                {
                    if (SavedResults.AreResultsSaved())
                    {
                        StateManager.ChangeState(new DisplayResults(SavedResults.GetResults(), true));
                    }
                    else
                    {
                        StateManager.ReturnToLastState();
                    }
                }
                else
                {
                    if (fileName.Contains('.'))
                    {
                        fileName = fileName.Substring(0, fileName.IndexOf("."));
                    }
                    if (File.Exists(fileName + ".cs"))
                    {
                        Algorithm algorithm = new Algorithm(fileName);
                        StateManager.ChangeState(new Running(algorithm));
                        Thread runAlgorithm = new Thread(() => algorithm.Run());
                        runAlgorithm.Priority = ThreadPriority.Highest;
                        runAlgorithm.Start();
                    }
                    else
                    {
                        throw new FileNotFoundException();
                    }
                }
            }
            catch (Exception ex)
            {
                InvalidInput(ex);
            }
        }

        private void InvalidInput(Exception ex)
        {
            Console.WriteLine();
            Graphics.PrintLine(ex.Message, ConsoleColor.Red);
            Console.ReadKey();
        }
    }
}