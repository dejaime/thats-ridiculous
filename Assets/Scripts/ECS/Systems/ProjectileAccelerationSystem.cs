using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using UnityEngine;

public class ProjectileAccelerationSystem : SystemBase {
		protected override void OnUpdate() {
				float deltaTime = Time.DeltaTime;

				Entities.ForEach((ref ProjectileSpeedData speedData, in ProjectileAccelerationData accelerationData) => {
						speedData.XZSpeed += accelerationData.XZAcceleration;
				}).Run();
		}
}
