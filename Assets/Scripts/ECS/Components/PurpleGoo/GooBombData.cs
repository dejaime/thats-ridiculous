using Unity.Entities;

[GenerateAuthoringComponent]
public struct GooBombData : IComponentData {
	public float bombSize;
	public bool hitGround;
}
