using System;

namespace GeniusCode.Components.Console
{
    public class HelpInfo
    {
        public HelpInfo(string userFlag, Func<bool> helpMode, params string[] text)
        {
            UserFlag = userFlag;
            HelpMode = helpMode;
            Text = text;
        }
        public string[] Text { get; set; }
        public string UserFlag { get; set; }
        public Func<bool> HelpMode { get; set; }

    }
}