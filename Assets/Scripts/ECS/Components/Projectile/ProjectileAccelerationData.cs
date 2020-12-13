using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct ProjectileAccelerationData : IComponentData {
    public float2 XZAcceleration;
}