using System;
using System.Reactive.Subjects;

namespace Lesson5.Tasks
{
    public class Task1
    {
        /// <summary>
        /// // Do not have 'Backpressure' analogues !!!
        /// 
        /// (original: Make sure your subscriber is not going to be overwhelmed by elements. 
        /// Drop all unrequested elements. Release dropped elements.)
        /// </summary>
        public static IConnectableObservable<int> DropElementsOnBackpressure(IConnectableObservable<int> upstream) =>
            throw new NotImplementedException();
    }
}
