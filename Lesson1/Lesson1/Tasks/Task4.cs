using System;
using System.Collections.Generic;
using System.Reactive.Linq;

namespace Lesson1.Tasks
{
    public class Task4
    {
        /// <summary>
        /// Create IObservable from IEnumerable 
        /// (original: Create Flux from Java Stream)
        /// </summary>
        public static IObservable<string> CreateSequence(IEnumerable<string> stream) =>
            stream.ToObservable();
    }
}
