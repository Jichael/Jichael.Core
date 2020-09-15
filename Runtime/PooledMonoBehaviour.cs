using System;
using System.Collections;
using UnityEngine;

namespace CustomPackages.Silicom.Core.Runtime
{

    public class PooledMonoBehaviour : MonoBehaviour
    {
        public int InitialPoolSize => initialPoolSize;
        [SerializeField] private int initialPoolSize = 1;

        public event Action<PooledMonoBehaviour> OnReturnToPool;

        private void OnDisable()
        {
            OnReturnToPool?.Invoke(this);
        }

        public T Get<T>(bool enable = true) where T : PooledMonoBehaviour
        {
            var pooledObject = Pool.GetPool(this).Get<T>();
            if (enable)
            {
                pooledObject.gameObject.SetActive(true);
            }

            return pooledObject;
        }

        public T Get<T>(Vector3 position, bool enable = true) where T : PooledMonoBehaviour
        {
            var pooledObject = Get<T>(enable);
            pooledObject.transform.position = position;
            return pooledObject;
        }

        public T Get<T>(Vector3 position, Quaternion rotation, bool enable = true) where T : PooledMonoBehaviour
        {
            var pooledObject = Get<T>(enable);
            pooledObject.transform.SetPositionAndRotation(position, rotation);
            return pooledObject;
        }

        // Protected so that only inheritors can call to it, may want to set to public for reasons
        protected void ReturnToPool(float delay = 0)
        {
            if (Mathf.Approximately(delay, 0))
            {
                gameObject.SetActive(false);
            }
            else
            {
                StartCoroutine(ReturnToPoolCo(delay));
            }
        }

        private IEnumerator ReturnToPoolCo(float delay)
        {
            yield return new WaitForSeconds(delay);
            gameObject.SetActive(false);
        }
    }
}