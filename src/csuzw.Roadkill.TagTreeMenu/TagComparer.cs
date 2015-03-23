using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace csuzw.Roadkill.TagTreeMenu
{
    public class TagComparer
    {
        private readonly Dictionary<string, Regex> _wildcards = new Dictionary<string, Regex>(StringComparer.CurrentCultureIgnoreCase);

        public bool IsMatch(string tag, string wildcard)
        {
            if (!_wildcards.ContainsKey(wildcard))
            {
                _wildcards.Add(wildcard, new Regex("^" + Regex.Escape(wildcard).Replace(@"\*", ".*").Replace(@"\?", ".") + "$", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled));
            }
            return _wildcards[wildcard].IsMatch(tag);
        }
    }
}
