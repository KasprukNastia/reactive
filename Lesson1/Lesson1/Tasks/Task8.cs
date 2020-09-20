using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;

namespace Lesson1.Tasks
{
    public class Task8
    {
        public static ArgumentException VALUE_OUT_OF_BOUNDS =
			new ArgumentException("Value out of bounds");

        /// <summary>
        /// Generate IObservable which returns a given value only in case 
        /// the value is in bounds [min, max]. In case the value is out of bounds 
        /// return an error-ing IObservable instance
        /// 
        /// (original: Generate Publisher which returns a given value only in case 
        /// the value is in bounds [min, max]. In case the value is out of bounds 
        /// return an error-ing Publisher instance)
        /// </summary>
        public static IObservable<int> CreateSequence(int value, int min, int max) =>
            value >= min && value <= max ? 
            Observable.Return(value) : 
            Observable.Throw<int>(VALUE_OUT_OF_BOUNDS);
    }
}
