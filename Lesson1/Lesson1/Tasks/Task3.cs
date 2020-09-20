using System;
using System.Reactive.Linq;

namespace Lesson1.Tasks
{
    public class Task3
    {
        /// <summary>
        /// Generate an IObservable from array
        /// (original: Generate a Flux from array)
        /// </summary>
        public static IObservable<string> CreateSequence(params string[] args) =>
            args.ToObservable();
    }
}
