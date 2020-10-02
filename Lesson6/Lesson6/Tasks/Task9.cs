using System;
using System.Reactive.Linq;

namespace Lesson6.Tasks
{
    public class Task9
    {
        /// <summary>
        /// NOT SURE!!!
        /// 
        /// Make sure that Observable timeouts in case first elements only is not delivered on time
        /// 
        /// (original: Make sure that Flux timeouts in case first elements only is not delivered on time)
        /// </summary>
        public static IObservable<string> TimeoutOnce(IObservable<string> flux, TimeSpan duration, string fallback) =>
            flux.Timeout(duration, Observable.Return(fallback));
    }
}
