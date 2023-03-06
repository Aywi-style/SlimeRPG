using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkMB : MonoBehaviour
{
    [field: SerializeField] public Transform ChunkEnd { get; private set; }

    [field: Space]
    [field: Range(1, 100)]
    [field: SerializeField] public int RoadPrefabCount { get; private set; } = 1;
    [field: SerializeField] public Transform RoadParent { get; private set; }
    [field: SerializeField] public Transform RoadPrefab { get; private set; }
    [field: SerializeField] public float RoadLength { get; private set; }

    private void OnDrawGizmos()
    {
        DrawChunkStartAndEnd();
    }

    private void DrawChunkStartAndEnd()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 0.5f);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(ChunkEnd.position, 0.5f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;

        var leftLine = new Vector3(-2f, 1f, transform.position.z);
        DrawRunLine(leftLine);

        var middleLine = new Vector3(0f, 1f, transform.position.z);
        DrawRunLine(middleLine);

        var rightLine = new Vector3(2f, 1f, transform.position.z);
        DrawRunLine(rightLine);

    }

    private void DrawRunLine(Vector3 from)
    {
        Gizmos.DrawLine(from, from + (Vector3.forward * (RoadLength * RoadPrefabCount)));
    }
}
