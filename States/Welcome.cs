using System;

namespace Project
{
    class Welcome : MenuState
    {

        public Welcome()
        {
            menu = new Menu(
                "Please select your choice:",
                new MenuOption("Enter an Algorithm", Continue),
                new MenuOption("About this program", Information),
                new MenuOption("Quit the program", Quit)
            );
        }

        public void Continue()
        {
            StateManager.ChangeState(new GetAlgorithm());
        }

        public void Quit()
        {
            StateManager.ChangeState(new Quit());
        }

        public void Information()
        {
            Console.Clear();
            Graphics.PrintLine("*** Time Complexity Estimator ***", ConsoleColor.DarkCyan);
            Console.WriteLine();
            Graphics.PrintLine("This program will estimate the time complexity of an algorithm in big O notation", ConsoleColor.White);
            Graphics.PrintLine("The user must be aware that due to Rice's theorom it is impossible to calculate the", ConsoleColor.White);
            Graphics.PrintLine("true time complexity of an algorithm but it can be estimated for relatively small", ConsoleColor.White);
            Graphics.PrintLine("programs.", ConsoleColor.White);
            Console.WriteLine();
            Graphics.PrintLine("To use this program, simply place your desired algorithm as a '.cs' in the bin", ConsoleColor.White);
            Graphics.PrintLine("folder of this program within the 'Programs' directory. Then select option 1 from the", ConsoleColor.White);
            Graphics.PrintLine("menu and input the name of the file. Ensure that there are no CPU intensive tasks", ConsoleColor.White);
            Graphics.PrintLine("running whilst the algorithm is running.", ConsoleColor.White);
            Console.WriteLine();
            Graphics.PrintLine("Press enter to continue", ConsoleColor.Red);
            Console.ReadKey();
        }
    }
}