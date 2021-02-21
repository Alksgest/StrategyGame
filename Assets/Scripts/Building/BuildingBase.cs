using Assets.Scripts.Behaviour.Common;
using Assets.Scripts.Commands.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Building
{
    public abstract class BuildingBase : MonoBehaviour, ICommandExecutor<BuildingBase>, ISelectable
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

        public virtual void Execute(ICommand<BuildingBase> command)
        {
            command.Execute(this);
        }

        public virtual void Select()
        {
            SetUiActive();
            Selected = true;
        }

        public virtual void Deselect()
        {
            SetUiInactive();
            Selected = false;
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }

        public void SetUiActive()
        {
            Ui.SetActive(true);
        }

        public void SetUiInactive()
        {
            Ui.SetActive(false);
        }
    }
}