using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantRecipes.Library
{
    public class OptionGreeks
    {
        private StatUtility _util;
        public OptionGreeks()
        {
            _util = new StatUtility();
        }
        /// <summary>
        /// Calculates vega, the sensitivity to volatility.
        /// </summary>
        /// <param name="price">stock price</param>
        /// <param name="strike">strike price</param>
        /// <param name="rate">interest rate</param>
        /// <param name="dividend">dividend yield</param>
        /// <param name="volatility">volatility, i.e. standard deviation of stock price</param>
        /// <param name="timeToMaturity">time remaining to option maturity date</param>
        /// <returns>option vega</returns>
        public double CalculateVega(double price, double strike, double rate,
            double dividend, double volatility, double timeToMaturity)
        {
            double d1, vega, normalPrime;
            d1 = GetD1(price, strike, rate, dividend, volatility, timeToMaturity);
            normalPrime = _util.CalculateStandardNormalProbability(d1);
            vega = (normalPrime * Math.Exp(-dividend * timeToMaturity)) * price * Math.Sqrt(timeToMaturity);
            return vega;
        }
        /// <summary>
        /// Calculates delta, the sensitivity to the underlying stock price.
        /// </summary>
        /// <param name="price">stock price</param>
        /// <param name="strike">strike price</param>
        /// <param name="rate">interest rate</param>
        /// <param name="dividend">dividend yield</param>
        /// <param name="volatility">volatility, i.e. standard deviation of stock price</param>
        /// <param name="timeToMaturity">time remaining to option maturity date</param>
        /// <param name="type">call or put</param>
        /// <returns>option delta</returns>
        public double CalculateDelta(double price, double strike, double rate,
            double dividend, double volatility, double timeToMaturity, OptionType type)
        {
            double d1, delta;

            d1 = GetD1(price, strike, rate, dividend, volatility, timeToMaturity);

            if (type == OptionType.Call)
            {
                // SuspectTODO: conflict in the following formula. formula on wikipedia (http://en.wikipedia.org/wiki/Greeks_%28finance%29#Formulas_for_vanilla_option_Greeks) 
                //              uses CalculateStandardNormalCumulative and not CalculateStandardNormalProbability.
                //              Decided to go with the formula on wikipedia.
                delta = Math.Exp(-dividend * timeToMaturity) * _util.CalculateStandardNormalCumulative(d1);
            }
            else
            {
                delta = Math.Exp(-dividend * timeToMaturity) * (_util.CalculateStandardNormalCumulative(d1) - 1);
            }
            return delta;
        }
        /// <summary>
        /// Calculates gamma, the sensitivity to change in delta.
        /// </summary>
        /// <param name="price">stock price</param>
        /// <param name="strike">strike price</param>
        /// <param name="rate">interest rate</param>
        /// <param name="dividend">dividend yield</param>
        /// <param name="volatility">volatility, i.e. standard deviation of stock price</param>
        /// <param name="timeToMaturity">time remaining to option maturity date</param>
        /// <returns>option gamma</returns>
        public double CalculateGamma(double price, double strike, double rate,
            double dividend, double volatility, double timeToMaturity)
        {
            double d1, gamma, normalPrime;
            d1 = GetD1(price, strike, rate, dividend, volatility, timeToMaturity);
            normalPrime = _util.CalculateStandardNormalProbability(d1);
            gamma = (normalPrime * Math.Exp(-dividend * timeToMaturity)) /
                price * volatility * Math.Sqrt(timeToMaturity);
            return gamma;
        }
        /// <summary>
        /// Calculates rho, the sensitivity to change in risk-free interest rate.
        /// </summary>
        /// <param name="price">stock price</param>
        /// <param name="strike">strike price</param>
        /// <param name="rate">interest rate</param>
        /// <param name="dividend">dividend yield</param>
        /// <param name="volatility">volatility, i.e. standard deviation of stock price</param>
        /// <param name="timeToMaturity">time remaining to option maturity date</param>
        /// <param name="type">call or put</param>
        /// <returns>option rho</returns>
        public double CalculateRho(double price, double strike, double rate,
            double dividend, double volatility, double timeToMaturity, OptionType type)
        {
            double d1 = GetD1(price, strike, rate, dividend, volatility, timeToMaturity);
            double d2 = d1 - volatility * Math.Sqrt(timeToMaturity);
            double rho = 0.0;

            if (type == OptionType.Call)
            {
                rho = strike * timeToMaturity * Math.Exp(-rate * timeToMaturity) * _util.CalculateStandardNormalCumulative(d2);
            }
            else
            {
                rho = -strike * timeToMaturity * Math.Exp(-rate * timeToMaturity) * _util.CalculateStandardNormalCumulative(-d2);
            }
            return rho;
        }

        /// <summary>
        /// Calculates theta, the sensitivity to the time to maturity.
        /// </summary>
        /// <param name="price">stock price</param>
        /// <param name="strike">strike price</param>
        /// <param name="rate">interest rate</param>
        /// <param name="dividend">dividend yield</param>
        /// <param name="volatility">volatility, i.e. standard deviation of stock price</param>
        /// <param name="timeToMaturity">time remaining to option maturity date</param>
        /// <param name="type">call or put</param>
        /// <returns>option theta</returns>
        public double CalculateTheta(double price, double strike, double rate,
            double dividend, double volatility, double timeToMaturity, OptionType type)
        {
            double d1 = GetD1(price, strike, rate, dividend, volatility, timeToMaturity);
            double d2 = d1 - volatility * Math.Sqrt(timeToMaturity);
            double theta = 0.0;

            if (type == OptionType.Call)
            {
                theta = (-price * _util.CalculateStandardNormalCumulative(d1) * volatility * Math.Exp(-dividend * timeToMaturity)) /
                    (2 * Math.Sqrt(timeToMaturity)) + dividend * price * _util.CalculateStandardNormalCumulative(d1) * Math.Exp(-dividend * timeToMaturity) -
                    rate * strike * Math.Exp(-rate * timeToMaturity) * _util.CalculateStandardNormalCumulative(d2);
            }
            else
            {
                theta = (-price * _util.CalculateStandardNormalCumulative(d1) * volatility * Math.Exp(-dividend * timeToMaturity)) / (2 * Math.Sqrt(timeToMaturity)) -
                    dividend * price * _util.CalculateStandardNormalCumulative(-d1) * Math.Exp(-dividend * timeToMaturity) +
                    rate * strike * Math.Exp(-rate * timeToMaturity) * _util.CalculateStandardNormalCumulative(-d2);
            }
            return theta;
        }

        private double GetD1(double price, double strike, double rate, double dividend, double volatility, double timeToMaturity)
        {
            double d1;
            d1 = (Math.Log(price / strike) + (rate - dividend + (volatility * volatility / 2)) * timeToMaturity) /
                (volatility * Math.Sqrt(timeToMaturity));
            return d1;
        }
    }
}
