using QuantRecipes.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantRecipes.Library
{
    /// <summary>
    /// An abstract class that defines the interface of concrete instruments which will
    /// be derived from this one. It implements IObservable interface.
    /// </summary>
    public abstract class Instrument : IObservableExtended<Instrument>, IObserver<Instrument>
    {
        #region IObservableExtended Implementation
        // see http://msdn.microsoft.com/en-us/library/dd990377.aspx 
        private List<IObserver<Instrument>> _observers;
        
        public IDisposable Subscribe(IObserver<Instrument> observer)
        {
            if (!_observers.Contains(observer))
                _observers.Add(observer);
            return new Unsubscriber<Instrument>(_observers, observer);
        }

        public void EndTransmission()
        {
            foreach (var observer in _observers.ToArray())
                if (_observers.Contains(observer))
                    observer.OnCompleted();

            _observers.Clear();
        }

        protected void NotifyObservers()
        {
            foreach (var observer in _observers)
            {
                observer.OnNext(this);
            }
        }
        #endregion

        #region IObserver Implementation
        public virtual void OnCompleted() { }
        public virtual void OnError(Exception error) { }
        public virtual void OnNext(Instrument value) { }
        #endregion
        // tracks whether instrument's value has been calculated
        private bool _calculated = false;
        // _NPV and _isExpired will be set inside performCalculations method.
        protected double _NPV;
        protected bool _isExpired;

        public Instrument()
        {
            _observers = new List<IObserver<Instrument>>();
            _NPV = 0.0;
            _isExpired = false;
            _calculated = false;
        }

        public Instrument(string isinCode, string description)
            : this()
        {
            IsinCode = isinCode;
            Description = description;
        }
        /// <summary>
        /// Returns ISIN code of the instrument when available.
        /// </summary>
        public string IsinCode
        {
            get;
            private set;
        }
        /// <summary>
        /// Gets a brief textual description of the instrument.
        /// </summary>
        public string Description
        {
            get;
            private set;
        }
        /// <summary>
        /// Returns the net present value of the instrument.
        /// </summary>
        /// <returns></returns>
        public double GetNPV()
        {
            Calculate();
            return (_isExpired ? 0.0 : _NPV);
        }
        /// <summary>
        /// Returns whether the instrument is still tradable.
        /// </summary>
        /// <returns></returns>
        public bool GetIsExpired()
        {
            Calculate();
            return _isExpired;
        }
        /// <summary>
        /// Updates dependent instrument classes.
        /// </summary>
        public void Update()
        {
            _calculated = false;
            NotifyObservers();
        }
        /// <summary>
        /// This method forces recaclulation of the instrument value and other results
        /// which would otherwise be cached. (?)Explicit invocation of this method is not 
        /// necessary if the instrument is registered itself as observer with the structures
        /// on which such results depend.(?)
        /// </summary>
        public void Recalculate()
        {
            PerformCalculations();
            _calculated = true;
            NotifyObservers();
        }
        /// <summary>
        /// This method performs all needed calculations by calling the PerformCalculations method.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Instruments cache results of the previous calculations. Such results will be returned upon 
        /// later invocations of Calculate method. The results depend upon arguments such as terms
        /// structures which could change betwen invocations; the instruments must register itself
        /// as observer of such objects for the calculations to be performed again when they change.
        /// 
        /// This method should not be redefined in derived classes.
        /// </remarks>
        public double Calculate()
        {
            if (!_calculated)
            {
                PerformCalculations();
            }
            _calculated = true;
            return 0.0;
        }

        /// <summary>
        /// This method must implement any calculations which must be redone
        /// in order to calculat NPV of the instrument.
        /// </summary>
        protected abstract void PerformCalculations();
    }
}
