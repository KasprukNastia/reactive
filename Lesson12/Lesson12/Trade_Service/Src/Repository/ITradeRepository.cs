using Lesson12.Trade_Service.Src.Domain;
using System;
using System.Collections.Generic;

namespace Lesson12.Trade_Service.Src.Repository
{
    public interface ITradeRepository
    {
        IObservable<int> SaveAll(List<Trade> input);
    }
}
