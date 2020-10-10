using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Lesson7.Tasks
{
    public class Task2
    {
        /// <summary>
        /// (original: Convert stream to replayable stream. Note, replay provides stream sharing)
        /// </summary>
        public static IConnectableObservable<string> ReplayLast3ElementsInHotFashion(IObservable<string> coldSource) =>
            coldSource.Replay(3);
	}
}
