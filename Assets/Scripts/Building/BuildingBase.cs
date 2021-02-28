using Assets.Scripts.Behaviour.Building;
using Assets.Scripts.Commands.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Building
{
    public abstract class BuildingBase : MonoBehaviour, ICommandExecutor<BuildingBase>, IBuilding
    {
        [SerializeField]
        protected bool IsInstantiated = false;

        [SerializeField]
        protected GameObject Ui;

        public string Owner { get; protected set; } = "mainPlayer";
        public bool CanBePlaced { get; protected set; } = true;
        public bool Selected { get; protected set; } = false;
        public float BuildingProgress => 100f;
        public Vector3 Destination => this.transform.position;

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

        public virtual bool Select()
        {
            SetUiActive();
            Selected = true;
            return true;
        }

        public virtual bool Deselect()
        {
            SetUiInactive();
            Selected = false;
            return true;
        }

        public void SetUiActive()
        {
            Ui.SetActive(true);
        }

        public void SetUiInactive()
        {
            Ui.SetActive(false);
        }

        public bool Delete()
        {
            Destroy(gameObject);
            return true;
        }
    }
}