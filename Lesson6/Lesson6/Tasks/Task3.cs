using System;
using System.Reactive.Linq;

namespace Lesson6.Tasks
{
    public class Task3
    {
        /// <summary>
        /// Retry operation in case of error
        /// 
        /// (original: Retry operation in case of error)
        /// </summary>
        public static IObservable<string> RetryOnError(IObservable<string> publisher) =>
            publisher.Retry();
    }
}
