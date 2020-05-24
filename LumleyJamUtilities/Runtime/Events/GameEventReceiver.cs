using UnityEngine;
using UnityEngine.Events;

namespace Lumley.Events
{
    public class GameEventReceiver : MonoBehaviour
    {
        [SerializeField] private GameEvent _eventToReceive;
        public UnityEvent OnEventReceived;

        private void OnEnable()
        {
            var gameEventManager = Toolbox.Toolbox.Get<IGameEventManager>();
            gameEventManager.AddListener(OnGameEventReceived, _eventToReceive);
        }

        private void OnDisable()
        {
            var gameEventManager = Toolbox.Toolbox.Get<IGameEventManager>();
            gameEventManager.RemoveListener(OnGameEventReceived, _eventToReceive);
        }
        
        private void OnGameEventReceived(GameEvent @event)
        {
            OnEventReceived.Invoke();
        }
    }
}