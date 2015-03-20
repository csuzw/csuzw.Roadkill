using csuzw.Roadkill.Core;
using Newtonsoft.Json;
using Roadkill.Core.Plugins;
using Roadkill.Core.Services;
using System;
using System.Collections.Generic;
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
            get { return "[[[ttm={\"Root\":{\"Child1\":{},\"Child2\":{}}}]]]"; }
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

            return "success";
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
