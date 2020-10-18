using System.IO;

namespace Lesson11.Tricky_Network_Interaction
{
    public class OrderedByteBuffer
    {
		public int WritePosition { get; }
		public MemoryStream Data { get; }

		public OrderedByteBuffer(int position, MemoryStream data)
		{
			WritePosition = position;
			Data = data;
		}
	}
}
