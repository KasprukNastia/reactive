using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace Lesson4.Tasks
{
    public class Task1
    {
        /// <summary>
        /// Move elements processing to another Thread. 
        /// Use System.Reactive.Concurrency for that purpose Observable
        /// 
        /// (original: Move elements processing to another Thread. 
        /// Use Schedulers.parallel for that purpose Flux)
        /// </summary>
        public static IObservable<string> PublishOnParallelThreadScheduler(IObservable<string> source) =>
            source.ObserveOn(new EventLoopScheduler());
    }
}
