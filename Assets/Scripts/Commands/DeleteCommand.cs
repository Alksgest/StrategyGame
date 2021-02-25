using Assets.Scripts.Behaviour.Common;
using Assets.Scripts.Commands.Interfaces;

namespace Assets.Scripts.Commands
{
    public class DeleteCommand<T> : ICommand<T> where T : IDeletable
    {
        public bool Interrupt { get; protected set; } = false;

        public bool Execute(T obj)
        {
            return obj.Delete();
        }
    }
}
