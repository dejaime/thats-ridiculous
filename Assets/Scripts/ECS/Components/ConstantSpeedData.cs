using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct ConstantSpeedData : IComponentData {
	public float3 constantSpeed;
}
