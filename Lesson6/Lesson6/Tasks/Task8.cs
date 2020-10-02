using System;
using System.Reactive.Linq;

namespace Lesson6.Tasks
{
    public class Task8
    {
        static int RETRY_COUNT = 5;
        static string IF_MESSAGE_STARTS_WITH = "[Retry]";

        /// <summary>
        /// NOT IMPLEMENTED!!!
        /// 
        /// (original: Retry with exponential backoff strategy when Flux errors. 
        /// Note, retryable errors are the one which starts with [Retry] message)
        /// </summary>
        public static IObservable<string> RetryWithBackoffOnError(IObservable<string> publisher) =>
            publisher.Retry(RETRY_COUNT);
    }
}
