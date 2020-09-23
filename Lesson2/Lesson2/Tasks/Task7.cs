using System;
using System.Reactive.Linq;

namespace Lesson2.Tasks
{
    public class Task7
    {
        /// <summary>
        /// Transform IObservable into IObservable from the first emitted signal of IObservable
        /// 
        /// (original: Transform Flux into Mono from the first emitted signal Flux)
        /// </summary>
        public static IObservable<long> FirstFromFlux(IObservable<long> observable) =>
            observable.Take(1);
    }
}
