using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct ProjectileSpeedData : IComponentData {
    public float2 XZSpeed;
}