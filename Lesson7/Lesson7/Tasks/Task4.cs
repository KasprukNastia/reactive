using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Lesson7.Tasks
{
    public class Task4
    {
        /// <summary>
        /// (original: Share Flux in such a way so backpressure will be controlled by all the subscribers. 
        /// Use a corresponding Processor type for that purpose)
        /// </summary>
        public static IConnectableObservable<string> TransformToHot2(IObservable<string> coldSource) =>
            coldSource.Multicast(new ReplaySubject<string>());
    }
}
