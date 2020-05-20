using Jichaels.Core;
using UnityEngine;

public class ExemplePooledObject : PooledMonoBehaviour
{
    [SerializeField] private float timeToLive = 3;
    private float _timer;

    
    // Reset everything that needs to be reset after pooling here
    private void OnEnable()
    {
        _timer = 0;
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= timeToLive)
        {
            ReturnToPool(0);
        }
    }
}
