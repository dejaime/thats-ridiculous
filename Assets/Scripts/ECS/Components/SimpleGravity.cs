using Unity.Entities;

[GenerateAuthoringComponent]
public struct SimpleGravityData : IComponentData {
	public float gravity;
    public float currentVerticalSpeed;
}
