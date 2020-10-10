using System;
using System.Reactive.Linq;

namespace Lesson7.Tasks
{
    public class Task1
    {
        /// <summary>
        /// (original: Share Flux in such a way so backpressure will be controlled by all the subscribers.
        /// Also, it should connect to the main Flux when the total number of subscriber reach number 3. 
        /// In case all the subscribers has been disconnected, it should cancel the subscription.)
        /// </summary>
        public static IObservable<string> TransformToHotWithOperator(IObservable<string> coldSource) =>
            coldSource.Publish().AutoConnect(3).RefCount();
    }
}
