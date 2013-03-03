using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantRecipes
{
    /// <summary>
    /// The binomial option procing engine.
    /// </summary>
    public class BinomialEngine
    {
        // using double instead of decimal for because of .NET math libraries. see http://stackoverflow.com/questions/803225/when-should-i-use-double-instead-of-decimal
        // for detailed discussion which recommends decimal. any input regarding double vs decimal 
        // in financial calculations will be welcome. e.g. what is the speed or performance trade-off?

        /// <summary>
        /// Prices European call option using binomial model.
        /// </summary>
        /// <param name="assetPrice">Current price of the underlying.</param>
        /// <param name="volatility">Standard devialtion of the underlying asset's price.</param>
        /// <param name="interestRate">The continuous discount rate to compute present values.</param>
        /// <param name="strikePrice">Strike price of the call option.</param>
        /// <param name="timeToExpiry">Time to expiry. Units are not specified so the interpretation of 
        /// units is upto the caller.</param>
        /// <param name="numberOfSteps">Total number of discrete steps. This corresponds to the number of 
        /// hierarchical levels in the binomial tree.</param>
        /// <returns>Price of the European call option for the given input.</returns>
        public double PriceEuropeanCallOption(double assetPrice, double volatility, double interestRate,
            double strikePrice, double timeToExpiry, int numberOfSteps)
        {
            // these arrays represent asset prices and option values at a particular level of the binomial tree.
            // number of nodes for a hierarchical level = number of that level + 1
            double[] assetPricesTree = new double[numberOfSteps + 1];
            double[] optionValuesTree = new double[numberOfSteps + 1];
            double timePerStep = timeToExpiry / numberOfSteps;
            double discountFactor = Math.Exp(-interestRate * timePerStep);

            double temp1 = Math.Exp((interestRate + volatility * volatility) * timePerStep);
            double temp2 = 0.5 * (discountFactor + temp1);

            double u = temp2 + Math.Sqrt(temp2 * temp2 - 1);
            double d = 1 / u;
            double p = (Math.Exp(interestRate * timePerStep) - d) / (u - d);

            assetPricesTree[0] = assetPrice;

            // at the end of following nested loops we get all possible nodes of asset pricing binomial tree at time T.
            // for example for number of steps = 3, we get: d*d*d*assetPrice, d*d*u*assetPrice, d*u*u*assetPrice, u*u*u*assetPrice.
            for (int i = 1; i <= numberOfSteps; i++)
            {
                for (int j = i; j >= 1; j--)
                {
                    assetPricesTree[j] = u * assetPricesTree[j - 1];
                }
                assetPricesTree[0] = d * assetPricesTree[0];
            }

            // this loop computs option values at the nodes at time T
            for (int i = 0; i <= numberOfSteps; i++)
            {
                optionValuesTree[i] = GetCallOptionPayoff(strikePrice, assetPricesTree[i]);
            }

            // following nested loops comput probability weighted and discounted option values at each step all the way
            // to the root node.
            for (int i = numberOfSteps; i >= 1; i--)
            {
                for (int j = 0; j <= i - 1; j++)
                {
                    optionValuesTree[j] = (p * optionValuesTree[j + 1] + (1 - p) * optionValuesTree[j]) * discountFactor;
                }
            }
            return optionValuesTree[0];
        }

        private double GetCallOptionPayoff(double strike, double assetPrice)
        {
            return Math.Max(assetPrice - strike, 0.0);
        }
    }
}
