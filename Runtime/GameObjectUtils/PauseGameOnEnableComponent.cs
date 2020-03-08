using UnityEngine;
using UnityEngine.Serialization;

namespace Lumley.GameObjectUtils
{
    public class PauseGameOnEnableComponent : MonoBehaviour
    {
        [FormerlySerializedAs("_timeScale")] [SerializeField] private float _valueWhenDisabled = 1f;
        [SerializeField] private float _valueWhenEnabled = 0f;
        
        private void OnEnable()
        {
            Time.timeScale = _valueWhenEnabled;
        }

        private void OnDisable()
        {
            Time.timeScale = _valueWhenDisabled;
        }
    }
}