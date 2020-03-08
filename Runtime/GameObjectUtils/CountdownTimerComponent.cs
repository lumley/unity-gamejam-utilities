using System;
using UnityEngine;
using UnityEngine.Events;

namespace Lumley.GameObjectUtils
{
    public class CountdownTimerComponent : MonoBehaviour
    {
        [SerializeField] private float _currentTimeLeft;

        public TimeLeftEvent OnTimeSet;
        public TimeLeftEvent OnSecondPassed;
        public UnityEvent OnTrigger;

        private int _previousFullSecond;

        public void SetTime(float time)
        {
            _currentTimeLeft = time;
            _previousFullSecond = (int) _currentTimeLeft;
            OnTimeSet?.Invoke(_currentTimeLeft);
        }

        private void Update()
        {
            if (_currentTimeLeft <= 0)
            {
                return;
            }

            var deltaTime = Time.deltaTime;
            _currentTimeLeft -= deltaTime;

            int currentSecond = (int) _currentTimeLeft;
            if (currentSecond != _previousFullSecond)
            {
                OnSecondPassed?.Invoke(_currentTimeLeft);
                _previousFullSecond = currentSecond;
            }

            if (_currentTimeLeft <= 0)
            {
                OnTrigger?.Invoke();
            }
        }
        
        [Serializable]
        public class TimeLeftEvent : UnityEvent<float>
        {
            
        }
    }
}