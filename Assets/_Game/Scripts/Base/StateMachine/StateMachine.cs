using System;

namespace _Game.Scripts.Base.StateMachine
{
    public class StateMachine
    {
        public IStateCanvas CurrentStateCanvas { get; private set; }

        public virtual void ChangeState(IStateCanvas stateCanvas)
        {
            if (stateCanvas == null)
                throw new ArgumentNullException(nameof(stateCanvas));

            if (CurrentStateCanvas != null)
                CurrentStateCanvas.Exit();

            CurrentStateCanvas = stateCanvas;
            CurrentStateCanvas.Enter();
        }
    }
}