using System;
using System.Reactive.Linq;

namespace Lesson2.Tasks
{
    public class Task3
    {
        /// <summary>
        /// Transform IObservable<string> into IObservable<char> of chars.
        /// 
        /// (original: Transform Flux<String> into Flux<Character> of chars)
        /// </summary>
        public static IObservable<char> CreateSequence(IObservable<string> stringObservable) =>
            stringObservable.SelectMany(str => str.ToCharArray());
    }
}
