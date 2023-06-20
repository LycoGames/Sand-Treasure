namespace _Game.Scripts.Interfaces
{
    public interface IMover
    {
        void Movement();
        bool HasInput();
        void IncreaseMovementSpeed(bool isIncrease, bool isMinSpeed);
        void IsStopped(bool isStopped);
    }
}