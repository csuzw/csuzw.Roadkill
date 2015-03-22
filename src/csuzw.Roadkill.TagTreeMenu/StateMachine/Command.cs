
namespace csuzw.Roadkill.TagTreeMenu.StateMachine
{
    internal class Command
    {
        public CommandType Type { get; private set; }
        public string Tag { get; private set; }

        public Command(string token)
        {
            var command = GetCommandFromToken(token);
            Type = command.Type;
            Tag = command.Tag;
        }

        public Command(CommandType type, string tag = "")
        {
            Type = type;
            Tag = tag;
        }

        private Command GetCommandFromToken(string token)
        {
            if (token == @"(") return new Command(CommandType.Up);
            if (token == @")") return new Command(CommandType.Down);
            if (token.StartsWith(",")) return new Command(CommandType.TagNext, token.Substring(1));
            if (token.StartsWith("!")) return new Command(CommandType.TagNull, token.Substring(1));
            if (token.StartsWith("|")) return new Command(CommandType.TagOr, token.Substring(1));
            if (token.StartsWith("&")) return new Command(CommandType.TagAnd, token.Substring(1));
            return new Command(CommandType.TagNone, token);
        }
    }
}
