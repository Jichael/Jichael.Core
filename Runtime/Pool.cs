using System.Collections.Generic;
using UnityEngine;

namespace CustomPackages.Silicom.Core.Runtime
{
    public class Pool : MonoBehaviour
    {

        private static readonly Dictionary<PooledMonoBehaviour, Pool> Pools =
            new Dictionary<PooledMonoBehaviour, Pool>();

        [SerializeField] private PooledMonoBehaviour toPool;

        private readonly Queue<PooledMonoBehaviour> _pooledObjects = new Queue<PooledMonoBehaviour>();

        public static Pool GetPool(PooledMonoBehaviour pooledObject)
        {
            if (Pools.ContainsKey(pooledObject))
            {
                return Pools[pooledObject];
            }

            var pool = new GameObject($"Pool-{pooledObject.name}").AddComponent<Pool>();
            DontDestroyOnLoad(pool);
            pool.toPool = pooledObject;
            for (int i = 0; i < pooledObject.InitialPoolSize; i++)
            {
                pool.GrowPool();
            }

            Pools.Add(pooledObject, pool);
            return pool;
        }

        public T Get<T>() where T : PooledMonoBehaviour
        {
            if (_pooledObjects.Count == 0)
            {
                GrowPool();
            }

            var pooledObject = _pooledObjects.Dequeue();
            return pooledObject as T;
        }

        private void GrowPool()
        {
            var pooledObject = Instantiate(toPool, transform);
            pooledObject.OnReturnToPool += AddObjectToQueue;
            pooledObject.gameObject.SetActive(false); // Triggers AddObjectToQueue
        }

        private void AddObjectToQueue(PooledMonoBehaviour pooled)
        {
            _pooledObjects.Enqueue(pooled);
        }
    }
}