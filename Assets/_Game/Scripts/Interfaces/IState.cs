namespace _Game.Scripts.Interfaces
{
    public interface IState
    {
        public void OnEnter(StateController controller);
        public void UpdateState(StateController controller);
        public void OnHurt(StateController controller);
        public void OnExit(StateController controller);
    }
}