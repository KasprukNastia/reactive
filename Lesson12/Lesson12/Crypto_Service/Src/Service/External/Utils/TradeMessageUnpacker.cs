using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Lesson12.Crypto_Service.Src.Service.External.Utils
{
    public class TradeMessageUnpacker : IMessageUnpacker
    {
        private static readonly (string, int)[] FIELDS = new (string, int)[]{
            ("TYPE", 0x0),  // hex for binary 0, it is a special case of fields that are always there          TYPE
            ("MARKET", 0x0),  // hex for binary 0, it is a special case of fields that are always there        MARKET
            ("FROMSYMBOL", 0x0), // hex for binary 0, it is a special case of fields that are always there     FROM SYMBOL
            ("TOSYMBOL", 0x0), // hex for binary 0, it is a special case of fields that are always there       TO SYMBOL
            ("FLAGS", 0x0), // hex for binary 0, it is a special case of fields that are always there          FLAGS
            ("ID", 0x1), // hex for binary 1                                                                   ID
            ("TIMESTAMP", 0x2), // hex for binary 10                                                           TIMESTAMP
            ("QUANTITY", 0x4), // hex for binary 100                                                           QUANTITY
            ("PRICE", 0x8),// hex for binary 1000                                                              PRICE
            ("TOTAL", 0x10) // hex for binary 10000                                                            TOTAL
        };

        public bool Supports(string messageType) => messageType.Equals("0");

        public Dictionary<string, object> Unpack(string message)
        {
            string[] valuesArray = message.Split("~");
            int valuesArrayLenght = valuesArray.Length;
            string mask = valuesArray[valuesArrayLenght - 1];
            int maskInt = int.Parse(mask, NumberStyles.HexNumber);
            Dictionary<string, object> unpackedTrade = new Dictionary<string, object>();
            int[] currentField = { 0 };

            FIELDS.ToList().ForEach(t => {
                string k = t.Item1;
                int v = t.Item2;
                if (v == 0)
                {
                    unpackedTrade.Add(k, valuesArray[currentField[0]]);
                    currentField[0]++;
                }
                else if ((maskInt & v) > 0)
                {
                    unpackedTrade.Add(k, float.Parse(valuesArray[currentField[0]]));
                    currentField[0]++;
                }
            });

            return unpackedTrade;
        }
    }
}
