using System;

namespace Lesson3.Tasks
{
    public class Task3
    {
        /// <summary>
        /// 
        /// (original: ***Concat*** all streams ( Publisher) into a single Flux in the way the order 
        /// of all elements are corresponding to the order in which Fluxes' are merged.)
        /// </summary>
        public static IObservable<string> ConcatSeveralSourcesOrdered(params IObservable<string>[] sources) =>
            throw new NotImplementedException();
    }
}
