using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantRecipes.Library
{
    /// <summary>
    /// Describes Ornstein-Uhlenbeck process governed by dx = -a x(t) dt + sigma dz(t).
    /// </summary>
    public class OrnsteinUhlenbeckProcess:DiffusionProcess
    {
        private double _speed, _volatility;

        public OrnsteinUhlenbeckProcess(double speed, double volatility, double x0 = 0.0) :
            base(x0)
        {
            _speed = speed;
            _volatility = volatility;
        }

        // TuningTODO: don't need time argument. Make an overload? or default arguments??
        public override double GetDrift(double time, double x)
        {
            return -_speed * x;
        }

        // TuningTODO: takes arguments which are not required... May be make an overload?
        public override double GetDiffusion(double time, double x)
        {
            return _volatility;
        }
        // TuningTODO: no need for t0 argument
        public override double GetExpectation(double t0, double x0, double dt)
        {
            return x0 * Math.Exp(-_speed * dt);
        }
        // TuningTODO: no need for t0 and x0 arguments.
        public override double GetVariance(double t0, double x0, double dt)
        {
            return 0.5 * _volatility * _volatility / _speed * (1.0 - Math.Exp(-2.0 * _speed * dt));
        }
    }
}
