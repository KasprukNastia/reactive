using System;
using System.Reactive.Linq;

namespace Lesson3.Tasks
{
    public class Task4
    {
        /// <summary>
        /// Having several information sources (a.k.a IObservable) you need to 
        /// subscribe to all of them and consume the whole stream only from the one which 
        /// answered earlier than others
        /// 
        /// (original: Having several information sources (a.k.a Publisher) you need to 
        /// subscribe to all of them and consume the whole stream only from the one which 
        /// answered earlier than others)
        /// </summary>
        public static IObservable<string> FromFirstEmitted(params IObservable<string>[] sources) =>
            Observable.Amb(sources);
    }
}
