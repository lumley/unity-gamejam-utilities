using System;

namespace Lumley.Events
{
    public interface IGameEventManager
    {
        void AddListener(Action<GameEvent> action, GameEvent @event);

        void RemoveListener(Action<GameEvent> action, GameEvent @event);

        void SendEvent(GameEvent @event);
    }
}