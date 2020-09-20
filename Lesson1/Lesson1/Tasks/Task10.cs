using System;
using System.Reactive.Linq;

namespace Lesson1.Tasks
{
    public class Task10
    {
		/// <summary>
		/// Create IObservable that generates a Fibonacci sequence with 20 iterations depth
		/// 
		/// (original: Create Flux that generates a Fibonacci sequence with 20 iterations depth)
		/// </summary>
		public static IObservable<long> CreateSequence() =>
			Observable.Generate(
				STATE_ONE,
				s => s.Iteration < 20,
				s => new State(s.Iteration + 1, s.Previous.Value + s.Value, s),
				s => s.Value);

        class State
		{
			public State Previous { get; }
			public long Value { get; }
			public long Iteration { get; }

			public State(long iteration, long value, State previous)
			{
				Iteration = iteration;
				Previous = previous;
				Value = value;
			}
		}

		private static State STATE_ZERO  = new State(0, 0, null);
		private static State STATE_ONE = new State(1, 1, STATE_ZERO);
	}
}
