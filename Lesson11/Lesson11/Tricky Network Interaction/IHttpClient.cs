using System;

namespace Lesson11.Tricky_Network_Interaction
{
    public interface IHttpClient
    {
        IObservable<int> Send(IObservable<OrderedByteBuffer> value);
    }
}
