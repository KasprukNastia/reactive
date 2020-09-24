using System;

namespace Lesson2.Tasks
{
    public class Task11
    {
		/// <summary>
		/// 
		/// 
		/// (original: Solve a FizzBuzz problem using Flux API)
		/// </summary>
		public static IObservable<string> FizzBuzz(IObservable<Integer> input) =>
			throw new NotImplementedException();

		public class IndexedWord
		{
			private int Index { get; }
			private string Word { get; }

			public IndexedWord(int index, string word)
			{
				Index = index;
				Word = word ?? throw new ArgumentNullException(nameof(word));
			}

			public override bool Equals(object o)
			{
				if (o == this)
				{
					return true;
				}
				if (!(o is IndexedWord)) {
					return false;
				}
				IndexedWord other = (IndexedWord)o;
				if (this.Index != other.Index)
				{
					return false;
				}
				object thisWord = this.Word;
				object otherWord = other.Word;
				if (thisWord == null ? otherWord != null : !thisWord.Equals(otherWord)) {
					return false;
				}
				return true;
			}

			public override int GetHashCode()
			{
				int PRIME = 59;
				int result = 1;
				result = result * PRIME + this.Index;
				object word = this.Word;
				result = result * PRIME + (word == null ? 43 : word.GetHashCode());
				return result;
			}

			public override string ToString()
			{
				return "IndexedWord(index=" + this.Index + ", word=" + this.Word + ")";
			}
		}
	}
}
