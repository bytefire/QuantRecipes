using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantRecipes
{
    // uses Monte Carlo simulations to price options.
    public class MonteCarloEngine
    {
        // Prices European option using Monte Carlo simulation
        public double PriceEuropeanOption(double startingAssetValue, double interestRate, OptionType optionType, double strike,
            double volatility, double timeStep, int numberOfTimeSteps)
        {
            int numberOfSimulations = 250;
            object syncLock = new object();
            double sumOfFutureValuesOfOption = 0.0;
            Parallel.For(0, numberOfSimulations, simulationNumber =>
                {
                    double currentAssetValue = startingAssetValue, futureValueOfOption = 0.0;
                    // each simulation
                    for (int i = 0; i < numberOfTimeSteps; i++)
                    {
                        currentAssetValue = SimulateNextAssetValue(currentAssetValue, interestRate, timeStep, volatility);
                    }
                    futureValueOfOption = Math.Max((int)optionType * (currentAssetValue - strike), 0);
                    // lock to ensure that each addition is atomic.
                    // TODO: this is a bottleneck as locking not only
                    //      makes this loop linear but slows it down even more due to locking overhead.
                    //      *Potential Race Condition too!*
                    lock (syncLock)
                    {
                        sumOfFutureValuesOfOption += futureValueOfOption;
                    }
                });
            double averageFutureValueOfOption = sumOfFutureValuesOfOption / numberOfSimulations;
            // compute present value of the average future value. here timeStep*numberOfTimeSteps gives total time to expiry
            double optionValue = Math.Exp(-interestRate * timeStep * numberOfTimeSteps) * averageFutureValueOfOption;
            return optionValue;
        }

        // Prices European option using Monte Carlo simulation. This uses linear for loop instead of Parallel
        // NOTE: Linear is faster, probably because of the locking overhead in parallel version.
        public double PriceEuropeanOptionLinear(double startingAssetValue, double interestRate, OptionType optionType, double strike,
            double volatility, double timeStep, int numberOfTimeSteps)
        {
            int numberOfSimulations = 50;
            object syncLock = new object();
            double sumOfFutureValuesOfOption = 0.0;
            for(int simulationNumber = 0;simulationNumber<numberOfSimulations; simulationNumber++)
            {
                double currentAssetValue = startingAssetValue, futureValueOfOption = 0.0;
                // each simulation
                for (int i = 0; i < numberOfTimeSteps; i++)
                {
                    currentAssetValue = SimulateNextAssetValue(currentAssetValue, interestRate, timeStep, volatility);
                }
                futureValueOfOption = Math.Max((int)optionType * (currentAssetValue - strike), 0);

                sumOfFutureValuesOfOption += futureValueOfOption;
            }
            double averageFutureValueOfOption = sumOfFutureValuesOfOption / numberOfSimulations;
            // compute present value of the average future value. here timeStep*numberOfTimeSteps gives total time to expiry
            double optionValue = Math.Exp(-interestRate * timeStep * numberOfTimeSteps) * averageFutureValueOfOption;
            return optionValue;
        }

        // uses the risk-neutral random walk to generate next asset value for the given parameters.
        // this overload uses Box-Muller method to generate standard Normal random number.
        private double SimulateNextAssetValue(double currentAssetValue, double interestRate, double timeStep, double volatility)
        {
            return SimulateNextAssetValue(currentAssetValue, interestRate, timeStep, volatility, (new Utilities()).GetBoxMullerRandomNumber);
        }
        // uses the risk-neutral random walk to generate next asset value for the given parameters.
        private double SimulateNextAssetValue(double currentAssetValue, double interestRate, double timeStep,
            double volatility, Func<double> getStandardNormalRandomNumber)
        {
            double phi = getStandardNormalRandomNumber();
            double assetValueStep = interestRate * currentAssetValue * timeStep +
                volatility * currentAssetValue * Math.Sqrt(timeStep) * phi;
            double nextAssetValue = currentAssetValue + assetValueStep;
            return nextAssetValue;
        }
    }
}
