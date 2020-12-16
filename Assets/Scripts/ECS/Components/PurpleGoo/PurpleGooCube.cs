using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct PurpleGooCube : IComponentData {
    public int2 gridPosition;
    public float height;
}
