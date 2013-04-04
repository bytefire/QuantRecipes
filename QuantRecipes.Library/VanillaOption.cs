using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantRecipes.Library
{
    /// <summary>
    /// Represents an option with no discrete dividends, no barriers and on a single asset.
    /// </summary>
    /// <remarks>
    /// Note that this class doesn't implement the method Calculate as is done in the text. The
    /// reason is that the comments in the base class implementation, Instrument.Calculate() method
    /// advise against redefining this method in derived class.
    /// </remarks>
    public abstract class VanillaOption : Option
    {
        protected DateTime _exerciseDate;
        protected TermStructure _riskFreeRate; // spot rate term structure
        protected double _delta, _gamma, _theta, _vega, _rho, _dividendRho;

        public VanillaOption()
        {
        }
        public VanillaOption(double price, double strike, double rate, double dividend, double volatility,
            double timeToMaturity, OptionType type, OptionExercise exercise, PricingEngine engine)
        {
            throw new NotImplementedException();
        }

        public abstract double GetImpliedVolatility(double targetValue, double accuracy = 1.0e-4,
            int maxEvaluations = 100, double minVolatility = 1.0e-4, double maxVolatility = 4.0);

        // the Greeks
        public double GetDelta()
        {
            throw new NotImplementedException();
        }
        public double GetGamma()
        {
            throw new NotImplementedException();
        }
        public double GetTheta()
        {
            throw new NotImplementedException();
        }
        public double GetVega()
        {
            throw new NotImplementedException();
        }
        public double GetRho()
        {
            throw new NotImplementedException();
        }

        public override void SetUpEngine()
        {
            throw new NotImplementedException();
        }

        protected override void PerformCalculations()
        {
            throw new NotImplementedException();
        }
    }
}
