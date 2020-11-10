using System.Collections.Generic;

namespace Lesson12.Crypto_Service.Src.Service.External.Utils
{
    public interface IMessageUnpacker
    {
        bool Supports(string messageType);

        Dictionary<string, object> Unpack(string message);
    }
}
