using System.Collections.Generic;

namespace Lesson11.Distributed_Media_Service
{
    public class ServersCatalogue
    {
		public List<Server> Servers { get; } =
			new List<Server>
			{
				new Server("http://a.servers.com"),
				new Server("http://b.servers.com"),
				new Server("http://c.servers.com"),
				new Server("http://d.servers.com"),
				new Server("http://e.servers.com"),
				new Server("http://f.servers.com")
			};
	}
}
