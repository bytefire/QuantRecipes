using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantRecipes
{
    public class FiniteDifferencesEngine
    {
        // Prices European calls and puts using Explicit Finite Difference method.
        public double[,] PriceEuropeanOption(double volatility, double interestRate, OptionType type,
            double strike, double timeToExpiration, int numberOfAssetSteps)
        {
            double[] assetPrices = new double[numberOfAssetSteps + 1];
            // maximum asset price divided by number of asset steps will yield asset price change per each step.
            double assetPriceStep = 2 * strike / numberOfAssetSteps;
            // time step must be less than 1/(volatility^2 * numberOfAssetSteps^2) for reasons of Stability
            double timeStep = 0.9 / ((volatility * volatility) * (numberOfAssetSteps * numberOfAssetSteps));
            int numberOfTimeSteps = ((int)(timeToExpiration / timeStep)) + 1;
            // following is to make sure that Expiration squarely falls an integer number of time steps later.
            timeStep = timeToExpiration / numberOfTimeSteps;

            double[,] optionValues = new double[numberOfAssetSteps + 1, numberOfTimeSteps + 1];

            // initialises asset prices array and the option values array for time = 0
            // OptTODO: Parallelisable??
            for (int i = 0; i <= numberOfAssetSteps; i++)
            {
                assetPrices[i] = i * assetPriceStep;
                // this is the only place where it call or put matters. everything else applies
                // to both calls and puts.
                optionValues[i, 0] = Math.Max((int)type * (assetPrices[i] - strike), 0);
            }

            // declare variables to be used for calculating option values
            double delta, gamma, theta;
            // the outer time loop. 
            for (int k = 1; k <= numberOfTimeSteps; k++)
            {
                // the asset loop covers only the inside of each array leaving end points outside the loop.
                // end points for the asset loop are treated separately using end point finite difference formulas.
                for (int i = 1; i < numberOfAssetSteps; i++) 
                {
                    // using central difference formula for delta (a finite difference formula)
                    delta = (optionValues[i + 1, k - 1] - optionValues[i - 1, k - 1]) / 2 / assetPriceStep;
                    // using central difference formula for gamma (a finite difference formula)
                    gamma = (optionValues[i + 1, k - 1] - 2 * optionValues[i, k - 1] + optionValues[i - 1, k - 1]) / assetPriceStep / assetPriceStep;
                    // theta using the Black-Scholes formula using the above calculated greeks
                    theta = -0.5 * volatility * volatility * assetPrices[i] * assetPrices[i] * gamma -
                        interestRate * assetPrices[i] * delta + interestRate * optionValues[i, k - 1];
                    // value of option = previous value of the option minus theta times time step.
                    // everything else equal, the option value drops as time to maturity decreases,
                    // irrespective of whether it's a call or a put.
                    optionValues[i, k] = optionValues[i, k - 1] - timeStep * theta;
                }
                // Boundary conditions for the asset array at time = k
                // boundary condition at asset step zero
                optionValues[0, k] = optionValues[0, k - 1] * (1 - interestRate * timeStep);
                // boundary condition at asset step infinity.
                optionValues[numberOfAssetSteps, k] = 2 * optionValues[numberOfAssetSteps - 1, k] - optionValues[numberOfAssetSteps - 2, k];
            }
            return optionValues;
        }
    }
}
