using Unity.Entities;
using Unity.Jobs;
using Unity.Collections;
using Unity.Physics;
using Unity.Transforms;

public class InitialProjectileSpatialSystem : SystemBase {
	protected override void OnUpdate() {
		float deltaTime = Time.DeltaTime;

		EntityCommandBuffer commandBuffer = new EntityCommandBuffer(Allocator.TempJob);

		Entities.ForEach((
			Entity entity,
			ref PhysicsVelocity velocity,
			ref Translation translation,
			ref ProjectileAccelerationData accelerationData,
			in InitialProjectileSpatialData initialSpatialData) => {

				commandBuffer.RemoveComponent<InitialProjectileSpatialData>(entity);

				translation.Value = initialSpatialData.spawnPosition;
				velocity.Linear = initialSpatialData.speed;
				accelerationData.acceleration = initialSpatialData.acceleration;

			}).Run();

		commandBuffer.Playback(EntityManager);
		commandBuffer.Dispose();
	}
}
