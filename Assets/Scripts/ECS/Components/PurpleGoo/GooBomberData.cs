using Unity.Entities;

[GenerateAuthoringComponent]
public struct GooBomberData : IComponentData {
    float timeSinceLastDrop;
    float cooldown;
}
