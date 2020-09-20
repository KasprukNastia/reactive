using System;
using System.Linq;
using System.Reactive.Linq;

namespace Lesson1.Tasks
{
    public class Task1
    {
        /// <summary>
        /// Create IObservable that emits in range [0..20)
        /// 
        /// (original: Create Flux that emits in range [0..20))
        /// </summary>
        public static IObservable<int> CreateSequence() =>
            Observable.Range(0, 20);

        /// <summary>
        /// Create IObservable that emits in range [0..20)
        /// </summary>
        public static IObservable<int> CreateSequenceAlternative() => 
            Enumerable.Range(0, 20).ToObservable();
    }
}
