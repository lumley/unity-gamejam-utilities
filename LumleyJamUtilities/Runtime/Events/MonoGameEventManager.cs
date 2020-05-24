using System;
using UnityEngine;

namespace Lumley.Events
{
    public class MonoGameEventManager : MonoBehaviour, IGameEventManager
    {
        private readonly GameEventManager _eventManager = new GameEventManager();
        
        public void AddListener(Action<GameEvent> action, GameEvent @event)
        {
            _eventManager.AddListener(action, @event);
        }

        public void RemoveListener(Action<GameEvent> action, GameEvent @event)
        {
            _eventManager.RemoveListener(action, @event);
        }

        public void SendEvent(GameEvent @event)
        {
            _eventManager.SendEvent(@event);
        }
    }
}