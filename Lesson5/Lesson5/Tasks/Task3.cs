using System;
using System.Reactive.Linq;

namespace Lesson5.Tasks
{
    public class Task3
    {
        /// <summary>
        /// Concat all numbers into a string that fall in the same window
        /// 
        /// (original: Concat all numbers into a String that fall in the same window)
        /// </summary>
        public static IObservable<string> BackpressureByBatching(IObservable<long> upstream, TimeSpan duration) =>
            upstream.Window(duration).Aggregate(string.Empty, (s, e) => $"{s}e", s => s);
    }
}
