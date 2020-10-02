using System;
using System.Collections.Generic;
using System.Reactive.Linq;

namespace Lesson5.Tasks
{
    public class Task2
    {
        /// <summary>
        /// Keep rate of elements at one batch per second
        /// 
        /// (original: Keep rate of elements at one batch per second)
        /// </summary>
        public static IObservable<IList<long>> BackpressureByBatching(IObservable<long> upstream, TimeSpan duration) =>
            upstream.Buffer(duration);
    }
}
