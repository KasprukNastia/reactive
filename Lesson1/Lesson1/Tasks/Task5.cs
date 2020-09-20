using System;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;

namespace Lesson1.Tasks
{
    public class Task5
    {
        /// <summary>
        /// Generate IObservable from Task
        /// (original: Generate Mono from Callable)
        /// </summary>
        public static IObservable<string> CreateSequence(Task<string> callable) =>
            callable.ToObservable();
    }
}
