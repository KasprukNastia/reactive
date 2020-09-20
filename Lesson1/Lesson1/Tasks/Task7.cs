using System;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;

namespace Lesson1.Tasks
{
    public class Task7
    {
        /// <summary>
        /// NOT SURE!!!
        /// 
        /// (original: Adapt CompletionStage to Publisher using Project Reactor)
        /// </summary>
        public static IObservable<string> CreateSequence(Task<string> callable) =>
            callable.ToObservable();
    }
}
