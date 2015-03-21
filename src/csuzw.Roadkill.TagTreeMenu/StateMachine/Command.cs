
namespace csuzw.Roadkill.TagTreeMenu.StateMachine
{
    internal class Command
    {
        public CommandType Type { get; private set; }
        public string Tag { get; private set; }

        public Command(CommandType type, string tag = "")
        {
            Type = type;
            Tag = tag;
        }
    }
}
