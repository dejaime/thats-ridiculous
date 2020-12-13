using Unity.Entities;
using Unity.Collections;
using Unity.Jobs;

[AlwaysSynchronizeSystem]
public class EntityCleanUpSystem : JobComponentSystem {
		protected override JobHandle OnUpdate(JobHandle inputDeps) {
				EntityCommandBuffer commandBuffer = new EntityCommandBuffer(Allocator.TempJob);

				Entities.WithAll<DeleteEntityTag>().ForEach((Entity entity) => {
                    commandBuffer.DestroyEntity(entity);
				}).Run();

                commandBuffer.Playback(EntityManager);
                commandBuffer.Dispose();
                
				return default;
		}
}
