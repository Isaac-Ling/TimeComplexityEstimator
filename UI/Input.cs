using System;

namespace Project
{

    static class Input
    {

        /// <summary>
        /// Will take user input and attempt to cast it to T
        /// </summary>
        /// <returns>
        /// The inputted data or the default value of T if the cast was invalid
        /// </returns>
        public static T GetInput<T>() where T : struct
        {
            T result;
            try
            {
                result = (T)Convert.ChangeType(Console.ReadLine(), typeof(T));
            }
            catch (Exception)
            {
                Console.WriteLine();
                Graphics.PrintLine(new InvalidInputException().Message, ConsoleColor.Red);
                Console.ReadKey();
                result = default;
            }

            return result;
        }

        /// <summary>
        /// Will take a user key press and attempt to cast it to an int
        /// </summary>
        /// <returns>
        /// The int or -1 if it was unsuccessful
        /// </returns>
        public static int KeyToInt(int min = 0, int max = int.MaxValue)
        {
            int result;

            try
            {
                result = int.Parse(Console.ReadKey().KeyChar.ToString());
                if (result < min || result > max)
                {
                    throw new Exception();
                }
            }
            catch (Exception)
            {
                Console.WriteLine();
                Graphics.PrintLine(new InvalidInputException().Message, ConsoleColor.Red);
                Console.ReadKey();
                result = -1;
            }

            return result;
        }
    }
}