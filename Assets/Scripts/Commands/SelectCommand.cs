using Assets.Scripts.Behaviour;
using Assets.Scripts.Commands.Interfaces;

namespace Assets.Scripts.Commands
{
    public class SelectCommand<T> : ICommand<T> where T : ISelectable
    {
        public void Execute(T obj)
        {
            obj.Select();
        }
    }
}
