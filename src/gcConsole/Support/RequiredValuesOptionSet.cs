using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using NDesk.Options;

namespace GeniusCode.Components.Console.Support
{


    /// <summary>
    /// Derives from OptionSet, and adds capability for variables that are required
    /// </summary>
    /// <remarks>http://www.ndesk.org/doc/ndesk-options/NDesk.Options/OptionSet.html</remarks>
    public class RequiredValuesOptionSet : OptionSet
    {
        public IEnumerable<Option> GetMissingVariables()
        {
            // get items in dictionary where there is no entry
            var q = from t in _requiredVariableValues
                    join o in this as KeyedCollection<string, Option> on t.Key equals o.Prototype
                    where t.Value == false
                    select o;

            return q;
        }

        /// <summary>
        /// Dictionary that holds whether or not prototype variables have been set
        /// </summary>
        private readonly Dictionary<string, bool> _requiredVariableValues = new Dictionary<string, bool>();

        public Variable<T> AddRequiredVariable<T>(string prototype, string description)
        {
            var p2 = prototype + "=";
            _requiredVariableValues.Add(p2, false);

            var output = new Variable<T>(p2);
            Add(p2, description, (T v) =>
                {
                    output.Value = v;
                    _requiredVariableValues[p2] = true;
                });
            return output;

        }

    }
}
