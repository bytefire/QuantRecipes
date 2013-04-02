using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantRecipes.Library
{
    /// <summary>
    /// Describes the stochastic process governed by dS = (r - 0.5{sigma^2}) dt + sigma dz(t).
    /// </summary>
    public class BlackScholesProcess : DiffusionProcess
    {
        double _r, _sigma;

        public BlackScholesProcess(double rate, double volatility, double s0 = 0.0) :
            base(s0)
        {
            _r = rate;
            _sigma = volatility;
        }
        // TuningTODO: both arguments not required
        public override double GetDrift(double t, double x)
        {
            return _r - 0.5 * _sigma * _sigma;
        }
        // TuningTODO: both arguments not reuired.
        public override double GetDiffusion(double t, double x)
        {
            return _sigma;
        }
    }
}
