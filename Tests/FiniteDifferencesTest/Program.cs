using QuantRecipes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiniteDifferencesTest
{
    class Program
    {
        static void Main(string[] args)
        {
            FiniteDifferencesEngine engine = new FiniteDifferencesEngine();
            double[,] optionValues = engine.PriceEuropeanOption(0.2, 0.05, OptionType.Call, 100, 1, 20);
            Print2DArray(optionValues);
            Console.WriteLine("Press any key to exit...");
            Console.ReadLine();
        }

        private static void Print2DArray(double[,] arrayToPrint)
        {
            System.IO.StreamWriter file = new System.IO.StreamWriter("output.txt");
            
            int rowCount = arrayToPrint.GetLength(0);
            int columnCount = arrayToPrint.GetLength(1);

            for (int row = 0; row < rowCount; row++ )
            {
                // display every other element in the row
                for (int column = 0; column < columnCount; column++)
                {
                    file.Write(arrayToPrint[row,column].ToString("#.000") + "\t");
                }
                file.WriteLine();
            }
            file.Close();
        }
    }
}
