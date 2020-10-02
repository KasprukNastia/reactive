using System;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;

namespace Lesson6.Tasks
{
    public class Task4
    {
        /// <summary>
        /// Make sure that CompletionStage executes no longer that a second. Otherwise return a fallback
        /// 
        /// (original: Make sure that CompletionStage executes no longer that a second. Otherwise return a fallback)
        /// </summary>
        public static IObservable<string> TimeoutLongOperation(Task<string> longRunningCall, TimeSpan duration, string fallback) =>
            longRunningCall.ToObservable().Timeout(duration, Observable.Return(fallback));
    }
}
