using Unity.Entities;
using Unity.Transforms;

public class StructureDamageSystem : SystemBase {
	protected override void OnUpdate() {
		Entities
			.WithAll<Translation, HealthData, StructureData>()
			.WithNone<DeleteEntityTag>()
			.ForEach((Entity entity, ref HealthData healthData, in Translation translation, in StructureData structureData) => {
				// Get structure position
                // Check goo height in that location
                // Apply structure damage (constant or based on goo height)
			}).ScheduleParallel();
	}
}
