using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantRecipes.Library
{
    // OkashTODO: add implementation as you go
    public abstract class PricingEngine
    {
        protected PricingEngineResult _result;
        public PricingEngineResult Result
        {
            get
            {
                return _result;
            }
        }
        public abstract void Calculate();
    }
}
