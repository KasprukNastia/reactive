using System;
using System.Reactive.Linq;

namespace Lesson1.Tasks
{
    public class Task2
    {
        /// <summary>
        /// Create an IObservable just of a single element
        /// (original: Create a Flux just of a single element)
        /// </summary>
        public static IObservable<string> CreateSequenceOfASingleElement(string element) =>
            Observable.Return(element);
    }
}
