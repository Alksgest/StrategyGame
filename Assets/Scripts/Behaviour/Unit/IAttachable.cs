using UnityEngine;

namespace Assets.Scripts.Behaviour.Unit
{
    public interface IAttachable
    {
        bool Attach(GameObject obj);
        void Detach();
    }
}
