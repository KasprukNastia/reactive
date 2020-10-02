using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;

namespace Lesson6.Tasks
{
    public class Task5
    {
        /// <summary>
        /// NOT SURE!!!
        /// 
        /// Make sure that Callback is executed faster than a 1 second. 
        /// Otherwise cancel execution and return a fallback. 
        /// Make sure Callback is not going to block central pipeline
        /// 
        /// (original: Make sure that Callback is executed faster than a 1 second. 
        /// Otherwise cancel execution and return a fallback. 
        /// Make sure Callback is not going to block central pipeline)
        /// </summary>
        public static IObservable<string> TimeoutBlockingOperation(
            Task<string> longRunningCall, TimeSpan duration, string fallback) =>
            longRunningCall.ToObservable()
                .SubscribeOn(NewThreadScheduler.Default)
                .Timeout(duration, Observable.Return(fallback));
    }
}
