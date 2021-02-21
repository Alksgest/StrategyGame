using UnityEngine;

namespace Assets.Scripts.Behaviour.Unit
{
    public interface IAttachable
    {
        void Attach(GameObject obj);
        void Detach();
    }
}
