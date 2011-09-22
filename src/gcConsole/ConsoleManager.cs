using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GeniusCode.Components.Console.Support;
using NDesk.Options;

namespace GeniusCode.Components.Console
{
    public class ConsoleManager
    {

        #region Constructers
        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleManager"/> class using -? as the help flag
        /// </summary>
        /// <param name="os">The os.</param>
        /// <param name="consoleName">Name of the console.</param>
        public ConsoleManager(RequiredValuesOptionSet os, string consoleName)
            : this(os, consoleName, "?", new string[] { })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleManager"/> class using a specified value for help flag and text
        /// </summary>
        /// <param name="os">The os.</param>
        /// <param name="consoleName">Name of the console.</param>
        /// <param name="helpFlag">The help flag.</param>
        /// <param name="text">The text.</param>
        public ConsoleManager(RequiredValuesOptionSet os, string consoleName, string helpFlag, string[] text)
            : this(os, consoleName, null)
        {
            var help = os.AddFlag(helpFlag, "help");
            _helpInfo = new HelpInfo("-" + helpFlag, () => help, text);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleManager"/> class.
        /// </summary>
        /// <param name="os">The os.</param>
        /// <param name="consoleName">Name of the console.</param>
        /// <param name="helpInfo">The help info.</param>
        private ConsoleManager(RequiredValuesOptionSet os, string consoleName, HelpInfo helpInfo)
        {
            ConsoleName = consoleName;
            _optionSet = os;
            Requirements = new List<Requirement>();
            _helpInfo = helpInfo;
        }
        #endregion

        public Func<List<Option>, string> OnMessageForMissingVariables { get; set; }
        public Func<Exception, string> OnExceptionMessage { get; set; }
        public List<Requirement> Requirements { get; private set; }

        readonly HelpInfo _helpInfo;

        public string[] HelpMessage { get; set; }


        public string ConsoleName { get; protected set; }
        readonly RequiredValuesOptionSet _optionSet;

        public bool PerformCanProceed(TextWriter tw, string[] args)
        {
            var success = true;
            var output = new List<string>();
            var helpMode = false;
            try
            {
                _optionSet.Parse(args);
                helpMode = _helpInfo.HelpMode();
                if (helpMode)
                {
                    //add text if specified
                    if (_helpInfo.Text.Any())
                        output.AddRange(_helpInfo.Text);

                    // instuctions
                    output.Add(String.Format("Usage: {0} [OPTIONS]+", ConsoleName));
                    output.Add(String.Empty);
                    output.Add("Options:");

                    //never proceed if in help mode
                    success = false;
                }
                else //Proceed:
                {

                    // check requirements
                    var requirements = (from t in Requirements
                                        where t.RequirementSatisfied() == false
                                        from t2 in t.Text
                                        select t2).ToList();

                    if (requirements.Any())
                    {
                        output.Add("Error: The Following requirements have not been met:");
                        output.AddRange(requirements);
                        success = false;
                    }

                    // check missing variables
                    var missingVars = _optionSet.GetMissingVariables().ToList();
                    if (missingVars.Any())
                    {
                        // use lambda
                        if (OnMessageForMissingVariables != null)
                            output.Add(OnMessageForMissingVariables(missingVars));
                        else  //use default
                        {
                            output.Add("Error: The following variables were missing:");
                            output.AddRange(missingVars.Select(a => String.Format("\t{0}   \t{1}", a.Prototype, a.Description)));
                        }
                        success = false;
                    }
                }

            }
            // catch exceptions
            catch (Exception ex)
            {
                // use lambda
                output.Add(OnExceptionMessage == null ? ex.Message : OnExceptionMessage(ex));
                success = false;
            }

            // WriteOutput
            if (output.Any())
            {
                // if we're not in output mode, put the console name up
                if (!helpMode)
                    tw.WriteLine(ConsoleName + ":");


                output.ForEach(tw.WriteLine);

                if (helpMode)
                    _optionSet.WriteOptionDescriptions(tw);
                else
                    if (!success)
                        tw.WriteLine(String.Format("Try {0}  {1} for more information", ConsoleName, _helpInfo.UserFlag));


            }

            return success;
        }




    }
}
