using System;

namespace Lesson3.Tasks
{
    public class Task6
    {
        /// <summary>
        /// 
        /// (original: Gather elements from two sources so every element is 
        /// combined with a corresponding one from the neighbor stream)
        /// </summary>
        public static IObservable<string> ZipSeveralSources(IObservable<string> prefixPublisher,
            IObservable<string> wordPublisher,
            IObservable<string> suffixPublisher) =>
            throw new NotImplementedException();
    }
}
