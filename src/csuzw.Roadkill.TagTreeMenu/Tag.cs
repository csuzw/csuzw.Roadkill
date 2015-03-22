
namespace csuzw.Roadkill.TagTreeMenu
{
    internal enum TagOperator
    {
        None,
        And,
        Or,
        Null
    }

    internal class Tag
    {
        public string Name { get; private set; }
        public TagOperator Operator { get; private set; }

        public string Description
        {
            get { return Name; }
        }

        public Tag(string name, TagOperator op = TagOperator.None)
        {
            Name = name;
            Operator = op;
        }
    }
}
