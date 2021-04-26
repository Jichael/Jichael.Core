using UnityEngine;
using Random = UnityEngine.Random;

namespace CustomPackages.Silicom.Core.Samples.PoolingExample
{
    public class ExempleSpawn : MonoBehaviour
    {
        [SerializeField] private ExemplePooledObject toSpawn;    
        [SerializeField] private float timeBetweenSpawn = 1;
        private float _timer;
    
        private void Update()
        {
            _timer += Time.deltaTime;
            if (!(_timer >= timeBetweenSpawn)) return;
        
            _timer -= timeBetweenSpawn;
            Spawn();
        }

        private void Spawn()
        {
            Vector3 spawnPosition = new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), Random.Range(-5f, 5f));
            toSpawn.Get<ExemplePooledObject>(spawnPosition);
        }
    }
}
