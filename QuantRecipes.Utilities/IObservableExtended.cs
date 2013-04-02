using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantRecipes.Utilities
{
    /// <summary>
    /// Inherits from IObservable and adds some more functionality.
    /// </summary>
    /// <typeparam name="T"> The object that provides notification information.This type parameter is
    ///     covariant. That is, you can use either the type you specified or any type
    ///     that is more derived. For more information about covariance and contravariance,
    ///     see Covariance and Contravariance in Generics.</typeparam>
    public interface IObservableExtended<out T> : IObservable<T>
    {
        void EndTransmission();
        void Update();
    }
}
