
namespace csuzw.Roadkill.TagTreeMenu.StateMachine
{
    internal enum StateType
    {
        Start,
        KeyInProgress,
        End
    }

    internal enum CommandType
    {
        TagNone,
        TagAnd,
        TagOr,
        TagNull,
        TagNext,
        Up,
        Down
    }
}
