using Unity.Entities;
using Unity.Collections;
using Unity.Transforms;

[AlwaysSynchronizeSystem]
public class GooBombExplosionSystem : SystemBase {
	protected override void OnUpdate() {
		EntityCommandBuffer commandBuffer = new EntityCommandBuffer(Allocator.TempJob);
		GooCubeGrid grid = GooCubeGrid.Instance;

		Entities.WithAll<Translation, GooBombData>().WithNone<DeleteEntityTag>().ForEach((Entity entity, in Translation translation, in GooBombData bombData) => {
			if (!bombData.hitGround) {
				return;
			}
			commandBuffer.AddComponent<DeleteEntityTag>(entity);
			grid.Explode(translation.Value.x, translation.Value.z, bombData.bombSize);
		}).WithoutBurst().Run();

		commandBuffer.Playback(EntityManager);
		commandBuffer.Dispose();
	}
}
