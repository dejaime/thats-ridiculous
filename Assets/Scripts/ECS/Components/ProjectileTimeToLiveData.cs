using Unity.Entities;

[GenerateAuthoringComponent]
public struct ProjectileTimeToLiveData : IComponentData {
    public float timeToLive;
}