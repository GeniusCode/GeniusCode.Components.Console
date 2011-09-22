using System;

namespace GeniusCode.Components.Console
{
    public class Requirement
    {
        public Func<bool> RequirementSatisfied { get; set; }
        public string[] Text { get; set; }
    }
}