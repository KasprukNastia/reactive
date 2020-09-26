using System;
using System.Reactive.Linq;

namespace Lesson3.Tasks
{
	/// <summary>
	/// - Group all the words by first letter
	/// - Count how many times the first letter appears in the grouped words
	/// - Summarize the counts and combine them with character that represents a group
	/// 
	/// For instance:
	/// Flux("ABCA", "BCD", "ABC") ->
	/// Flux(Group-'A'("ABCA", "ABC"), Group-'B'("BCD")) ->
	/// Flux(Tuple2('A', 3), Tuple2('B', 1))
	/// 
	/// Because:
	/// We have 2 words that starts with 'A' and every word in that group has in total 3 'A' appearance.
	/// For the second group, 'B' happens only once in a single word so total is 1.
	/// </summary>
	public class Task5
    {
		public static IObservable<(char, int)> GroupWordsByFirstLatter(IObservable<string> words) =>
			throw new NotImplementedException();

		public static IObservable<IGroupedObservable<char, string>> GroupByFirstLetter(IObservable<string> words) =>
			throw new NotImplementedException();

		public static IObservable<(char, int)> CountLettersInWordsInGroup(
			IObservable<IGroupedObservable<char, string>> groupedWords) =>
			throw new NotImplementedException();
	}
}
