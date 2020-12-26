using Unity.Entities;
using Unity.Collections;

[AlwaysSynchronizeSystem]
public class GooBombExplosionSystem : SystemBase {
	protected override void OnUpdate() {
		EntityCommandBuffer commandBuffer = new EntityCommandBuffer(Allocator.TempJob);

		Entities.WithAll<GooBombData>().WithNone<DeleteEntityTag>().ForEach((Entity entity, in GooBombData bombData) => {
			if (bombData.hitGround) {
				commandBuffer.AddComponent<DeleteEntityTag>(entity);
			}
		}).Run();

		commandBuffer.Playback(EntityManager);
		commandBuffer.Dispose();
	}
}
