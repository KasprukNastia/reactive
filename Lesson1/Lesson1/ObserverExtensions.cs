using System;

namespace Lesson1
{
    public static class ObserverExtensions
    {
        public static IDisposable SubscribeConsole<T>(
            this IObservable<T> observable,
            string name = "")
        {
            return observable.Subscribe(new ConsoleObserver<T>(name));
        }
    }
}
