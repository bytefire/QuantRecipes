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

        // Prices American calls and puts using Explicit Finite Difference method.
        // NOTE that if isEarlyExercise is set to false, this method will produce output same as 
        // European option.
        public double[,] PriceAmericanOption(double volatility, double interestRate, OptionType optionType,
            double strike, double timeToExpiration, bool isEarlyExercise, int numberOfAssetSteps)
        {
            double[] assetPrices = new double[numberOfAssetSteps + 1];
            double[] payoffs = new double[numberOfAssetSteps + 1];

            double assetStep = 2 * strike / numberOfAssetSteps;
            // for stability...
            double timeStep = 0.9 / (volatility * volatility) / (numberOfAssetSteps * numberOfAssetSteps);
            int numberOfTimeSteps = (int)(timeToExpiration / timeStep) + 1;
            // to make sure that expiration falls a whole-number of steps away..
            timeStep = timeToExpiration / numberOfTimeSteps;

            double[,] optionValues = new double[numberOfAssetSteps + 1, numberOfTimeSteps + 1];

            for (int i = 0; i <= numberOfAssetSteps; i++)
            {
                assetPrices[i] = i * assetStep;
                optionValues[i, 0] = Math.Max((int)optionType * (assetPrices[i] - strike), 0);
                payoffs[i] = optionValues[i, 0];
            }

            double delta, gamma, theta;
            for (int k = 1; k <= numberOfTimeSteps; k++)
            {
                for (int i = 1; i < numberOfAssetSteps; i++)
                {
                    // delta using central difference.
                    delta = (optionValues[i + 1, k - 1] - optionValues[i - 1, k - 1]) / 2 / assetStep;
                    // gamma using central difference.
                    gamma = (optionValues[i + 1, k - 1] - 2 * optionValues[i, k - 1] + optionValues[i - 1, k - 1]) / assetStep / assetStep;
                    // theta using Black-Scholes formula
                    theta = -0.5 * volatility * volatility * assetPrices[i] * assetPrices[i] * gamma -
                        interestRate * assetPrices[i] * delta + interestRate * optionValues[i, k - 1];
                    optionValues[i, k] = optionValues[i, k - 1] - timeStep * theta;
                }
                // boundary condition at asset price = 0.
                optionValues[0, k] = optionValues[0, k - 1] * (1 - interestRate * timeStep);
                // boundary condition at asset price = infinity.
                optionValues[numberOfAssetSteps, k] = 2 * optionValues[numberOfAssetSteps - 1, k] - optionValues[numberOfAssetSteps - 2, k];

                if (isEarlyExercise)
                {
                    for (int i = 0; i <= numberOfAssetSteps; i++)
                    {
                        optionValues[i, k] = Math.Max(optionValues[i, k], payoffs[i]);
                    }
                }
            }
            return optionValues;
        }

        // Similar to the method above which prices American options except that this doesn't output the whole 
        // option values matrix. It just returns values for the curve today. Plus it returns the greeks as well.
        // Returns a 2d array with following columns for today's date: Asset Price, Payoff, Option Values, Delta, Gamma, Theta
        // NOTE: same as its 3d equivalent, this method prices a European option if isEarlyExercise is set to false.
        public double[,] PriceAmericanOption2d(double volatility, double interestRate, OptionType optionType,
            double strike, double timeToExpiration, bool isEarlyExercise, int numberOfAssetSteps)
        {
            double[] assetPrices = new double[numberOfAssetSteps + 1];
            double[] optionValuesOld = new double[numberOfAssetSteps + 1];
            double[] optionValuesNew = new double[numberOfAssetSteps + 1];
            double[] payoffs = new double[numberOfAssetSteps + 1];
            double[,] returnMatrix = new double[numberOfAssetSteps + 1, 6];

            double assetStep = 2 * strike / numberOfAssetSteps;
            // for algorithmic stability...
            double timeStep = 0.9 / (volatility * volatility) / (numberOfAssetSteps * numberOfAssetSteps);
            int numberOfTimeSteps = (int)(timeToExpiration / timeStep) + 1;
            timeStep = timeToExpiration / numberOfTimeSteps;

            for (int i = 0; i <= numberOfAssetSteps; i++)
            {
                assetPrices[i] = i * assetStep;
                optionValuesOld[i] = Math.Max((int)optionType * (assetPrices[i] - strike), 0);
                payoffs[i] = optionValuesOld[i];
                // first column to store asset prices.
                returnMatrix[i, 0] = assetPrices[i];
                // second column for payoffs.
                returnMatrix[i, 1] = payoffs[i];
            }

            double delta, gamma, theta;
            for (int k = 1; k <= numberOfTimeSteps; k++)
            {
                for (int i = 1; i < numberOfAssetSteps; i++)
                {
                    // delta using central difference.
                    delta = (optionValuesOld[i + 1] - optionValuesOld[i - 1]) / 2 / assetStep;
                    // gamma using central difference
                    gamma = (optionValuesOld[i + 1] - 2 * optionValuesOld[i] + optionValuesOld[i - 1]) / assetStep / assetStep;
                    // theta using the Black-Scholes equation
                    theta = -0.5 * volatility * volatility * assetPrices[i] * assetPrices[i] * gamma -
                        interestRate * assetPrices[i] * delta + interestRate * optionValuesOld[i];
                    optionValuesNew[i] = optionValuesOld[i] - timeStep * theta;
                }
                // boundary condition when asset price = 0
                optionValuesNew[0] = optionValuesOld[0] * (1 - interestRate * timeStep);
                // boundary condition when asset price = infinity
                optionValuesNew[numberOfAssetSteps] = 2 * optionValuesNew[numberOfAssetSteps - 1] - optionValuesNew[numberOfAssetSteps - 2];
                // old values become new values...
                optionValuesNew.CopyTo(optionValuesOld, 0);

                if (isEarlyExercise)
                {
                    for (int i = 0; i <= numberOfAssetSteps; i++)
                    {
                        optionValuesOld[i] = Math.Max(optionValuesOld[i], payoffs[i]);
                    }
                }
            }

            for (int i = 1; i < numberOfAssetSteps; i++)
            {
                // column 3 for option values
                returnMatrix[i, 2] = optionValuesOld[i];
                // column 4 for delta - using central difference
                returnMatrix[i, 3] = (optionValuesOld[i + 1] - optionValuesOld[i - 1]) / 2 / assetStep;
                // column 5 for gamma - using central difference
                returnMatrix[i, 4] = (optionValuesOld[i + 1] - 2 * optionValuesOld[i] + optionValuesOld[i - 1]) / assetStep / assetStep;
                // column 6 for theta - using Black-Scholes
                returnMatrix[i, 5] = -0.5 * volatility * volatility * assetPrices[i] * assetPrices[i] * returnMatrix[i, 4] -
                    interestRate * assetPrices[i] * returnMatrix[i, 3] + interestRate * optionValuesOld[i];
            }

            returnMatrix[0, 2] = optionValuesOld[0];
            returnMatrix[numberOfAssetSteps, 3] = optionValuesOld[numberOfAssetSteps];
            // separate treatment of endpoints for delta, gamma and theta
            returnMatrix[0, 3] = (optionValuesOld[1] - optionValuesOld[0]) / assetStep;
            returnMatrix[numberOfAssetSteps, 3] = (optionValuesOld[numberOfAssetSteps] - optionValuesOld[numberOfAssetSteps - 1]) / assetStep;
            returnMatrix[0, 4] = 0;
            returnMatrix[numberOfAssetSteps, 4] = 0;
            returnMatrix[0, 5] = interestRate * optionValuesOld[0];
            returnMatrix[numberOfAssetSteps, 5] = -0.5 * volatility * volatility * assetPrices[numberOfAssetSteps] * assetPrices[numberOfAssetSteps] * returnMatrix[numberOfAssetSteps, 4] -
                interestRate * assetPrices[numberOfAssetSteps] * returnMatrix[numberOfAssetSteps, 3] + interestRate * optionValuesOld[numberOfAssetSteps];

            return returnMatrix;
        }
    }
}
