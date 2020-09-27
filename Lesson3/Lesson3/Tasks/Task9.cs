using System;
using System.IO;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;

namespace Lesson3.Tasks
{
    public class Task9
    {
        /// <summary>
        /// (original: 
        /// 1. Open File safely (use File.ReadAllLinesAsync API)
        /// 2. Asynchronously read all the lines
        /// 3. Close opened resource on terminal operation)
        /// 
        /// (original: 
        /// 1. Open File safely (use Files.lines API)
        /// 2. Asynchronously read all the lines
        /// 3. Close opened resource on terminal operation)
        /// </summary>
        public static IObservable<string> ReadFile(string filename) =>
            Observable.Using(
                () => File.ReadAllLinesAsync(filename), 
                allLines => allLines.ToObservable().SelectMany(str => str));
    }
}
