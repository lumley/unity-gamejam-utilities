using System;
using System.Collections.Generic;
using UnityEngine;

namespace Lumley.Toolbox
{
    public class Toolbox : MonoBehaviour
    {
        private static Toolbox _instance;

        [SerializeField] private bool _shouldNotDestroyOnLoad = true;
        private readonly List<object> _allObjects = new List<object>();
        private readonly Dictionary<Type, object> _cachedObjectMap = new Dictionary<Type, object>();
        private bool _isInitialized;

        private void Awake()
        {
            TryInitialize();
        }

        private void TryInitialize()
        {
            if (_isInitialized)
            {
                return;
            }

            _isInitialized = true;
            
            if (_instance != null && _instance != this)
            {
                Destroy(this);
                return;
            }

            _instance = this;
            if (_shouldNotDestroyOnLoad)
            {
                DontDestroyOnLoad(this);
            }

            var currentTransform = gameObject.transform;
            for (int i = 0; i < currentTransform.childCount; i++)
            {
                var child = currentTransform.GetChild(i);
                Add(child);
            }
            
        }

        public static void Add(object accessibleObject)
        {
            _instance.AddInternal(accessibleObject);
        }
        
        public static T Get<T>() where T : class
        {
            if (_instance == null)
            {
                var toolboxInstance = FindObjectOfType<Toolbox>();
                if (toolboxInstance == null)
                {
                    return null;
                }
                toolboxInstance.TryInitialize();
            }
            return _instance.GetInternal<T>();
        }

        private void AddInternal(object accessibleObject)
        {
            if (accessibleObject is MonoBehaviour monoBehaviour)
            {
                monoBehaviour.transform.SetParent(transform, false);
            }
            _allObjects.Add(accessibleObject);
        }

        private T GetInternal<T>() where T : class
        {
            var type = typeof(T);
            if (_cachedObjectMap.TryGetValue(type, out object value))
            {
                return (T) value;
            }

            foreach (object element in _allObjects)
            {
                if (element is T tElement)
                {
                    _cachedObjectMap[type] = tElement;
                    return tElement;
                }
                if (element is Component component)
                {
                    var matchingComponent = component.GetComponent<T>();
                    if (matchingComponent != null)
                    {
                        _cachedObjectMap[type] = matchingComponent;
                        return matchingComponent;
                    }
                }
            }

            return null;
        }
    }
}