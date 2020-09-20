using System;
using System.Collections.Generic;
using System.Reactive.Linq;

namespace Lesson1.Tasks
{
    public class Task4
    {
        public static IObservable<string> CreateSequence(IEnumerable<string> stream) =>
            stream.ToObservable();
    }
}
