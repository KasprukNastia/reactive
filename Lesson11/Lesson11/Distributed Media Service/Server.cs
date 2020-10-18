using System;
using System.Reactive.Linq;

namespace Lesson11.Distributed_Media_Service
{
    public class Server
    {
		private readonly string _address;

		public Server(string address)
		{
			_address = address;
		}

		public IObservable<Video> SearchOne(string name)
		{
			var random = new Random();
			bool doubled = Convert.ToBoolean(random.Next(0, 1));

			return Observable.Return(new Video(name, _address, "Some fake description"))
				.DelaySubscription(TimeSpan.FromMilliseconds(random.Next(200, doubled ? 6000 : 3000)));
		}
	}
}
