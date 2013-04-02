using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantRecipes.Library
{
    public abstract class Option : Instrument
    {
        // OkashTODO: does _greeks have to be protected??
        protected OptionGreeks _greeks;
        protected double _strike;
        protected double _rate;
        protected double _timeToMaturity;
        protected double _price;
        protected double _volatility;
        protected double _dividend;
        protected OptionType _type;
        protected OptionExercise _exercise;
        protected PricingEngine _engine;
        protected StatUtility _statUtil;
        protected MatrixUtil _matrixUtil;

        public Option()
        {
            _price = 50.0;
            _strike = 50.0;
            _rate = 0.06;
            _dividend = 0.0;
            _timeToMaturity = 1;
            _type = OptionType.Call;
            _exercise = OptionExercise.European;
        }
        public Option(double price, double strike, double volatility, double rate, double dividend,
            double timeToMaturity, OptionType type, OptionExercise exercise)
        {
            _price = price;
            _strike = strike;
            _volatility = volatility;
            _rate = rate;
            _dividend = dividend;
            _timeToMaturity = timeToMaturity;
            _type = type;
            _exercise = exercise;
        }

        public Option(PricingEngine engine)
        {
            Debug.Assert(engine != null, "engine==null");

            _engine = engine;
        }

        /// <summary>
        /// initialises pricing engine.
        /// </summary>
        /// <param name="engine">the new pricing engine</param>
        public void SetPricingEngine(PricingEngine engine)
        {
            Debug.Assert(engine != null, "engine==null");

            _engine = engine;
            // following will trigger recalculation and notify observers
            Update();
            SetUpEngine();
        }
        /// <summary>
        /// sets up the newly set pricing engine.
        /// </summary>
        public abstract void SetUpEngine();

        /// <summary>
        /// Calculates and stores price of the security.
        /// </summary>
        protected override void PerformCalculations()
        {
            SetUpEngine();
            _engine.Calculate();
            OptionValue result = _engine.Result as OptionValue;
            Debug.Assert(result != null, "result == null");
            _NPV = result.ComputedValue;
        }
    }
}
