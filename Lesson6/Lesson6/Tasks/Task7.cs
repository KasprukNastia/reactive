using System;

namespace Lesson6.Tasks
{
    public class Task7
    {
        /// <summary>
        /// 
        /// (original: In case mapping function throws an exception, catch it and skip error value. 
        /// In that case use no additional strategies.)
        /// </summary>
        public static IObservable<int> ProvideHandmadeContinuation(IObservable<int> values, Func<int, int> mapping) =>
            throw new NotImplementedException();
    }
}
