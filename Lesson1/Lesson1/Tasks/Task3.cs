using System;
using System.Reactive.Linq;

namespace Lesson1.Tasks
{
    public class Task3
    {
        public static IObservable<string> CreateSequence(params string[] args) =>
            args.ToObservable();
    }
}
