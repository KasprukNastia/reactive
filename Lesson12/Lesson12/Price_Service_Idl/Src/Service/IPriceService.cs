using Lesson12.Common.Src.Dto;
using System;

namespace Lesson12.Price_Service_Idl.Src.Service
{
    public interface IPriceService
    {
        IObservable<MessageDTO<float>> PricesStream(IObservable<long> intervalPreferencesStream);
    }
}
