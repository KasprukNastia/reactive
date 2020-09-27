using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace Lesson4.Tasks
{
    public class Task4
    {
        /// <summary>
        /// NOT SURE ABOUT TASK!!!
        /// 
        /// Modify code so that stream's subscription happens 
        /// on the CurrentThreadScheduler. Modify the pipeline so that only work2 
        /// happens on the different Thread than the one related to 
        /// subscription phase. For instance, use NewThreadScheduler
        /// 
        /// (original: Modify code so that stream's subscription happens 
        /// on the Scheduler.single. Modify the pipeline so that only work2 
        /// happens on the different Thread than the one related to 
        /// subscription phase. For instance, use Scheduler.parallel)
        /// </summary>
        public static IObservable<long> ModifyStreamExecution(
            IObservable<long> observable,
            Func<long, long> work1, Func<long, long> work2) =>
            observable.Select(work1).ObserveOn(CurrentThreadScheduler.Instance)
                .Select(work2).ObserveOn(NewThreadScheduler.Default);
    }
}
