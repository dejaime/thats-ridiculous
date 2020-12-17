using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct ProjectileData : IComponentData {
    public int hitsLeft;
    public float scale;
}