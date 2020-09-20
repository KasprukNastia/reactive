using System;
using System.Reactive.Linq;

namespace Lesson1.Tasks
{
    public class Task2
    {
        public static IObservable<string> CreateSequenceOfASingleElement(string element) =>
            Observable.Return(element);
    }
}
