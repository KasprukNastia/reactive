using Lesson12.Common.Src.Dto;
using System;

namespace Lesson12.Trade_Service_Idl.Src.Service
{
    public interface ITradeService
    {
        IObservable<MessageDTO<MessageTrade>> TradesStream();
    }
}
