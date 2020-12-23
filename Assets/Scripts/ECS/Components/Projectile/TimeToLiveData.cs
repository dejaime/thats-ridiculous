using Unity.Entities;

[GenerateAuthoringComponent]
public struct TimeToLiveData : IComponentData {
	public float timeToLive;
}