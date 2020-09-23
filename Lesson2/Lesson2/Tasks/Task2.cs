using System;
using System.Reactive.Linq;

namespace Lesson2.Tasks
{
    public class Task2
    {
        /// <summary>
        /// Filter given IObservable<string> where every string should be longer than 3 character
        /// 
        /// (original: Filter given Flux<String> where every String should be longer than 3 character)
        /// </summary>
        /// <param name="observable"></param>
        /// <returns></returns>
        public static IObservable<string> TransformSequence(IObservable<string> observable) =>
            observable.Where(o => o.Length > 3);
    }
}
