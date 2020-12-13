using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public class ProjectileSpeedData : IComponentData {
    public float2 XZDirection;
}