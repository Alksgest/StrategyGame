using System;
using Assets.Scripts.Behaviour.Building;
using Assets.Scripts.Behaviour.Unit;
using Assets.Scripts.Commands.Interfaces;

namespace Assets.Scripts.Commands
{
    public class BuildCommand<T>: IRejectableCommand<T> // where T : IBuildingBuilder
    {
        public bool Interrupt { get; protected set; }

        private readonly IBuildable _building;

        public BuildCommand(IBuildable building, bool interrupt = true)
        {
            _building = building;
            Interrupt = interrupt;
        }


        public bool Execute(T obj)
        {
            if (obj is IBuildingBuilder builder)
            {
                return builder.Build(_building);
            }

            return true;
        }

        public void Reject(T source)
        {
            if (source is IBuildingBuilder builder)
            {
                builder.DetachFromBuilding();
            }
        }
    }
}
