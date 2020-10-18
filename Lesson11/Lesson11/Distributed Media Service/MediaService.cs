using System;
using System.Linq;
using System.Reactive.Linq;

namespace Lesson11.Distributed_Media_Service
{
    public class MediaService
    {
		private readonly ServersCatalogue _catalogue;

		public MediaService(ServersCatalogue catalogue)
		{
			_catalogue = catalogue;
		}

		public IObservable<Video> FindVideo(string videoName) =>
			Observable.Merge(_catalogue.Servers.Select(s => s.SearchOne(videoName))).Take(1);
	}
}
