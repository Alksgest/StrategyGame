using Assets.Scripts.Behaviour.Common;
using Assets.Scripts.Commands.Interfaces;

namespace Assets.Scripts.Commands
{
    public class DeleteCommand<T> : ICommand<T> where T : IDeletable
    {
        public void Execute(T obj)
        {
            obj.Delete();
        }
    }
}
