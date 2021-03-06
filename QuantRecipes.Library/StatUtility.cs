﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantRecipes.Library
{
    // OkashTODO: Add methods to this class as needed.
    public class StatUtility
    {
        private const double a1 = 0.319381530;
        private const double a2 = -0.356563782;
        private const double a3 = 1.781477937;
        private const double a4 = -1.821255978;
        private const double a5 = 1.330274429;
        private const double gamma = 0.2316419;
        
        /// <summary>
        /// Calculates cumulative normal distribution probabilities.
        /// </summary>
        /// <param name="d">The critical value argument.</param>
        /// <returns>Probability</returns>
        public double CalculateStandardNormalCumulative(double d)
        {
            // OkashTODO: implement CalculateNormalCdf and then call that method here by passing in
            //              mean = 0 and standardDeviation = 1, just like CalculateStandardNormalPdf.
            //              Following implementation is based on Modeling Derivatives. For CalculateNormalCdf
            //              as discussed see Numerical Recipes.
            double k1 = 1 / (1 + gamma * d);
            double k2 = 1 / (1 - gamma * d);
            double normalPrime = CalculateStandardNormalProbability(d);
            double value = 0.0;
            // double h = 0.0;

            if (d >= 0)
            {
                value = 1 - normalPrime * (a1 * k1 + a2 * Math.Pow(k1, 2) + a3 * Math.Pow(k1, 3) +
                    a4 * Math.Pow(k1, 4) + a5 * Math.Pow(k1, 5));
            }
            else
            {
                value = normalPrime * (a1 * k2 + a2 * Math.Pow(k2, 2) + a3 * Math.Pow(k2, 3) +
                    a4 * Math.Pow(k2, 4) + a5 * Math.Pow(k2, 5));
            }
            return value;
        }
        /// <summary>
        /// Returns standard normal probability density function (pdf).
        /// </summary>
        /// <param name="d">Value for which the std pdf is required.</param>
        /// <returns>Standard normal pdf.</returns>
        public double CalculateStandardNormalProbability(double x)
        {
            // for standard normal, mean = 0 and standard deviation = 1
            double standardNormalPdf = CalculateNormalProbability(x, 0, 1);
            return standardNormalPdf;
        }
        /// <summary>
        /// Calculates normal probability density function for the given variable,
        /// the mean and the standard deviation.
        /// </summary>
        /// <param name="x">value for which normal pdf needs calculated.</param>
        /// <param name="mean">the mean, mu</param>
        /// <param name="standardDeviation">the standard deviation, sigma</param>
        /// <returns>Normal pdf corresponding to the arguments.</returns>
        public double CalculateNormalProbability(double x, double mean, double standardDeviation)
        {
            // formula based on numerical recipes.
            // normal pdf = (1/(sqrt(PI)*sigma))*exp(-0.5*sqr((x-mu)/sigma))
            double xMinusMuUponSigma = (x - mean) / standardDeviation;
            double normalPdf = (1 / (Math.Sqrt(2 * Math.PI) * standardDeviation)) * Math.Exp(-0.5 * xMinusMuUponSigma * xMinusMuUponSigma);
            return normalPdf;
        }
        /// <summary>
        /// Computes d1 of the Black-Scholes equation.
        /// </summary>
        /// <param name="price">stock price</param>
        /// <param name="strike">strike price</param>
        /// <param name="rate">interest rate</param>
        /// <param name="dividend">dividend yield</param>
        /// <param name="volatility">volatility, i.e. standard deviation of stock price</param>
        /// <param name="timeToMaturity">time remaining to option maturity date</param>
        /// <returns>D1</returns>
        public double GetD1(double price, double strike, double rate, double dividend, double volatility, double timeToMaturity)
        {
            double d1;
            d1 = (Math.Log(price / strike) + (rate - dividend + (volatility * volatility / 2)) * timeToMaturity) /
                (volatility * Math.Sqrt(timeToMaturity));
            return d1;
        }
        /// <summary>
        /// Computes d2 of the Black-Scholes equation.
        /// </summary>
        /// <param name="price">stock price</param>
        /// <param name="strike">strike price</param>
        /// <param name="rate">interest rate</param>
        /// <param name="dividend">dividend yield</param>
        /// <param name="volatility">volatility, i.e. standard deviation of stock price</param>
        /// <param name="timeToMaturity">time remaining to option maturity date</param>
        /// <returns>D2</returns>
        public double GetD2(double price, double strike, double rate, double dividend, double volatility, double timeToMaturity)
        {
            double d1 = GetD1(price, strike, rate, dividend, volatility, timeToMaturity);
            double d2 = GetD2(d1, volatility, timeToMaturity);
            return d2;
        }
        /// <summary>
        /// Computes d2 of the Black-Scholes equation.
        /// </summary>
        /// <param name="d1">D1</param>
        /// <param name="volatility">volatility, i.e. standard deviation of stock price</param>
        /// <param name="timeToMaturity">time remaining to option maturity date</param>
        /// <returns>d2</returns>
        public double GetD2(double d1, double volatility, double timeToMaturity)
        {
            double d2 = d1 - volatility * Math.Sqrt(timeToMaturity);
            return d2;
        }


        // OkashTODO: this is supposed to generate normal deviate... didnt get time to check, so see how to name it etc.
        public double gasdev(ref long idum)
        {
            throw new NotImplementedException();
        }
        
    }
}
