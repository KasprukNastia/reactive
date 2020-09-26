using System;

namespace Lesson3.Tasks
{
    public class Task7
    {
        /// <summary>
        /// 
        /// (original: Combine elements from Publisher's similarly to Flux.zip 
        /// but in the way to combine every new element with the latest 
        /// observed from the neighbors)
        /// </summary>
        public static IObservable<string> CombineSeveralSources(IObservable<string> prefixPublisher,
            IObservable<string> wordPublisher,
            IObservable<string> suffixPublisher) =>
            throw new NotImplementedException();
    }
}
