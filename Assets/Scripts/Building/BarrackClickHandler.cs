// using UnityEngine;
// using StrategyGame.Assets.Scripts.Util;

// namespace StrategyGame.Assets.Scripts.Building
// {
//     public class BarrackClickHandler : MonoBehaviour
//     {
//         [SerializeField]
//         private GameObject _barrackUI;

//         private void Awake()
//         {
//             var gch = FindObjectOfType<GlobalClickHandler>();
//             gch.GameObjectLeftClick += OnLeftClick;
//         }

//         private void OnLeftClick(RaycastHit hit)
//         {
//             _barrackUI.SetActive(false);

//             if (hit.transform.parent.tag == this.tag && hit.transform.parent.gameObject == this.gameObject)
//             {
//                 _barrackUI.SetActive(!_barrackUI.activeSelf);
//             }
//         }

//         private void OnDestroy()
//         {
//             var gch = FindObjectOfType<GlobalClickHandler>();
//             if (gch != null)
//                 gch.GameObjectLeftClick -= OnLeftClick;
//         }
//     }
// }