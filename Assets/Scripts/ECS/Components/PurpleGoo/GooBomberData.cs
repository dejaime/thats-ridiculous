using Unity.Entities;

[GenerateAuthoringComponent]
public struct GooBomberData : IComponentData {
	public float timeSinceLastDrop;
	public float cooldown;
	public float bombSize;
	public Entity bombEntityTemplate;
}
