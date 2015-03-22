using System;
using System.Collections.Generic;

namespace csuzw.Roadkill.TagTreeMenu.StateMachine
{
    internal class TagTreeStateMachine
    {
        private readonly Dictionary<Transition, Action<Command>> _transitions;
        private bool _isFinalized;

        private State _currentState = new State(StateType.Start);

        public TagTree TagTree
        {
            get
            {
                FinalizeTagTree(); // requires because tags don't need to be terminated
                return _currentState.TagTree;
            }
        }

        public TagTreeStateMachine()
        {
            _transitions = new Dictionary<Transition, Action<Command>>();
            _transitions.Add(new Transition(StateType.Start, CommandType.TagNone), TransitionTagNone);
            _transitions.Add(new Transition(StateType.KeyInProgress, CommandType.Up), TransitionUp);
            _transitions.Add(new Transition(StateType.KeyInProgress, CommandType.Down), TransitionDown);
            _transitions.Add(new Transition(StateType.KeyInProgress, CommandType.TagOr), TransitionTagOr);
            _transitions.Add(new Transition(StateType.KeyInProgress, CommandType.TagAnd), TransitionTagAnd);
            _transitions.Add(new Transition(StateType.KeyInProgress, CommandType.TagNull), TransitionTagNull);
            _transitions.Add(new Transition(StateType.KeyInProgress, CommandType.TagNext), TransitionKeyInProgressTagNext);
            _transitions.Add(new Transition(StateType.End, CommandType.TagNext), TransitionEndTagNext);
            _transitions.Add(new Transition(StateType.End, CommandType.Down), TransitionDown);
        }

        public void ProcessCommand(Command command)
        {
            if (_isFinalized) throw new Exception("Finalized state machine cannot process commands");
            var transition = new Transition(_currentState.Type, command.Type);
            if (!_transitions.ContainsKey(transition)) throw new Exception(string.Format("Cannot transition from state '{0}' using command '{1}'", transition.State, transition.Command));

            _transitions[transition](command);
        }

        private void FinalizeTagTree()
        {
            if (_isFinalized) return;
            if (_currentState.Depth > 0) throw new Exception("TagTree is not in finalizable state");
            _currentState.AddInProgressToTree();
            _isFinalized = true;
        }

        private void TransitionTagNone(Command command)
        {
            DoAction(
                command,
                normal: (c) => _currentState.InProgressKey = new TagKey(new Tag(c.Tag)),
                depth: (c) => _currentState.InProgressValue.ProcessCommand(c),
                post: (c) => _currentState.Type = StateType.KeyInProgress
                );
        }

        private void TransitionUp(Command command)
        {
            DoAction(
                command,
                normal: (c) => _currentState.InProgressValue = new TagTreeStateMachine(),
                depth: (c) => _currentState.InProgressValue.ProcessCommand(c),
                post: (c) => { _currentState.Depth += 1; _currentState.Type = StateType.Start; },
                requiresTag: false
                );
        }

        private void TransitionDown(Command command)
        {
            DoAction(
                command,
                normal: (c) => _currentState.AddInProgressToTree(),
                depth: (c) => _currentState.InProgressValue.ProcessCommand(c),
                post: (c) => { _currentState.Depth -= 1; _currentState.Type = StateType.End; },
                depthLimit: 1,
                requiresTag: false
                );
        }

        private void TransitionTagAnd(Command command)
        {
            TransitionTagOperator(command, TagOperator.And);
        }

        private void TransitionTagOr(Command command)
        {
            TransitionTagOperator(command, TagOperator.Or);
        }

        private void TransitionTagNull(Command command)
        {
            TransitionTagOperator(command, TagOperator.Null);
        }

        private void TransitionTagOperator(Command command, TagOperator tagOperator)
        {
            DoAction(
                command,
                normal: (c) => _currentState.InProgressKey.Tags.Add(new Tag(c.Tag, tagOperator)),
                depth: (c) => _currentState.InProgressValue.ProcessCommand(c),
                post: (c) => _currentState.Type = StateType.KeyInProgress
                );
        }

        private void TransitionKeyInProgressTagNext(Command command)
        {
            DoAction(
                command,
                normal: (c) => {
                    _currentState.AddInProgressToTree();
                    _currentState.InProgressKey = new TagKey(new Tag(c.Tag)); 
                },
                depth: (c) => _currentState.InProgressValue.ProcessCommand(c),
                post: (c) => _currentState.Type = StateType.KeyInProgress
                );
        }

        private void TransitionEndTagNext(Command command)
        {
            DoAction(
                command,
                normal: (c) => _currentState.InProgressKey = new TagKey(new Tag(c.Tag)),
                depth: (c) => _currentState.InProgressValue.ProcessCommand(c),
                post: (c) => _currentState.Type = StateType.KeyInProgress
                );
        }

        private void DoAction(Command command, Action<Command> pre = null, Action<Command> normal = null, Action<Command> depth = null, Action<Command> post = null, int depthLimit = 0, bool requiresTag = true)
        {
            if (requiresTag && string.IsNullOrWhiteSpace(command.Tag)) throw new Exception(string.Format("Command '{0}' requires Tag", command.Type)); 
            if (pre != null) pre(command);
            if (_currentState.Depth > depthLimit)
            {
                if (depth != null) depth(command);
            }
            else if (_currentState.Depth == depthLimit)
            {
                if (normal != null) normal(command);
            }
            else
            {
                throw new Exception(string.Format("Unexpected command '{0}'", command.Type));
            }
            if (post != null) post(command);
        }
    }
}
