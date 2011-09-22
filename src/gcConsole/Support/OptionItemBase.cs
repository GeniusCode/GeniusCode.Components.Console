using System;
using NDesk.Options;

namespace GeniusCode.Components.Console.Support
{
    public abstract class OptionItemBase<T>
    {

        protected internal virtual T CastString(string input)
        {
            return (T)Convert.ChangeType(input, typeof(T));
        }

        protected OptionItemBase(string prototype)
        {
            Prototype = prototype;
        }

        protected string Prototype { get; private set; }

        protected OptionException GetException(string message)
        {
            return new OptionException(message, Prototype);
        }

    }
}
