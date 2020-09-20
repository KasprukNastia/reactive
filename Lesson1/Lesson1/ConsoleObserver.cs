using System;

namespace Lesson1
{
    public class ConsoleObserver<T> : IObserver<T>
    {
        private readonly string _name;

        public ConsoleObserver(string name = "")
        {
            _name = name;
        }

        public void OnCompleted()
        {
            Console.WriteLine($"{_name} - OnCompleted()");
        }

        public void OnError(Exception error)
        {
            Console.WriteLine($"{_name} - OnError");
            Console.WriteLine(error.Message);
        }

        public void OnNext(T value)
        {
            Console.WriteLine($"{_name} = OnNext({value})");
        }
    }
}
