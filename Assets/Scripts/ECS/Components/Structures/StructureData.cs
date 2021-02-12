using Unity.Entities;

[GenerateAuthoringComponent]
public struct StructureData : IComponentData {
    public bool isEssential;
    public bool isRebuildable;
    public bool isDestroyed;
    public float timeSinceDestruction;
}
