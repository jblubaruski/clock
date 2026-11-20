using System;
using System.Collections.Generic;
using System.Text;

namespace Clock.Models.Notifications
{
    /// <summary>
    /// Pushes notifications out to a collection of observers that are subscribed to this observable.
    /// </summary>
    /// 
    /// <typeparam name="T">
    /// The data type sent out to subscribers.
    /// </typeparam>
    public sealed partial class NotificationProvider<T> : IObservable<T>
    {
        // the list of subscribed observers
        private readonly List<IObserver<T>> _observers = [];
        // the lock preventing the observer list from being accessed by multiple threads at the same time
        private readonly Lock _sync = new();

        // the disposable giving an observer the ability to unsubscribe from the provider
        private partial class Unsubscriber(List<IObserver<T>> observers, IObserver<T> observer, Lock sync) : IDisposable
        {
            public void Dispose()
            {
                lock (sync)
                {
                    observers.Remove(observer);
                }
            }
        }

        /// <summary>
        /// Subscribes an observer to this provider.
        /// </summary>
        /// 
        /// <param name="observer">
        /// The observer subscribing to this provider.
        /// </param>
        /// 
        /// <returns>
        /// An instance of IDisposable that provides the ability to unsubscribe from the provider.
        /// </returns>
        /// 
        /// <exception cref="ArgumentNullException">
        /// Thrown when observer is null.
        /// </exception>
        public IDisposable Subscribe(IObserver<T> observer)
        {
            ArgumentNullException.ThrowIfNull(observer);

            lock (_sync)
            {
                if (!_observers.Contains(observer))
                    _observers.Add(observer);
            }

            return new Unsubscriber(_observers, observer, _sync);
        }

        /// <summary>
        /// Sends a push notification containing data of type T out to all the subscribed observers.
        /// </summary>
        /// 
        /// <param name="data">
        /// The data to push to subscribers.
        /// </param>
        /// 
        /// <exception cref="ArgumentNullException">
        /// Thrown if data is null.
        /// </exception>
        public void NotifyAll(T data)
        {
            ArgumentNullException.ThrowIfNull(data);

            List<IObserver<T>> snapshot;

            lock (_sync)
            {
                snapshot = [.. _observers];
            }

            foreach (IObserver<T> subscriber in snapshot)
            {
                subscriber.OnNext(data);
            }
        }

        /// <summary>
        /// Sends a push notification to all subscribers notifying them that the provider has finished sending notifications.
        /// Unsubscribes all subscribers from this provider.
        /// </summary>
        public void CompleteAll()
        {
            List<IObserver<T>> snapshot;

            lock (_sync)
            {
                snapshot = [.. _observers];
                _observers.Clear();
            }

            foreach (IObserver<T> subscriber in snapshot)
            {
                subscriber.OnCompleted();
            }
        }

        /// <summary>
        /// Sends a push notification to all subscribers informing them that an error has occured.
        /// Unsubscribes all subscribers from this provider.
        /// </summary>
        /// 
        /// <param name="ex">
        /// The exception containing information about the error.
        /// </param>
        /// 
        /// <exception cref="ArgumentNullException">
        /// Thrown if ex is null.
        /// </exception>
        public void ErrorAll(Exception ex)
        {
            ArgumentNullException.ThrowIfNull(ex);

            List<IObserver<T>> snapshot;

            lock (_sync)
            {
                snapshot = [.. _observers];
                _observers.Clear();
            }

            foreach (IObserver<T> subscriber in snapshot)
            {
                subscriber.OnError(ex);
            }
        }
    }
}
