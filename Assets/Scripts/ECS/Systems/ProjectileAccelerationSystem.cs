using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using UnityEngine;

public class ProjectileAccelerationSystem : JobComponentSystem {
		protected override JobHandle OnUpdate(JobHandle inputDeps) {
				float deltaTime = Time.DeltaTime;

				Entities.ForEach((ref ProjectileSpeedData speedData, in ProjectileAccelerationData accelerationData) => {
						speedData.XZSpeed += accelerationData.XZAcceleration;
				}).Schedule(inputDeps).Complete();

				return default;
		}
}
