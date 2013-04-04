using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantRecipes.Library
{
    /// <summary>
    /// Black-Scholes-Merton option.
    /// </summary>
    public class BlackScholesOption:VanillaOption
    {
        // should this go in the Instrument base class as a protected member??
        private StatUtility _util = new StatUtility();

        public BlackScholesOption(OptionType type, double price, double strike, double dividend, double rate,
            double timeToMaturity, double volatility)
        {
            _type = type;
            _price = price;
            _strike = strike;
            _dividend = dividend;
            _rate = rate;
            _timeToMaturity = timeToMaturity;
            _volatility = volatility;
        }

        public override double GetImpliedVolatility(double targetValue, double accuracy = 1.0e-4, int maxEvaluations = 100, double minVolatility = 1.0e-4, double maxVolatility = 4.0)
        {
            throw new NotImplementedException();
        }

        public virtual void SetVolatility(double newVolatility)
        {
            _volatility = newVolatility;
        }

        public virtual void SetRate(double newRate)
        {
            _rate = newRate;
        }

        public virtual void SetDividend(double newDividend)
        {
            _dividend = newDividend;
        }

        public double CalculateBSCallPrice(double price, double strike, double volatility, double rate, double dividend,
            double timeToMaturity)
        {
            double d1 = _util.GetD1(price, strike, rate, dividend, volatility, timeToMaturity);
            double d2 = _util.GetD2(d1, volatility, timeToMaturity);

            double prob1 = _util.CalculateStandardNormalCumulative(d1);
            double prob2 = _util.CalculateStandardNormalCumulative(d2);
            // same formula on wiki: http://en.wikipedia.org/wiki/Black%E2%80%93Scholes#Extensions_of_the_model
            // (see under 'instruments with continuous dividends')
            // following formula is from text.
            double callPrice = 
                price * Math.Exp(-dividend * timeToMaturity) * prob1 - strike * Math.Exp(-rate * timeToMaturity) * prob2;

            return callPrice;
        }

        public double CalculateBSPutPrice(double price, double strike, double volatility, double rate, double dividend,
            double timeToMaturity)
        {
            double d1 = _util.GetD1(price, strike, rate, dividend, volatility, timeToMaturity);
            double d2 = _util.GetD2(d1, volatility, timeToMaturity);

            double prob1 = _util.CalculateStandardNormalCumulative(-d1);
            double prob2 = _util.CalculateStandardNormalCumulative(-d2);
            // same formula on wiki: http://en.wikipedia.org/wiki/Black%E2%80%93Scholes#Extensions_of_the_model
            // (see under 'instruments with continuous dividends')
            // following formula is from text.
            double putPrice = 
                strike * Math.Exp(-rate * timeToMaturity) * prob2 - price * Math.Exp(-dividend * timeToMaturity) * prob1;
            return putPrice;
        }
    }
}
