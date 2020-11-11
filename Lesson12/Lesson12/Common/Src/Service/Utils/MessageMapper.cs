using Lesson12.Common.Src.Dto;
using System.Collections.Generic;

namespace Lesson12.Common.Src.Service.Utils
{
    public class MessageMapper
    {
        public static readonly string TYPE_KEY      = "TYPE";
        public static readonly string TIMESTAMP_KEY = "TIMESTAMP";
        public static readonly string PRICE_KEY     = "PRICE";
        public static readonly string QUANTITY_KEY  = "QUANTITY";
        public static readonly string CURRENCY_KEY  = "FROMSYMBOL";
        public static readonly string MARKET_KEY    = "MARKET";
        public static readonly string FLAGS_KEY     = "FLAGS";

        public static MessageDTO<float> MapToPriceMessage(Dictionary<string, object> messageEvent) {
            return MessageDTO<float>.Price(
                (float) messageEvent[PRICE_KEY],
                (string) messageEvent[CURRENCY_KEY],
                (string) messageEvent[MARKET_KEY]);
        }

        public static MessageDTO<MessageTrade> MapToTradeMessage(Dictionary<string, object> messageEvent)
        {
            return MessageDTO<MessageTrade>.TradeMessage(
                (long)(float) messageEvent[TIMESTAMP_KEY] * 1000,
                    (float) messageEvent[PRICE_KEY],
                    messageEvent[FLAGS_KEY].Equals("1") ? (float) messageEvent[QUANTITY_KEY] : -(float) messageEvent[QUANTITY_KEY],
                    (string) messageEvent[CURRENCY_KEY],
                    (string) messageEvent[MARKET_KEY]);
        }

        public static bool IsPriceMessageType(Dictionary<string, object> messageEvent)
        {
            return messageEvent.ContainsKey(TYPE_KEY) &&
                    messageEvent[TYPE_KEY].Equals("5");
        }

        public static bool IsValidPriceMessage(Dictionary<string, object> messageEvent)
        {
            return messageEvent.ContainsKey(PRICE_KEY) &&
                    messageEvent.ContainsKey(CURRENCY_KEY) &&
                    messageEvent.ContainsKey(MARKET_KEY);
        }

        public static bool IsTradeMessageType(Dictionary<string, object> messageEvent)
        {
            return messageEvent.ContainsKey(TYPE_KEY) &&
                    messageEvent[TYPE_KEY].Equals("0");
        }
    }
}
