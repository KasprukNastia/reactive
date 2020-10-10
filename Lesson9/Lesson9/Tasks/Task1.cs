using Microsoft.Reactive.Testing;
using System;
using System.Reactive;

namespace Lesson9.Tasks
{
    public class Task1
    {
        /// <summary>
        /// (original: Test a Flux that returns 10 random elements)
        /// </summary>
        public static void VerifyThat10ElementsEmitted()
        {
            var scheduler = new TestScheduler();

            var random = new Random();
            var testObservable = scheduler.CreateHotObservable(
                ReactiveTest.OnNext(10, random.Next(-100, 100)),
                ReactiveTest.OnNext(20, random.Next(-100, 100)),
                ReactiveTest.OnNext(30, random.Next(-100, 100)),
                ReactiveTest.OnNext(40, random.Next(-100, 100)),
                ReactiveTest.OnNext(50, random.Next(-100, 100)),
                ReactiveTest.OnNext(60, random.Next(-100, 100)),
                ReactiveTest.OnNext(70, random.Next(-100, 100)),
                ReactiveTest.OnNext(80, random.Next(-100, 100)),
                ReactiveTest.OnNext(90, random.Next(-100, 100)),
                ReactiveTest.OnNext(100, random.Next(-100, 100)),
                ReactiveTest.OnCompleted<int>(110));

            var results = scheduler.Start(() => testObservable,
                created: 1,
                subscribed: 5,
                disposed: 120);

            Func<int, bool> valuesPredicate = value => value >= -100 && value <= 100;

            ReactiveAssert.AreElementsEqual(results.Messages,
                new Recorded<Notification<int>>[]
                {
                    ReactiveTest.OnNext(10, valuesPredicate),
                    ReactiveTest.OnNext(20, valuesPredicate),
                    ReactiveTest.OnNext(30, valuesPredicate),
                    ReactiveTest.OnNext(40, valuesPredicate),
                    ReactiveTest.OnNext(50, valuesPredicate),
                    ReactiveTest.OnNext(60, valuesPredicate),
                    ReactiveTest.OnNext(70, valuesPredicate),
                    ReactiveTest.OnNext(80, valuesPredicate),
                    ReactiveTest.OnNext(90, valuesPredicate),
                    ReactiveTest.OnNext(100, valuesPredicate),
                    ReactiveTest.OnCompleted<int>(110)
                });
        }
    }
}
