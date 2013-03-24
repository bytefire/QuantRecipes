using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantRecipes
{
    public class Utilities
    {
        // NOTE: Random must be instantiated on class-level so that it is initialised just once.
        //       Otherwise it will be initialised everytime GetBoxMullerRandomNumber method is called,
        //       the same sequence of random numbers will be generated. It's the same reason that this 
        //       instance is static too.
        private static Random rng = new Random();
        // Generates standard normal random number by applying Box-Muller transform on a uniform random number.
        public double GetBoxMullerRandomNumber()
        {
            // NOTE: Aparently there's a better way to generate uniform random numbers: Normal Deviates by Ratio-of-Uniforms
            // (section 7.3.9 in Numerical Recipes)
            double x, y, randomSumOfSquares;
            do
            {
                x = 2 * rng.NextDouble() - 1;
                y = 2 * rng.NextDouble() - 1;
                randomSumOfSquares = x * x + y * y;
            } while (randomSumOfSquares >= 1);
            double boxMuller = x * Math.Sqrt(-2 * Math.Log(randomSumOfSquares) / randomSumOfSquares);
            return boxMuller;
        }
    }
}
