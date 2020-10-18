namespace Lesson11.Distributed_Media_Service
{
    public class Video
    {
		public string Name { get; }
		public string Location { get; }
		public string Description { get; }

		public Video(string name, string location, string description)
		{
			Name = name;
			Location = location;
			Description = description;
		}
	}
}
