using System;

namespace Project
{

    class InvalidInputException : Exception
    {
        public InvalidInputException() : base("Invalid input")
        {

        }
    }

    class GraphLimitReachedException : Exception
    {
        public GraphLimitReachedException() : base("A maximum of 5 graphs can be drawn on one axis")
        {

        }
    }

    class CompilationException : Exception
    {
        public CompilationException(string message) : base("An error has occured, ensure your code compiles correctly. The error was:\n" + message)
        {

        }
    }

    class NotEnoughResultsException : Exception
    {
        public NotEnoughResultsException() : base("There are not enough results for this option")
        {

        }
    }
}
