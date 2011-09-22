using System.Collections.Generic;

namespace GeniusCode.Components.Console.Support
{
    public class VariableMatrix<T> : OptionItemBase<T>
    {
        public VariableMatrix(string prototype)
            : base(prototype)
        {
            Matrix = new Dictionary<string, T>();
        }

        public Dictionary<string, T> Matrix { get; private set; }

    }
}
