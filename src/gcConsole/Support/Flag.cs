namespace GeniusCode.Components.Console.Support
{
    public class Flag
    {
        public static implicit operator bool(Flag input)
        {
            return input.On;
        }
        public bool On { get; internal set; }
    }
}
