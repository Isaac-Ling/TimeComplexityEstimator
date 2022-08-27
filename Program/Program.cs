using System;
using System.IO;

namespace Project
{
    static class Program
    {
        static void Main()
        {

            CheckDependencies();

            SetUp();

            StateManager.ChangeState(new Welcome());

            do
            {
                StateManager.DrawCurrentState();
                StateManager.UpdateCurrentState();
            }
            while (!StateManager.ShouldQuit());
        }

        static void SetUp()
        {
            Console.Title = "Time Complexity Estimator";
            Console.WindowHeight = 40;
            Console.CursorVisible = false;
            Directory.SetCurrentDirectory("Programs");
        }

        static void CheckDependencies()
        {
            if (!Directory.Exists("C:\\Windows\\Microsoft.NET\\Framework"))
            {
                Graphics.PrintLine("It seems that Microsoft .NET Framework is not installed, please install and then restart the program", ConsoleColor.Red);
                Environment.Exit(-1);
            }
        }
    }
}