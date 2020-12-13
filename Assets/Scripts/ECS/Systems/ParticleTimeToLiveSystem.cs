using Unity.Entities;
using Unity.Jobs;
using Unity.Collections;

[AlwaysSynchronizeSystem]
public class ProjectileTimeToLiveSystem : SystemBase {
		protected override void OnUpdate() {
				float deltaTime = Time.DeltaTime;
                
				Entities.WithAll<ProjectileTimeToLiveData>().ForEach((Entity entity, ref ProjectileTimeToLiveData ttlData) => {
						ttlData.timeToLive -= deltaTime;
				}).Run();
		}
}
