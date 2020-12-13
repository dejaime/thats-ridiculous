using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using UnityEngine;

public class ProjectileMovementSystem : JobComponentSystem {
		protected override JobHandle OnUpdate(JobHandle inputDeps) {
				Entities.ForEach((ref PhysicsVelocity velocity, in ProjectileSpeedData projectileData) => {
						velocity.Linear.xz = projectileData.XZSpeed;
				}).Run();
				return inputDeps;
		}
}
