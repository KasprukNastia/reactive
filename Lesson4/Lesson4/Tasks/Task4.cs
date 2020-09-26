using System;
using System.Reactive.Linq;

namespace Lesson4.Tasks
{
    public class Task4
    {
        /// <summary>
        /// 
        /// (original: Modify code so that stream's subscription happens 
        /// on the Scheduler.single. Modify the pipeline so that only work2 
        /// happens on the different Thread than the one related to 
        /// subscription phase. For instance, use Scheduler.parallel)
        /// </summary>
        public static IObservable<long> ModifyStreamExecution(
            IObservable<long> flux,
            Func<long, long> work1, Func<long, long> work2) =>
            flux.Select(work1).Select(work2);
    }
}
