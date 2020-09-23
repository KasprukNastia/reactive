using System;
using System.Reactive.Linq;

namespace Lesson2.Tasks
{
    public class Task6
    {
        /// <summary>
        /// Sum all elements in the given IObservable<long>
        /// 
        /// (original: Sum all elements in the given Flux<Long>)
        /// </summary>
        public static IObservable<long> CreateSequence(IObservable<long> observable) =>
            observable.Sum();
    }
}
