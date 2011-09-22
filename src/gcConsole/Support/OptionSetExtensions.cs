using System;
using NDesk.Options;

namespace GeniusCode.Components.Console.Support
{
    public static class OptionSetExtensions
    {

        public static Flag AddFlag(this OptionSet input, string prototype, string description)
        {
            var option = new Flag();
            input.Add(prototype, description, v => option.On = !String.IsNullOrEmpty(v));
            return option;
        }

        public static Variable<T> AddVariable<T>(this OptionSet input, string prototype, string description)
        {
            var p2 = prototype + "=";
            var output = new Variable<T>(p2);
            input.Add(p2, description, value => output.Value = output.CastString(value));
            return output;
        }

        public static VariableList<T> AddVariableList<T>(this OptionSet input, string prototype, string description)
        {
            var p2 = prototype + "=";
            var output = new VariableList<T>(p2);
            input.Add(p2, description, v => output.ValuesList.Add(output.CastString(v)));
            return output;
        }

        public static VariableMatrix<T> AddVariableMatrix<T>(this OptionSet input, string prototype, string description)
        {
            var p2 = prototype + ":";
            var output = new VariableMatrix<T>(p2);
            input.Add(p2, description, (n, v) =>
                {
                    if (String.IsNullOrEmpty(n))
                        throw new OptionException("Name not specified", p2);
                    output.Matrix.Add(n, output.CastString(v));
                });

            return output;
        }

    }
}
