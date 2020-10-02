using System;
using System.Reactive.Joins;
using System.Reactive.Linq;

namespace Lesson2.Tasks
{
    public class Task9
    {
        /// <summary>
        /// 
        /// 
        /// (original: Ignore all elements from Flux and then propagate onComplete only)
        /// </summary>
        public static Plan<long> WaitUntilFluxCompletion(IObservable<long> observable) =>
            observable.Then(n => n);
    }
}
