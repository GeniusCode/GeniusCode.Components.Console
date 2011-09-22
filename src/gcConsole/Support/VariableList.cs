using System.Collections.Generic;

namespace GeniusCode.Components.Console.Support
{
    public class VariableList<T> : OptionItemBase<T>
    {

        public VariableList(string prototype)
            : base(prototype)
        {

        }

        internal List<T> ValuesList = new List<T>();

        public IEnumerable<T> Values
        {
            get
            {
                return ValuesList;
            }
        }

    }
}
