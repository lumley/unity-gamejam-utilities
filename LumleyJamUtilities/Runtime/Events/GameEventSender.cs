using UnityEngine;

namespace Lumley.Events
{
    public class GameEventSender : MonoBehaviour
    {
        [SerializeField] private GameEvent _eventToSend;
        [SerializeField] public bool _sendOnEnable;

        private void OnEnable()
        {
            if (_sendOnEnable)
            {
                SendEvent();
            }
        }

        public void SendEvent()
        {
            var gameEventManager = Toolbox.Toolbox.Get<IGameEventManager>();
            gameEventManager.SendEvent(_eventToSend);
        }
    }
}