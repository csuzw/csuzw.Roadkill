
namespace csuzw.Roadkill.TagTreeMenu.StateMachine
{
    internal class State
    {
        public StateType Type { get; set; }
        public int Depth { get; set; }

        public TagTree TagTree { get; set; }

        public TagKey InProgressKey { get; set; }
        public TagTreeStateMachine InProgressValue { get; set; }

        public State(StateType type, TagTree tagTree = null)
        {
            Type = type;
            TagTree = tagTree ?? new TagTree();
        }

        public void AddInProgressToTree()
        {
            if (InProgressKey != null) TagTree.Add(InProgressKey, (InProgressValue != null) ? InProgressValue.TagTree : null);
            InProgressKey = null;
            InProgressValue = null;
        }
    }
}
