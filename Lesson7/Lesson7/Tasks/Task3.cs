using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Lesson7.Tasks
{
    public class Task3
    {
        /// <summary>
        /// (original: Convert stream to shared stream but with no backpressure support)
        /// </summary>
        public static IConnectableObservable<string> TransformToHotUsingProcessor(IObservable<string> coldSource) =>
            coldSource.Multicast(new Subject<string>());
    }
}
