using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;

public class ProjectileAccelerationSystem : SystemBase {
	protected override void OnUpdate() {
		float deltaTime = Time.DeltaTime;

		Entities
		.WithAll<PhysicsVelocity, ProjectileAccelerationData>()
		.WithNone<InitialProjectileSpatialData>()
		.ForEach((ref PhysicsVelocity velocity, in ProjectileAccelerationData accelerationData) => {
			velocity.Linear += accelerationData.acceleration * deltaTime;
		}).Run();
	}
}
