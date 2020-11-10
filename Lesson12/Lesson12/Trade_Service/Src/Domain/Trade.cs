namespace Lesson12.Trade_Service.Src.Domain
{
    public class Trade
    {
		public string Id { get; set; }
		public long Timestamp { get; set; }
		public float Price { get; set; }
		public float Amount { get; set; }
		public string Currency { get; set; }
		public string Market { get; set; }

		public override bool Equals(object o)
		{
			if (this == o)
				return true;
			if (o == null || !(o is Trade trade))
				return false;

			if (Timestamp != trade.Timestamp)
				return false;
			if (trade.Price != Price)
				return false;
			if (trade.Amount != Amount)
				return false;
			if (Id != null ? !Id.Equals(trade.Id) : trade.Id != null)
				return false;
			if (!Currency.Equals(trade.Currency))
				return false;

			return Market.Equals(trade.Market);
		}

		public override int GetHashCode()
		{
			int result = Id != null ? Id.GetHashCode() : 0;
			result = 31 * result + (int)(Timestamp ^ (Timestamp >> 32));
			result = 31 * result + (Price != +0.0f ? (int)Price : 0);
			result = 31 * result + (Amount != +0.0f ? (int)Amount : 0);
			result = 31 * result + Currency.GetHashCode();
			result = 31 * result + Market.GetHashCode();
			return result;
		}

		public override string ToString()
		{
			return "Trade{" + "Id=" + Id + ", Timestamp=" + Timestamp + ", Price=" + Price + ", Amount=" + Amount + ", currency='" + Currency + '\'' + ", market='" + Market + '\'' + '}';
		}
	}
}
