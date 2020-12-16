using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct PurpleGooCubeData : IComponentData {
    public int2 gridPosition;
    public float height;
}
