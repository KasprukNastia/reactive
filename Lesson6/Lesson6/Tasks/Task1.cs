using System;
using System.Reactive.Linq;

namespace Lesson6.Tasks
{
    public class Task1
    {
        /// <summary>
        /// Provide a fallback in case the given Observable is empty.
        /// 
        /// (original: Provide a fallback in case the given Flux is empty.)
        /// </summary>
        public static IObservable<string> FallbackIfEmpty(IObservable<string> publisher, string fallback) =>
            publisher.DefaultIfEmpty(fallback);
    }
}
