
namespace csuzw.Roadkill.TagTreeMenu.StateMachine
{
    internal struct Transition
    {
        public StateType State;
        public CommandType Command;

        public Transition(StateType state, CommandType command)
        {
            State = state;
            Command = command;
        }
    }
}
