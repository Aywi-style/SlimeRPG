using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChunksConfig", menuName = "Configs/Chunks")]
public class ChunksConfig : ScriptableObject
{
    [field: SerializeField] public int MaxChunkCount { private set; get; }
    [field: SerializeField] public int StartChunkPositionOffset { private set; get; }

    [field: Space]
    [field: SerializeField] public int DisablingChunkPositionOffset { private set; get; }
    [field: SerializeField] public int EnablingChunkPositionOffset { private set; get; }

    [field: Space]
    [field: SerializeField] public ChunkMB[] ChunksMBs { private set; get; }
}
