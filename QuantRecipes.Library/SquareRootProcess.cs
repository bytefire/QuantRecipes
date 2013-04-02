using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantRecipes.Library
{
    /// <summary>
    /// Describes a square-root process governed by: dx = a (b - x_t) dt +\sigma sqrt{x_t} dW_t
    /// </summary>
    public class SquareRootProcess: DiffusionProcess
    {
        private double _mean, _speed, _volatility;

        public SquareRootProcess(double b, double a, double sigma, double x0 = 0.0) :
            base(x0)
        {
            _mean = b;
            _speed = a;
            _volatility = sigma;
        }

        // Tuning TODO: time argument not required.
        public override double GetDrift(double time, double x)
        {
            return _speed * (_mean - x);
        }
        // TuningTODO: time argument not required.
        public override double GetDiffusion(double time, double x)
        {
            return _volatility * Math.Sqrt(x);
        }
    }
}
