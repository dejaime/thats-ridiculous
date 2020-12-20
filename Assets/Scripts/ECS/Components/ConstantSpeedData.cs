using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct ConstantSpeedData : IComponentData {
    float2 constantSpeed;
}
