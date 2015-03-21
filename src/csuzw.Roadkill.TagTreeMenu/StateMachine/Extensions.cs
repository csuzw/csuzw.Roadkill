using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace csuzw.Roadkill.TagTreeMenu.StateMachine
{
    internal static class Extensions
    {
        private static readonly Regex _tokenizer = new Regex(@"([\w]+)|(\|[\w]+)|(&[\w]+)|(,[\w]+)|(\()|(\))", RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);

        public static TagTree ToTagTree(this string definition)
        {
            var stateMachine = new TagTreeStateMachine();
            var commands = definition.ToCommands();
            foreach (var command in commands)
            {
                stateMachine.ProcessCommand(command);
            }
            return stateMachine.TagTree;
        }

        public static IEnumerable<Command> ToCommands(this string definition)
        {
            var tokens = _tokenizer
                .Split(definition)
                .Where(t => !string.IsNullOrWhiteSpace(t));

            var commands = tokens
                .Select(t => t.ToCommand());

            return commands;
        }

        public static Command ToCommand(this string token)
        {
            if (token == @"(") return new Command(CommandType.Up);
            if (token == @")") return new Command(CommandType.Down);
            if (token.StartsWith(",")) return new Command(CommandType.TagNext, token.Substring(1));
            if (token.StartsWith("|")) return new Command(CommandType.TagOr, token.Substring(1));
            if (token.StartsWith("&")) return new Command(CommandType.TagAnd, token.Substring(1));
            return new Command(CommandType.TagNone, token);
        }
    }
}
