using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct PurpleGooCubeData : IComponentData {
	public int3 gridIndex;
	public float height;
	public float pendingDamage;

	public bool active;
}
