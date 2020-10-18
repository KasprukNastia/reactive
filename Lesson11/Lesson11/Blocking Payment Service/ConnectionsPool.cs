using System;
using System.Threading;

namespace Lesson11.Blocking_Payment_Service
{
    public class ConnectionsPool
    {
		private static readonly ConnectionsPool _connectionsPool = new ConnectionsPool(20);
		public static ConnectionsPool Instance => _connectionsPool;

		private long _connectionsCounter;

		public int Size { get; }

		private ConnectionsPool(int size)
		{
			Size = size;
		}

		public void Release()
		{
			Interlocked.Decrement(ref _connectionsCounter);
		}

		public void TryAcquire()
		{
			long currentConnectionsCounter = Interlocked.Read(ref _connectionsCounter);

			if (currentConnectionsCounter >= Size)
				throw new InvalidOperationException("No available connections in the pool");

			Interlocked.Increment(ref _connectionsCounter);
		}
	}
}
