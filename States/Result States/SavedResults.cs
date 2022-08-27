using System.Collections.Generic;

namespace Project
{
    static class SavedResults
    {

        private static List<Result> results;

        public static int XAxisLength { get; set; } = Graph.DefaultXAxisLength;
        public static int YAxisLength { get; set; } = Graph.DefaultYAxisLength;

        public static void SaveResults(List<Result> resultsToSave)
        {
            results = resultsToSave;
        }

        public static void SaveGraphAxisLengths(int xAxisLength, int yAxisLength)
        {
            XAxisLength = xAxisLength;
            YAxisLength = yAxisLength;
        }

        public static void RemoveResults()
        {
            results = null;
            XAxisLength = Graph.DefaultXAxisLength;
            YAxisLength = Graph.DefaultYAxisLength;
        }

        public static List<Result> GetResults()
        {
            return results;
        }

        public static void AddResult(Result result)
        {
            results.Add(result);
        }

        public static bool AreResultsSaved()
        {
            return !(results == null);
        }
    }
}