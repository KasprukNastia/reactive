using System;

namespace Lesson5.Tasks
{
    /// <summary>
    /// NOT SURE!!!
    /// 
    /// Provide your own implementation of Subscriber which implements the following backpressure rules:
    /// - The first request should be equal to 1
    /// - Every next request should be twice more than previous
    /// - The request must be performed only in case the previous was fulfilled - DID NOT UNDERSTARD!!!
    /// </summary>
    public class Task5
    {
        public static void DynamicDemand(IObservable<int> source) =>
            source.Subscribe(onNext: v => v *= 2);
    }
}
