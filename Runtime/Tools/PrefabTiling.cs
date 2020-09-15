#if UNITY_EDITOR
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace CustomPackages.Silicom.Core.Runtime.Tools
{
    public class PrefabTiling : MonoBehaviour
    {
        [SerializeField] private Transform tiledObject;
        [SerializeField] private Transform startPosition;    
        [SerializeField] private int tileCountX = 1;
        [SerializeField] private int tileCountY = 1;
        [SerializeField] private int tileCountZ = 1;
        [SerializeField] private Vector3 tilingOffSet;
    
        [SerializeField] private List<Transform> spawned = new List<Transform>();
    
        private Vector3 _startPosition;

        private Vector3 _scaling;

        [Button]
        private void Create()
        {
            if (tiledObject.gameObject == gameObject)
            {
                Debug.LogWarning("Place this script on the parent, not on the tiled object !");
                return;
            }
        
            if (tileCountX * tileCountY * tileCountZ > 10000)
            {
                Debug.LogWarning("You are trying to spawn more than 10.000 prefabs, is this intended ?", this);
                return;
            }
        
            _startPosition = startPosition.position;
            _scaling = tiledObject.lossyScale;
            _scaling.x *= tilingOffSet.x;
            _scaling.y *= tilingOffSet.y;
            _scaling.z *= tilingOffSet.z;

            bool isPrefab = EditorUtility.IsPersistent(tiledObject);
        
            Vector3 spawnPos = Vector3.zero;
        
            for (int x = 0; x < tileCountX; x++)
            {
                for (int y = 0; y < tileCountY; y++)
                {
                    for (int z = 0; z < tileCountZ; z++)
                    {
                    
                        if(x == 0 && y == 0 && z == 0) continue;

                        spawnPos.x = x * _scaling.x;
                        spawnPos.y = y * _scaling.y;
                        spawnPos.z = z * _scaling.z;

                        Transform tilled = isPrefab ? PrefabUtility.InstantiatePrefab(tiledObject) as Transform : Instantiate(tiledObject);
                        tilled.parent = transform;
                        tilled.SetPositionAndRotation(_startPosition + spawnPos, tiledObject.rotation);
                        spawned.Add(tilled);
                    }
                }
            }
        }

        [Button]
        private void Clear()
        {
            for (int i = 0; i < spawned.Count; i++)
            {
                if (spawned[i] != null) DestroyImmediate(spawned[i].gameObject);
            }
        
            spawned.Clear();
        }
    
    }
}

#endif