using csuzw.Roadkill.Core;
using Newtonsoft.Json;
using Roadkill.Core.Mvc.ViewModels;
using Roadkill.Core.Plugins;
using Roadkill.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace csuzw.Roadkill.TagTreeMenu
{
    public class TagTreeMenu : TextPlugin, ITextPlugin, IHaveSampleInput
    {
        private static readonly Regex _regex = new Regex(@"\[\[\[ttm=(?'inner'.*?)\]\]\]", RegexOptions.Singleline | RegexOptions.Compiled);

        private readonly IPageService _pageService;

        #region Properties

        public override string Id
        {
            get { return "TagTreeMenu"; }
        }

        public override string Name
        {
            get { return "Tag Tree Menu"; }
        }

        public override string Description
        {
            get { return "Creates simple menu pages based on provided tag tree.  Usage: [[[ttm={ your json here }]]]"; }
        }

        public override string Version
        {
            get { return "1.0"; }
        }

        public string SampleInput
        {
            get { return "[[[ttm={\"One\":{\"Two\":{\"Four\":{}},\"Three\":{\"Five\":{},\"Six\":{}}}}]]]"; }
        }

        #endregion

        public TagTreeMenu(IPageService pageService)
        {
            _pageService = pageService;
        }

        public override string BeforeParse(string markupText)
        {
            markupText = _regex.Replace(markupText, GetMenuText);

            return markupText;
        }

        private string GetMenuText(Match match)
        {
            string error;
            var tagTree = GetTagTree(match, out error);
            if (tagTree == null) return error;

            var menuText = GetMenuText(tagTree);

            return menuText;
        }

        private string GetMenuText(TagTree tagTree, ICollection<PageViewModel> pages = null, int depth = 0)
        {
            string indent = new String(' ', depth * 4);
            depth += 1;
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var tag in tagTree)
            {
                var matchingPages = GetMatchingPages(tag.Key, pages);
                string childText = (tag.Value != null) ? GetMenuText(tag.Value, matchingPages, depth) : "";
                stringBuilder.AppendFormat(@"{0}* **{1}**", indent, tag.Key).AppendLine();
                foreach (var matchingPage in matchingPages)
                {
                    stringBuilder.AppendFormat(@"{0}  * [{1}]({2})", indent, matchingPage.Title, matchingPage.EncodedTitle).AppendLine();
                }
                if (!string.IsNullOrWhiteSpace(childText)) stringBuilder.AppendLine(childText);
            }
            return stringBuilder.ToString();
        }

        private ICollection<PageViewModel> GetMatchingPages(string tag, ICollection<PageViewModel> pages = null)
        {
            if (pages == null) return _pageService.FindByTag(tag).OrderBy(p => p.Title).ToList();

            return pages.Where(p => p.Tags.Any(t => t.ToLower() == tag.ToLower())).ToList();
        }

        private TagTree GetTagTree(Match match, out string error)
        {
            error = string.Empty;
            try
            {
                return JsonConvert.DeserializeObject<TagTree>(match.Groups["inner"].Value);
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return null;
            }
        }

        private class TagTree : Dictionary<string, TagTree>
        {
            public TagTree() {}

            public TagTree(params string[] tags) : this()
            {
                foreach (var tag in tags)
                {
                    Add(tag, null);
                }
            }
        }
    }
}
