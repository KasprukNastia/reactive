using System;
using System.Reactive.Linq;

namespace Lesson2.Tasks
{
    public class Task5
	{
		/// <summary>
		/// Transform IObservable using high-order functions validate and doBusinessLogic in the following order:
		/// - `validate`
		/// - `doBusinessLogic`
		/// 
		/// (original: Transform Flux using high-order functions validate and doBusinessLogic in the following order:
		/// - `validate`
		/// - `doBusinessLogic`)
		/// </summary>
		public static IObservable<long> TransformSequence(IObservable<string> input) =>
			input.Replay(Validate).Replay(DoBusinessLogic);

		private static IObservable<long> DoBusinessLogic(IObservable<string> observable) =>
			observable.Select(s => s.Replace(oldValue: "0x", newValue: ""))
				.Select(s => long.Parse(s));

		private static IObservable<string> Validate(IObservable<string> observable) =>
			observable.Where(s => s.Length > 0)
				.Where(s => s.StartsWith("0x"));
	}
}
