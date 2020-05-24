using System;
using System.Collections.Generic;

namespace Lumley.Events
{
    public class GameEventManager : IGameEventManager
    {
        private readonly Dictionary<GameEvent, List<Action<GameEvent>>> _gameEventToListenerMap = new Dictionary<GameEvent, List<Action<GameEvent>>>();
        private readonly HashSet<int> _runningEventSet = new HashSet<int>();
        
        public void AddListener(Action<GameEvent> action, GameEvent @event)
        {
            if (!_gameEventToListenerMap.TryGetValue(@event, out List<Action<GameEvent>> list))
            {
                list = new List<Action<GameEvent>>();
                _gameEventToListenerMap[@event] = list;
            }
            list.Add(action);
        }

        public void RemoveListener(Action<GameEvent> action, GameEvent @event)
        {
            if (_gameEventToListenerMap.TryGetValue(@event, out List<Action<GameEvent>> list))
            {
                var instanceId = @event.GetInstanceID();
                // Safely remove this listener in case it was the one running right now. This saves the case in which several listeners are removed at once as a result of sending a specific event.
                if (_runningEventSet.Contains(instanceId))
                {
                    var actions = new List<Action<GameEvent>>(list);
                    if (actions.Remove(action))
                    {
                        _gameEventToListenerMap[@event] = actions;
                    }
                }
                else
                {
                    list.Remove(action);
                }
                
            }
        }
        
        public void SendEvent(GameEvent @event)
        {
            if (_gameEventToListenerMap.TryGetValue(@event, out List<Action<GameEvent>> list))
            {
                var instanceId = @event.GetInstanceID();
                bool isFirstRun = _runningEventSet.Add(instanceId);
                try
                {
                    foreach (var action in list)
                    {
                        action.Invoke(@event);
                    }
                }
                finally
                {
                    if (isFirstRun)
                    {
                        _runningEventSet.Remove(instanceId);
                    }
                }
            }
        }
    }
}