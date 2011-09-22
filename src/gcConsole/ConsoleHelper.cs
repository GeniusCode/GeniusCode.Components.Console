using System;

namespace GeniusCode.Components.Console
{
    public static class ConsoleHelper
    {
        public static void MakeOptionExceptionsVisualStudioAware(this ConsoleManager manager)
        {
            manager.OnMessageForMissingVariables = missingVars =>
                {
                    var errorMessage = "\n Missing Vars: \n";
                    missingVars.ForEach(option => errorMessage += option.Prototype + "\n");

                    return CreateVisualStudioErrorString(manager.ConsoleName, "100", errorMessage);
                };

            manager.OnExceptionMessage = ex =>
                                             {
                                                 const string errorMessage = "\n An unexpected argument exception occurred: \n";
                                                 return CreateVisualStudioErrorString(manager.ConsoleName, "101", errorMessage);
                                             };
        }

        public static string CreateVisualStudioErrorString(string consoleName, string errorNumber, string errorDescription)
        {
            // Visual Studio Error Handling
            // http://msdn.microsoft.com/en-us/library/yxkt8b26.aspx

            return String.Format("{0} : error Code{1} : {2}", consoleName, errorNumber, errorDescription);
        }
    }
}
