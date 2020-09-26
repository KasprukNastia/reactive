using System;
using System.Threading.Tasks;

namespace Lesson4.Tasks
{
    public class Task5
    {
        /// <summary>
        /// 
        /// (original: There is a stream of blocking calls. 
        /// Ensure every call is executed in non-blocking 
        /// (means moving call on another Thread) 
        /// way and will not block other executions. 
        /// Ensure that pool is not going over 256 created Threads)
        /// </summary>
        public static IObservable<string> ParalellizeLongRunningWorkOnUnboundedAmountOfThread(
            IObservable<Task<String>> streamOfLongRunningSources) =>
            throw new NotImplementedException();
    }
}
