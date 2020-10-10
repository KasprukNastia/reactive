using System;
using System.Collections.Generic;
using System.Reactive.Subjects;

namespace Lesson7.Tasks
{
    public static class MyReactiveExtensions
    {
        public static IConnectableObservable<T> AutoConnect<T>(this IConnectableObservable<T> connectableObservable, int toConnectSubscribersCount)
        {
            return new AutoConnectableObservable<T>(connectableObservable, toConnectSubscribersCount);
        }
    }

    class AutoConnectableObservable<T> : IConnectableObservable<T>
    {
        private readonly IConnectableObservable<T> _initialObservable;
        private readonly int _toConnectSubscribersCount;
        private readonly List<IObserver<T>> _subscribers;

        public AutoConnectableObservable(
            IConnectableObservable<T> initialObservable,
            int toConnectSubscribersCount)
        {
            _initialObservable = initialObservable ?? throw new ArgumentNullException(nameof(initialObservable));
            _toConnectSubscribersCount = toConnectSubscribersCount;
            _subscribers = new List<IObserver<T>>();
        }

        public IDisposable Connect()
        {
            return _initialObservable.Connect();
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            _subscribers.Add(observer);

            if (_subscribers.Count == _toConnectSubscribersCount)
                _initialObservable.Connect();

            return _initialObservable.Subscribe(observer);
        }
    }
}
