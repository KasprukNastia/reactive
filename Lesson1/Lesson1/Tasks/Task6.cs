using System;
using System.Reactive.Linq;

namespace Lesson1.Tasks
{
    public class Task6
    {
        /// <summary>
        /// Create IObservable which emits elements in interval
        /// 
        /// (original: Create Flux which emits elements in interval)
        /// </summary>
        public static IObservable<long> CreateSequence(TimeSpan duration) =>
            Observable.Interval(duration);
    }
}
