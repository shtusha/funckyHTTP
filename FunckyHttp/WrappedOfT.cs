using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunckyHttp
{
    //This type is needed for StepArgument transformations to work when transforming T into T.
    public class Wrapped<T> where T : class
    {
        public T Value;
        public Wrapped(T value)
        {
            Value = value;
        }

        public static implicit operator T(Wrapped<T> wrappedValue)
        {
            return wrappedValue == null ? null : wrappedValue.Value;
        }

        public static implicit operator Wrapped<T>(T value)
        {
            return new Wrapped<T>(value);
        }

        public override string ToString()
        {
            if (Value != null)
            {
                return Value.ToString();
            }
            return base.ToString();
        }
    }
}
