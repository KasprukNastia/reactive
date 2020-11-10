using System;
using System.Collections.Generic;

namespace Lesson12.Lesson12.Crypto_Service_Idl.Src.Service
{
    public interface ICryptoService
    {
        IObservable<Dictionary<string, object>> EventsStream();
    }
}
