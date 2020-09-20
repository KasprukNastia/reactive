using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace Lesson1.Tasks
{
    public class Task9
    {
		/// <summary>
		/// Create IObservable that is empty and sends onComplete only
		/// 
		/// (original: Create Flux that is empty and sends onComplete only)
		/// </summary>
		/// <remark>the Observable.Empty<long>() variant exists</remark>
		public static IObservable<long> CreateFluxEmittingOnlyOnComplete() =>
			new ObservableEmittingOnlyOnComplete<long>();

		/// <summary>
		/// Create IObservable that emits no signals and is never completes
		/// 
		/// (original: Create Flux that emits no signals and is never completes)
		/// </summary>
		public static IObservable<string> CreateFluxWhichNeverEmits() =>
			Observable.Never<string>();

		/// <summary>
		/// Create IObservable that is empty and sends onComplete only
		/// 
		/// (original: Create Mono that is empty and sends onComplete only)
		/// </summary>
		/// <remark>the Observable.Empty<long>() variant exists</remark>
		public static IObservable<long> CreateMonoEmittingOnlyOnComplete() =>
			new ObservableEmittingOnlyOnComplete<long>();

		/// <summary>
		/// Create IObservable that emits no signals and is never completes
		/// 
		/// (original: Create Mono that emits no signals and is never completes)
		/// </summary>
		public static IObservable<string> CreateMonoWhichNeverEmits() =>
			Observable.Never<string>();
	}

    public class ObservableEmittingOnlyOnComplete<T> : IObservable<T>
    {
        public IDisposable Subscribe(IObserver<T> observer)
        {
			observer.OnCompleted();
			return Disposable.Empty;
		}
    }
}
