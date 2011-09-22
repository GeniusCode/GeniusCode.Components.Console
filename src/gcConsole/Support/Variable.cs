using NDesk.Options;

namespace GeniusCode.Components.Console.Support
{
    public class Variable<T> : OptionItemBase<T>
    {
        public static implicit operator T(Variable<T> input)
        {
            return input.Value;
        }

        public Variable(string prototype)
            : base(prototype)
        {
            _contianer = new SingleSet<T>();
        }



        private SingleSet<T> _contianer;
        public T Value
        {
            get { return _contianer.Value; }
            internal set
            {
                try
                {
                    _contianer.Value = value;
                }
                catch
                {
                    throw new OptionException("Parameter appeared too many times", Prototype);
                }
            }
        }
    }
}
