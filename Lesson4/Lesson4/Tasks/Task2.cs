using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;

namespace Lesson4.Tasks
{
    public class Task2
    {
        /// <summary>
        /// Move Task execution on the separate Thread
        /// 
        /// (original: Move Callable execution on the separate Thread)
        /// </summary>
        public static IObservable<string> SubscribeOnSingleThreadScheduler(Task<string> blockingCall) =>
            blockingCall.ToObservable().ObserveOn(new EventLoopScheduler());

        // Alternative
        public static IObservable<string> SubscribeOnSingleThreadSchedulerNew(Task<string> blockingCall) =>
            blockingCall.ToObservable().ObserveOn(NewThreadScheduler.Default);

        // Alternative
        public static IObservable<string> SubscribeOnSingleThreadSchedulerThreadPool(Task<string> blockingCall) =>
            blockingCall.ToObservable().ObserveOn(ThreadPoolScheduler.Instance);

        // Alternative
        public static IObservable<string> SubscribeOnSingleThreadSchedulerTaskPool(Task<string> blockingCall) =>
            blockingCall.ToObservable().ObserveOn(TaskPoolScheduler.Default);
    }
}
