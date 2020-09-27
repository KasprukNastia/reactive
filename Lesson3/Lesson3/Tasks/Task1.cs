using System;
using System.Reactive.Linq;

namespace Lesson3.Tasks
{
    public class Task1
    {
        /// <summary>
        /// Merge several sources (IObservable) into a single IObservable
        /// 
        /// (original: Merge several sources (Publisher) into a single Flux)
        /// </summary>
        public static IObservable<string> MergeSeveralSources(params IObservable<string>[] sources) =>
            Observable.Merge(sources);
    }
}
