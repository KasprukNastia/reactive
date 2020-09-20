using System;
using System.Linq;
using System.Reactive.Linq;

namespace Lesson1.Tasks
{
    public class Task1
    {
        public static IObservable<int> CreateSequence() =>
            Observable.Range(0, 20);

        public static IObservable<int> CreateSequenceAlternative() => 
            Enumerable.Range(0, 20).ToObservable();
    }
}
