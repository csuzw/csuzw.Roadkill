using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Roadkill.Core.Plugins;

namespace Roadkill.GitHubExtensions
{
    public class GitHubExtensions : TextPlugin
    {
        private static readonly Regex TableRegex = new Regex(@"
            \| (?: (.*?) \|)+\s*$\n
            ^\| (?: (\:?\-*\:?) \|)+\s*$
            (?: \n ^\| ((?: (.*?) \|)+)* )*",
            RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);

        private static readonly Regex CodeBlockRegex = new Regex(@"
            (?<!\\)
            (`{3})
            (\w*)
            (
			(?:
				.*\n+
			)+?
			)
            \1
            (?!`)", 
            RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);

        private static readonly Regex StrikethroughRegex = new Regex(@"(~{2}) (?=\S) (.+?) (?<=\S) \1",
            RegexOptions.IgnorePatternWhitespace | RegexOptions.Singleline | RegexOptions.Compiled);

        public override string Id
        {
            get { return "GitHubExtensions"; }
        }

        public override string Name
        {
            get { return "GitHub Extensions"; }
        }

        public override string Description
        {
            get { return "Adds GitHub Markdown extensions, including Tables, Code Blocks and Strikethroughs"; }
        }

        public override string Version
        {
            get { return "1.0"; }
        }

        public override string BeforeParse(string markupText)
        {
            markupText = CodeBlockRegex.Replace(markupText, CodeBlockEvaluator);
            markupText = StrikethroughRegex.Replace(markupText, StrikethroughEvaluator);

            return markupText;
        }

        public override string AfterParse(string html)
        {
            html = TableRegex.Replace(html, TableEvaluator);

            return html;
        }

        private string TableEvaluator(Match match)
        {
            var columnCount = match.Groups[1].Captures.Count;
            var columnAlignments = match.Groups[2].Captures.Cast<Capture>().Select(c => GetTableColumnAlignment(c.Value)).ToList();

            var tableHeader = GetTableHeader(match.Groups[1].Captures.Cast<Capture>().Select(c => c.Value), columnAlignments);
            var tableRows = string.Join("\n", match.Groups[3].Captures.Cast<Capture>().Select(c => GetTableRow(c.Value, columnAlignments, columnCount)));

            return string.Concat("<table>\n<thead>\n", tableHeader, "</thead>\n<tbody>\n", tableRows, "</tbody>\n</table>\n");
        }

        private string CodeBlockEvaluator(Match match)
        {
            string language = match.Groups[2].Value;
            if (string.IsNullOrWhiteSpace(language))
            {
                language = "plain";
            }

            string codeBlock = HttpUtility.HtmlEncode(match.Groups[3].Value);

            return string.Concat("\n\n<pre class=\"brush: ", language, "\">", codeBlock, "</pre>\n\n");
        }

        private string StrikethroughEvaluator(Match match)
        {
            return string.Concat("<s>", match.Groups[2].Value, "</s>");
        }

        private string GetTableColumnAlignment(string value)
        {
            var left = value.StartsWith(":");
            var right = value.EndsWith(":");
            if (left && right) return " align=\"center\"";
            if (left) return " align=\"left\"";
            if (right) return " align=\"right\"";
            return "";
        }

        private string GetTableHeader(IEnumerable<string> values, List<string> columnAlignments)
        {
            var cells = string.Join("", values.Select((v, i) => GetTableCell(v, columnAlignments.ElementAtOrDefault(i), true)));

            return string.Concat("<tr>\n", cells, "</tr>\n");
        }

        private string GetTableRow(string value, List<string> columnAlignments, int columnCount)
        {
            var split = value.Split('|');
            var values = new List<string>();
            for (int i = 0; i < columnCount; i++)
            {
                values.Add(i < split.Length ? split[i] : "");
            }

            var cells = string.Join("", values.Select((v, i) => GetTableCell(v, columnAlignments.ElementAtOrDefault(i))));

            return string.Concat("<tr>\n", cells, "</tr>\n");
        }

        private string GetTableCell(string value, string columnAlignment = "", bool isHeader = false)
        {
            var element = (isHeader) ? "th" : "td";

            return string.Concat("<", element, columnAlignment, ">", value.Trim(), "</", element, ">\n");
        }
    }
}
