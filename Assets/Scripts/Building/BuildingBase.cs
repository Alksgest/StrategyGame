using UnityEngine;

namespace Assets.Scripts.Building
{
    public abstract class BuildingBase : MonoBehaviour
    {
        [SerializeField]
        protected bool IsInstantiated = false;

        [SerializeField]
        protected GameObject Ui;

        public string Owner { get; protected set; } = "mainPlayer";

        public bool CanBePlaced { get; protected set; } = true;

        public bool Selected { get; protected set; } = false;

        public virtual void Instantiate()
        {
            if (!IsInstantiated)
            {
                IsInstantiated = true;
            }
            else
            {
                Debug.LogError("Smth went wrong. Trying to instantiate already instantiated object");
            }
        }

        public virtual void LeftClick(object obj)
        {
            Select();
        }

        public virtual void Select()
        {
            SetUIActive();
            Selected = true;
        }

        public virtual void Deselect()
        {
            SetUIInactive();
            Selected = false;
        }

        public virtual void RightClick(object obj)
        {

        }

        public void Destroy()
        {
            Destroy(gameObject);
        }

        public void SetUIActive()
        {
            Ui.SetActive(true);
        }

        public void SetUIInactive()
        {
            Ui.SetActive(false);
        }

        private void OnCollisionEnter(Collision other)
        {
            if (IsInstantiated)
            {
                CanBePlaced = false;
            }
        }

        private void OnCollisionExit(Collision other)
        {
            if (IsInstantiated)
            {
                CanBePlaced = true;
            }
        }
    }
}