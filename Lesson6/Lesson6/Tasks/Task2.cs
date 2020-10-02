using System;
using System.Reactive.Linq;

namespace Lesson6.Tasks
{
    public class Task2
    {
        /// <summary>
        /// Return a fallback if Observable errors
        /// 
        /// (original: Return a fallback if Flux errors)
        /// </summary>
        public static IObservable<string> FallbackOnError(IObservable<string> publisher, string fallback) =>
            publisher.OnErrorResumeNext(Observable.Return(fallback));
    }
}
