using System;
using System.Reactive.Linq;

namespace Lesson6.Tasks
{
    public class Task6
    {
        /// <summary>
        /// NOT SURE!!!
        /// 
        /// (original: Continue stream in case of exception in .map)
        /// </summary>
        public static IDisposable ProvideSupportOfContinuation(
            IObservable<int> values,
            IObserver<object> consumer) =>
            values.Subscribe(onNext: val => { consumer.OnNext(val); }, onError: e => { }, onCompleted: () => { consumer.OnCompleted(); });
    }
}
