using System;
using System.Reactive.Linq;

namespace Lesson2.Tasks
{
    public class Task10
    {
        /// <summary>
        /// Wait for the last element in the IObservable in the blocking fashion
        /// 
        /// (original: Wait for the last element in the Flux in the blocking fashion)
        /// </summary>
        public static long TransformToImperative(IObservable<long> observable) =>
            observable.Last();
    }
}
