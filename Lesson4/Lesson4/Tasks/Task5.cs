using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;

namespace Lesson4.Tasks
{
    public class Task5
    {
        /// <summary>
        /// NOT SURE!!!
        /// 
        /// There is a stream of blocking calls. 
        /// Ensure every call is executed in non-blocking 
        /// (means moving call on another Thread) 
        /// way and will not block other executions. 
        /// Ensure that pool is not going over 256 created Threads
        /// 
        /// (original: There is a stream of blocking calls. 
        /// Ensure every call is executed in non-blocking 
        /// (means moving call on another Thread) 
        /// way and will not block other executions. 
        /// Ensure that pool is not going over 256 created Threads)
        /// </summary>
        public static IObservable<string> ParalellizeLongRunningWorkOnUnboundedAmountOfThread(
            IObservable<Task<string>> streamOfLongRunningSources) =>
            streamOfLongRunningSources.Select(s => s.ToObservable())
                .SelectMany(s => s)
                .ObserveOn(NewThreadScheduler.Default);
    }
}
