using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct InitialProjectileSpatialData : IComponentData {
    public float3 spawnPosition;
    public float3 speed;
    public float3 acceleration;
}