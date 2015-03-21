using csuzw.Roadkill.Core;
using csuzw.Roadkill.TagTreeMenu.StateMachine;
using Roadkill.Core.Database;
using Roadkill.Core.Plugins;
using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace csuzw.Roadkill.TagTreeMenu
{
    public class TagTreeMenu : TextPlugin, ITextPlugin, IHaveSampleInput
    {
        private static readonly Regex _regex = new Regex(@"\[\[\[ttm=(?'inner'.*?)\]\]\]", RegexOptions.Singleline | RegexOptions.Compiled);

        private readonly IRepository _repository;

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
            get { return "Creates simple menu pages based on provided tag tree.  Usage: [[[ttm=Tag1(Tag2&Tag4(Tag3),Tag4(Tag5|Tag6))]]]"; }
        }

        public override string Version
        {
            get { return "1.0"; }
        }

        public string SampleInput
        {
            get { return "[[[ttm=One(Two(Four),Three(Five|Six))]]]"; }
        }

        #endregion

        public TagTreeMenu(IRepository repository)
        {
            _repository = repository;
        }

        public override string BeforeParse(string markupText)
        {
            var pageProvider = new PageMetaDataProvider(_repository);
            markupText = _regex.Replace(markupText, m => GetMenuText(m, pageProvider));

            return markupText;
        }

        private string GetMenuText(Match match, PageMetaDataProvider pageProvider)
        {
            TagTree tagTree;
            try
            {
                tagTree = match.Groups["inner"].Value.ToTagTree();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            var createMenuHeaders = tagTree.Keys.Count > 1;
            var stringBuilder = new StringBuilder();
            foreach (var root in tagTree)
            {
                var menuText = GetMenuText(pageProvider, root.Value, 0, root.Key);
                if (createMenuHeaders) stringBuilder.AppendFormat("## {0}", root.Key.Description).AppendLine();
                stringBuilder.AppendLine(menuText);
            }

            return stringBuilder.ToString();
        }

        private string GetMenuText(PageMetaDataProvider pageProvider, TagTree tagTree, int depth = 0, params TagKey[] rootTags)
        {
            string indent = new String(' ', depth * 4);
            depth += 1;

            StringBuilder stringBuilder = new StringBuilder();

            var pages = pageProvider.GetPages(rootTags);
            if (pages != null)
            {
                foreach (var page in pages)
                {
                    stringBuilder.AppendFormat(@"{0}* [{1}]({2})", indent, page.Name, page.EncodedName).AppendLine();
                }
            }
            if (tagTree == null) return stringBuilder.ToString();
            foreach (var tag in tagTree)
            {
                stringBuilder.AppendFormat(@"{0}    * **{1}**", indent, tag.Key.Description).AppendLine();
                stringBuilder.Append(GetMenuText(pageProvider, tag.Value, depth, rootTags.Concat(new [] { tag.Key }).ToArray()));
            }

            return stringBuilder.ToString();
        }
    }
}
