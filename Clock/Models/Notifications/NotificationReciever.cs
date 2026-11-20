using System;
using System.Collections.Generic;
using System.Text;

namespace Clock.Models.Notifications
{
    /// <summary>
    /// Recieves notifications from a provider.
    /// </summary>
    /// 
    /// <typeparam name="T">
    /// The type of the data to be recieved from the provider.
    /// </typeparam>
    public sealed class NotificationReciever<T> : IObserver<T>
    {
        private readonly Action<T> Next;
        private readonly Action Completed;
        private readonly Action<Exception> Error;
        private IDisposable? _unsubscriber = null;

        /// <summary>
        /// Constructs the observer with the passed in methods.
        /// </summary>
        /// 
        /// <param name="onNext">
        /// The method called when OnNext(T) is invoked.
        /// </param>
        /// <param name="onCompleted">
        /// The method called when OnCompleted() is invoked.
        /// </param>
        /// <param name="onError">
        /// The method called when OnError(Exception) is invoked.
        /// </param>
        /// 
        /// <exception cref="ArgumentNullException">
        /// Thrown if onNext, onCompleted, or onError is null.
        /// </exception>
        public NotificationReciever(Action<T> onNext, Action onCompleted, Action<Exception> onError)
        {
            ArgumentNullException.ThrowIfNull(onNext);
            ArgumentNullException.ThrowIfNull(onCompleted);
            ArgumentNullException.ThrowIfNull(onError);

            Next = onNext;
            Completed = onCompleted;
            Error = onError;
        }

        /// <summary>
        /// Subscribes this observer to the passed in notification provider.
        /// Does nothing if this observer is already subscribed to a provider.
        /// </summary>
        /// 
        /// <param name="provider">
        /// The provider this observer is subscribing to.
        /// </param>
        /// 
        /// <returns>
        /// True if the observer successfully subscribed to the passed in provider.
        /// </returns>
        /// 
        /// <exception cref="ArgumentNullException">
        /// Thrown if provider is null.
        /// </exception>
        public bool Subscribe(IObservable<T> provider)
        {
            ArgumentNullException.ThrowIfNull(provider);

            if (_unsubscriber == null)
            {
                _unsubscriber = provider.Subscribe(this);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Unsubscribes this observer from the provider it's subscribed to if it's subscribed to one.
        /// </summary>
        public void Unsubscribe()
        {
            _unsubscriber?.Dispose();
            _unsubscriber = null;
        }

        /// <summary>
        /// Calls the method that was passed into the onNext argument when this observer was constructed.
        /// </summary>
        /// 
        /// <param name="data">
        /// The data coming from the provider.
        /// </param>
        /// 
        /// <exception cref="ArgumentNullException">
        /// Thrown if data is null.
        /// </exception>
        public void OnNext(T data)
        {
            ArgumentNullException.ThrowIfNull(data);
            Next(data);
        }

        /// <summary>
        /// Calls the method that was passed into the onCompleted argument when this observer was constructed.
        /// Unsubscribes from the provider.
        /// </summary>
        public void OnCompleted()
        {
            Completed();
            Unsubscribe();
        }

        /// <summary>
        /// Calls the method that was passed into the onError argument when this observer was constructed.
        /// Unsubscribes from the provider.
        /// </summary>
        /// 
        /// <param name="ex">
        /// The exception coming from the provider.
        /// </param>
        /// 
        /// <exception cref="ArgumentNullException">
        /// Thrown if ex is null.
        /// </exception>
        public void OnError(Exception ex)
        {
            ArgumentNullException.ThrowIfNull(ex);
            Error(ex);
            Unsubscribe();
        }
    }
}
