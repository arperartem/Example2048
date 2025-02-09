namespace Example2048
{
    public class BaseValue
    {
        public static readonly BaseValue Zero = new(0);
        public static readonly BaseValue Two = new(2);
        public static readonly BaseValue Four = new(4);
        
        private readonly int _value;

        public int Value => _value;
        
        public BaseValue(int value)
        {
            _value = value;
        }
        
        public static implicit operator BaseValue(int number) => new(number);
        public static implicit operator int(BaseValue number) => number.Value;
        public static BaseValue operator %(BaseValue a, BaseValue b) => new(a.Value % b.Value);
        public static BaseValue operator %(int a, BaseValue b) => new(a % b.Value);
        public static BaseValue operator %(BaseValue a, int b) => new(a.Value % b);
        public static BaseValue operator *(BaseValue a, BaseValue b) => new(a.Value * b.Value);
        
        public static bool operator ==(BaseValue left, BaseValue right)
        {
            if (ReferenceEquals(left, right)) 
                return true;
            if (left is null || right is null)
                return false;
            
            return left.Value == right.Value;
        }
        
        public static bool operator !=(BaseValue left, BaseValue right)
        {
            return !(left == right);
        }
        
        protected bool Equals(BaseValue other)
        {
            return _value == other._value;
        }

        public override bool Equals(object obj)
        {
            if (obj is null) 
                return false;
            
            if (ReferenceEquals(this, obj))
                return true;
            
            if (obj.GetType() != GetType())
                return false;
            
            return Equals((BaseValue)obj);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return _value.ToString();
        }
    }
}