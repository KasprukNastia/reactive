using System;
using System.Linq;
using System.Reactive.Linq;

namespace Lesson9
{
    class Program
    {
        static void Main(string[] args)
        {
            var seq = Observable.Interval(TimeSpan.FromSeconds(1))
              .Do(x => Console.WriteLine(x.ToString()))
              .Buffer(5)
              .Select(y => {
                  return y;
              }) // set a breakpoint at this line
              .Do(x => Console.WriteLine("buffer is full"))
              .Subscribe(x => Console.WriteLine("Sum of the buffer is " + x.Sum()));
            
            Console.ReadKey();
        }
    }
}
