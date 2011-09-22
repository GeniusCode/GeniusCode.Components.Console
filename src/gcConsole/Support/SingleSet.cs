using System;
using NDesk.Options;

namespace GeniusCode.Components.Console.Support
{
    internal struct SingleSet<T>
    {
        private bool _wasSet;

        private T _value;
        public T Value
        {
            get { return _value; }
            set
            {
                if (_wasSet)
                    throw new Exception("Value set more than once.");

                _value = value;
                _wasSet = true;
            }
        }
    }
}