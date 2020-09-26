using System;

namespace Lesson3.Tasks
{
    public class Task2
    {
        /// <summary>
        /// 
        /// (original: Merge all streams (Publisher) into a single Flux in the way the order of 
        /// all elements are corresponding to the order in which Fluxes' are merged.)
        /// </summary>
        public static IObservable<string> MergeSeveralSourcesSequential(params IObservable<string>[] sources) =>
            throw new NotImplementedException();
    }
}
