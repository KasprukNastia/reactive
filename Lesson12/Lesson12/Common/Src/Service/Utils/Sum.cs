namespace Lesson12.Common.Src.Service.Utils
{
    public class Sum
    {
        public float Value { get; }
        public int Counter { get; }

        public Sum(float value, int counter)
        {
            Value = value;
            Counter = counter;
        }

        public Sum Add(float Value) => new Sum(Value + Value, Counter + 1);

        public float Avg() => Value / Counter;

        public static Sum Empty() => new Sum(0, 0);

        public override bool Equals(object o)
        {
            if (o == this) return true;
            if (!(o is Sum other)) return false;
            if (Value != other.Value) return false;
            if (Counter != other.Counter) return false;
            return true;
        }

        public override int GetHashCode()
        {
            int PRIME = 59;
            int result = 1;
            result = result * PRIME + (int)Value;
            result = result * PRIME + Counter;
            return result;
        }

        public override string ToString()
        {
            return "Sum(Value=" + Value + ", Counter=" + Counter + ")";
        }
    }
}
