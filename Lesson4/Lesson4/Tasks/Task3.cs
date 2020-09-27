using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace Lesson4.Tasks
{
    public class Task3
    {
        /// <summary>
        /// Parallel execution on several Threads
        /// 
        /// (original: Parallel execution on several Threads)
        /// </summary>
        public static IObservable<int> ParalellizeWorkOnDifferentThreads(IObservable<int> source) =>
            source.ObserveOn(NewThreadScheduler.Default);
    }
}
