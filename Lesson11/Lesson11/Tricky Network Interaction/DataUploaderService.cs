using System;
using System.Reactive.Linq;

namespace Lesson11.Tricky_Network_Interaction
{
    public class DataUploaderService
    {
		private readonly IHttpClient _client;

		public DataUploaderService(IHttpClient client)
		{
			_client = client;
		}

		public IObservable<int> upload(IObservable<OrderedByteBuffer> input) =>
			_client.Send(input.Window(TimeSpan.FromMilliseconds(500), 50).SelectMany(o => o))
				.Timeout(TimeSpan.FromSeconds(1))
				.Retry();
	}
}
