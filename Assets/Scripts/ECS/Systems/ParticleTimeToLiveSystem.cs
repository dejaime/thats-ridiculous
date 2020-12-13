using Unity.Entities;
using Unity.Jobs;
using Unity.Collections;

[AlwaysSynchronizeSystem]
public class ProjectileTimeToLiveSystem : SystemBase {
		protected override void OnUpdate() {
				float deltaTime = Time.DeltaTime;

                EntityCommandBuffer commandBuffer = new EntityCommandBuffer(Allocator.TempJob);
                
				Entities.WithAll<ProjectileTimeToLiveData>().ForEach((Entity entity, ref ProjectileTimeToLiveData ttlData) => {
						ttlData.timeToLive -= deltaTime;
                        if (ttlData.timeToLive < 0) {
                            commandBuffer.AddComponent<DeleteEntityTag>(entity);
                        }
				}).Run();

                commandBuffer.Playback(EntityManager);
                commandBuffer.Dispose();
		}
}
