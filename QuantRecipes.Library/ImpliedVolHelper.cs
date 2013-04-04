using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantRecipes.Library
{
    /// <summary>
    /// Helper class for implied volatility.
    /// </summary>
    public class ImpliedVolHelper : ObjectiveFunction
    {
        private PricingEngine _engine;
        private double _targetValue;
        private OptionValue _results;
        private StatUtility _util = new StatUtility();

        public ImpliedVolHelper(PricingEngine engine, double targetValue)
        {
            _engine = engine;
            _targetValue = targetValue;
        }

        public Dictionary<int, double> CalculateImpliedVolatilities(double price, List<double> optionPrices,
            List<int> strikes, double rate, double dividend, double timeToMaturity, OptionType type)
        {
            throw new NotImplementedException();
        }

        public Dictionary<KeyValuePair<double, int>, double> CalculateImpliedSurface(double price,
            List<double> optionPrices, List<int> strikes, List<double> timeToMaturities, Dictionary<double, double> rates,
            double dividend, OptionType type)
        {
            throw new NotImplementedException();
        }
        public double this[double x]
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
