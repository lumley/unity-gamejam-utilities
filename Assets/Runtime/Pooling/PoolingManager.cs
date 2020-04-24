using System.Collections.Generic;
using UnityEngine;

namespace Lumley.Pooling
{
    public class PoolingManager : MonoBehaviour
    {

        private readonly Dictionary<GameObject, Stack<GameObject>> _prefabToObjectMap = new Dictionary<GameObject, Stack<GameObject>>();

        public T Create<T>(T prefab, Transform onTransform = null) where T : MonoBehaviour
        {
            var instance = Create(prefab.gameObject, onTransform);
            if (instance != null)
            {
                return instance.GetComponent<T>();
            }

            return null;
        }

        public GameObject Create(GameObject prefab, Transform onTransform = null)
        {
            if (_prefabToObjectMap.TryGetValue(prefab, out Stack<GameObject> pool))
            {
                if (pool.Count > 0)
                {
                    var instance = pool.Pop();
                    instance.transform.parent = onTransform;
                    instance.SetActive(true);
                    return instance;
                }
            }

            var newInstance = Instantiate(prefab, onTransform);
            newInstance.SetActive(true);
            return newInstance;
        }

        public void Recycle<T>(T prefab, T instance) where T : MonoBehaviour
        {
            Recycle(prefab.gameObject, instance.gameObject);
        }

        public void Recycle(GameObject prefab, GameObject instance)
        {
            if (!_prefabToObjectMap.TryGetValue(prefab, out Stack<GameObject> pool))
            {
                pool = new Stack<GameObject>();
                _prefabToObjectMap[prefab] = pool;
            }
            
            instance.SetActive(false);
            instance.transform.parent = transform;
            pool.Push(instance);
        }
        

    }
}