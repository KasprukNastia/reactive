using System;
using System.Collections.Generic;
using System.Reactive.Linq;

namespace Lesson2.Tasks
{
    public class Task8
    {
        /// <summary>
        /// NOT SURE!!!
        /// Collect all elements in IObservable into list
        /// 
        /// (original: Collect all elements in Flux into list)
        /// </summary>
        public static IObservable<IList<string>> TransformToList(IObservable<string> observable) =>
            observable.Buffer(int.MaxValue);
    }
}
