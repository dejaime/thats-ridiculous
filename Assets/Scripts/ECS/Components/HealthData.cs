using Unity.Entities;
using Unity.Mathematics;

// This component keeps track of the health of an entity, and, along with HealthSystem it 
//  applies damage on the same frame or the one following the one where damage was generated.
// Mind though that this does not destroy entities. Nothing happens if an entity's health goes below 0.
// We still need to do something with this information somewhere else.
[GenerateAuthoringComponent]
public struct HealthData : IComponentData {
    public float health;
    public float pendingDamage;
}
