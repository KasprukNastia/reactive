using System;
using System.Reactive.Linq;

namespace Lesson2.Tasks
{
    public class Task1
    {
        /// <summary>
        /// Convert given IObservable<T> into IObservable<string>
        /// 
        /// (original: Convert given Publisher<T> into Publisher<String>)
        /// </summary>
        public static IObservable<string> TransformSequence<T>(IObservable<T> observable) =>
            observable.Select(o => o.ToString());
    }
}
