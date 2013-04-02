using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantRecipes.Library
{
    /// <summary>
    /// Diffusion process base class. This class describes a stochastic process
    /// governed by dx(t) = mu(t, x(t))dt + sigma(t, x(t))dz(t).
    /// </summary>
    public abstract class DiffusionProcess
    {
        private double _x0;

        public DiffusionProcess(double x0)
        {
            _x0 = x0;
        }
        /// <summary>
        /// Gets x zero.
        /// </summary>
        public double X0
        {
            get
            {
                return _x0;
            }
        }
        /// <summary>
        /// Returns the drift part of the equation, i.e. mu(t, x_t).
        /// </summary>
        /// <param name="time">The time.</param>
        /// <param name="x">Value of random variable at time t.</param>
        /// <returns>mu(t, x_t)</returns>
        public abstract double GetDrift(double time, double x);
        /// <summary>
        /// Returns the diffusion part of the equation, i.e.sigma(t, x_t).
        /// </summary>
        /// <param name="time">The time.</param>
        /// <param name="x">Value of random variable at time t.</param>
        /// <returns></returns>
        public abstract double GetDiffusion(double time, double x);
        /// <summary>
        /// Returns the expectation of the process after a time interval, i.e
        /// E(x_{t_0 + delta t} | x_{t_0} = x_0) since it is a Markov. By default it 
        /// returns Euler approximation defined by x_0 + mu(t_0, x_0) delta t.
        /// </summary>
        /// <param name="t0">Current time.</param>
        /// <param name="x0">Current value of random variable x.</param>
        /// <param name="dt">Time step.</param>
        /// <returns></returns>
        public virtual double GetExpectation(double t0, double x0, double dt)
        {
            return x0 + GetDrift(t0, x0) * dt;
        }
        /// <summary>
        /// Returns the variance of the process after a time interval, i.e.
        /// Var(x_{t_0 + \Delta t} | x_{t_0} = x_0). By default, it returns
        /// the Euler approximation defined by \sigma(t_0, x_0)^2 \Delta t .
        /// </summary>
        /// <param name="t0"></param>
        /// <param name="x0"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public virtual double GetVariance(double t0, double x0, double dt)
        {
            double sigma = GetDiffusion(t0, x0);
            return sigma * sigma * dt;
        }
    }
}
