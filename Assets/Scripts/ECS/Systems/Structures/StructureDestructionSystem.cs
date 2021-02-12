using Unity.Collections;
using Unity.Entities;

public class StructureDestructionSystem : SystemBase {
	protected override void OnUpdate() {
		float deltaTime = Time.DeltaTime;

		EntityCommandBuffer commandBuffer = new EntityCommandBuffer(Allocator.TempJob);

		Entities.WithAll<HealthData, StructureData>().WithNone<DeleteEntityTag>().ForEach((Entity entity, ref StructureData structureData, in HealthData healthData) => {
			if (healthData.health <= 0) {
				if (!structureData.isRebuildable) {
					structureData.isDestroyed = true;
					commandBuffer.AddComponent<DeleteEntityTag>(entity);
				} else {
					if (structureData.isDestroyed) {
						// No health but already destroyed, nothing to do but update dead time.
						structureData.timeSinceDestruction += deltaTime;
					} else {
						// No health, but not yet destroyed, destroy it.
						structureData.isDestroyed = true;
						// TODO	Apply destruction effects, whatever those are.
					}
				}
			} else {
				if (structureData.isDestroyed) {
					// Structure destroyed but with positive health, this means it was repaired.
					// Non repairable strucrues (isRebuildable == false) wouldn't enter here
					//		as they had the DeleteEntityTag added to them when they were first destroyed.
					if (structureData.isEssential) {

					}

					// TODO Undo the effects applied on destruction.
					structureData.isDestroyed = false;
					structureData.timeSinceDestruction = 0;
				}
				// If NOT destroyed and health > 0, structure is fine, keep going.
			}
		}).ScheduleParallel();

		commandBuffer.Playback(EntityManager);
		commandBuffer.Dispose();
	}
}
