using System;

namespace Lesson3.Tasks
{
    public class Task8
    {
		/// <summary>
		/// 
		/// (original: Depends on the clientPreferences Flux, 
		/// switch between vanillaIceCreamStream and chocolateIceCreamStream)
		/// </summary>
		public static IObservable<IceCreamBall> FillIceCreamWaffleBowl(IObservable<IceCreamType> clientPreferences,
			IObservable<IceCreamBall> vanillaIceCreamStream,
			IObservable<IceCreamBall> chocolateIceCreamStream) =>
			throw new NotImplementedException();

		public class IceCreamBall
		{
			private readonly string _type;

			public IceCreamBall(string type)
			{
				_type = type;
			}

			public override bool Equals(object o)
			{
				if (this == o)
				{
					return true;
				}
				if (o == null || !(o is IceCreamBall))
				{
					return false;
				}

				IceCreamBall that = (IceCreamBall)o;

				return _type != null ? _type.Equals(that._type) : that._type == null;
			}

            public override int GetHashCode()
			{
				return _type != null ? _type.GetHashCode() : 0;
			}

			public static IceCreamBall Ball(String type)
			{
				return new IceCreamBall(type);
			}

            public override string ToString()
			{
				return _type;
			}
		}

		public enum IceCreamType
		{
			VANILLA, CHOCOLATE
		}
	}
}
