using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace csuzw.Roadkill.TagTreeMenu.StateMachine
{
    internal static class Extensions
    {
        private static readonly Regex _tokenizer = new Regex(@"([\w]+)|(\|[\w]+)|(&[\w]+)|(![\w]+)|(,[\w]+)|(\()|(\))", RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);

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

        public static IEnumerable<string> ToTokens(this string definition)
        {
            var tokens = _tokenizer
                .Split(definition)
                .Where(t => !string.IsNullOrWhiteSpace(t));

            return tokens;
        }

        public static IEnumerable<Command> ToCommands(this IEnumerable<string> tokens)
        {
            var commands = tokens.Select(t => t.ToCommand());

            return commands;
        }

        public static IEnumerable<Command> ToCommands(this string definition)
        {
            var commands = definition
                .ToTokens()
                .Select(t => t.ToCommand());

            return commands;
        }

        public static Command ToCommand(this string token)
        {
            return new Command(token);
        }
    }
}
