using Unity.Entities;

public struct GooBomberData : IComponentData {
	public float timeSinceLastDrop;
	public float cooldown;
	public Entity bombPrefab;
}
