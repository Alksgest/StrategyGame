using UnityEngine;

namespace StrategyGame.Assets.Scripts.Unit.Interfaces
{
    public interface IWorkable
    {
        GameObject GameObject { get; }
        GameObject ObjectAttachedTo { get; set; }
        void SetTag(string tag);
    }
}