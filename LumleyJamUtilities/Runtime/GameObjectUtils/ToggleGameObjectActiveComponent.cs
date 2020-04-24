using UnityEngine;

namespace Lumley.GameObjectUtils
{
    public class ToggleGameObjectActiveComponent : MonoBehaviour
    {
        [SerializeField] private GameObject _target;
        
        public void Toggle()
        {
            if (_target != null)
            {
                _target.SetActive(!_target.activeSelf);
            }
        }
    }
}