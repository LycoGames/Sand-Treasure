using _Game.Scripts.Enums;

namespace _Game.Scripts.Observer
{
    public interface IObserver
    {
        public void OnNotify(int value,ItemType type);
    }
}
