using Unity.Entities;

[GenerateAuthoringComponent]
public struct GooBomberData : IComponentData {
	public float timeSinceLastDrop;
	public float cooldown;
}
