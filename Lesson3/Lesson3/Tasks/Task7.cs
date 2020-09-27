using System;
using System.Reactive.Linq;

namespace Lesson3.Tasks
{
    public class Task7
    {
        /// <summary>
        /// Combine elements from Publisher's similarly to IObservable.zip 
        /// but in the way to combine every new element with the latest 
        /// observed from the neighbors
        /// 
        /// (original: Combine elements from Publisher's similarly to Flux.zip 
        /// but in the way to combine every new element with the latest 
        /// observed from the neighbors)
        /// </summary>
        public static IObservable<string> CombineSeveralSources(IObservable<string> prefixPublisher,
            IObservable<string> wordPublisher,
            IObservable<string> suffixPublisher) =>
            prefixPublisher.CombineLatest(wordPublisher, suffixPublisher, (p, w, s) => $"{p}{w}{s}");
    }
}
