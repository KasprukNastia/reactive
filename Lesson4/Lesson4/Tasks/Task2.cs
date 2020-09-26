using System;
using System.Threading.Tasks;

namespace Lesson4.Tasks
{
    public class Task2
    {
        /// <summary>
        /// 
        /// (original: Move Callable execution on the separate Thread)
        /// </summary>
        public static IObservable<string> SubscribeOnSingleThreadScheduler(Task<string> blockingCall) =>
            throw new NotImplementedException();
    }
}
