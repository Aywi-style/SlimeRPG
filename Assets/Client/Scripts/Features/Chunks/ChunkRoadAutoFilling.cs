using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class ChunkRoadAutoFilling : MonoBehaviour
{
    [SerializeField] private bool _isAutoFilling = false;

    [SerializeField] private ChunkMB _chunkMB;

    private void Start()
    {
        if (TryGetComponent(out ChunkMB chunkMB))
        {
            _chunkMB = chunkMB;
        }

        if (Application.IsPlaying(this))
        {
            CorrectChildCount();
            Destroy(this);
        }
    }

    private void Update()
    {
        CorrectChildCount();
    }

    private void CorrectChildCount()
    {
        if (_chunkMB == null)
        {
            return;
        }

        if (!_isAutoFilling)
        {
            return;
        }

        if (_chunkMB.RoadParent.childCount < _chunkMB.RoadPrefabCount)
        {
            for (int i = _chunkMB.RoadParent.childCount; i < _chunkMB.RoadPrefabCount; i++)
            {
                if (_chunkMB.RoadPrefab == null)
                {
                    Debug.LogError("_roadPrefab in " + name + " is null!");
                    continue;
                }

                Transform instantiate = Instantiate(_chunkMB.RoadPrefab, _chunkMB.RoadParent);
                instantiate.Translate(Vector3.forward * _chunkMB.RoadLength * i);
            }
        }
        else if (_chunkMB.RoadParent.childCount > _chunkMB.RoadPrefabCount)
        {
            for (int i = _chunkMB.RoadParent.childCount - 1; i >= _chunkMB.RoadPrefabCount; i--)
            {
                DestroyImmediate(_chunkMB.RoadParent.GetChild(i).gameObject);
            }
        }
    }
}
