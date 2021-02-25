using Assets.Scripts.Behaviour;
using Assets.Scripts.Behaviour.Common;
using Assets.Scripts.Commands.Interfaces;

namespace Assets.Scripts.Commands
{
    public class DeselectCommand<T> : ICommand<T> where T : ISelectable
    {
        public bool Interrupt { get; protected set; } = false;

        public bool Execute(T obj)
        {
            return obj.Deselect();
        }
    }
}
