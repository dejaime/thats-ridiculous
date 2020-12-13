using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using UnityEngine;

[AlwaysSynchronizeSystem]
public class ProjectileTimeToLiveSystem : JobComponentSystem {
		protected override JobHandle OnUpdate(JobHandle inputDeps) {
				float deltaTime = Time.DeltaTime;

				Entities.WithAll<ProjectileTimeToLiveData>().ForEach((ref ProjectileTimeToLiveData ttlData) => {
						ttlData.timeToLive -= deltaTime;
				}).Run();

				return default;
		}
}
