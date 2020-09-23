using System;
using System.Reactive.Linq;

namespace Lesson2.Tasks
{
    public class Task4
    {
        /// <summary>
        /// Take only two last elements from the given IObservable
        /// 
        /// (original: Take only two last elements from the given Flux)
        /// </summary>
        public static IObservable<string> TransformSequence(IObservable<string> stringObservable) =>
            stringObservable.TakeLast(2);
    }
}
