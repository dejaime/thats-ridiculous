using UnityEngine;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public class OutOfBoundsSystem : SystemBase {
	protected override void OnUpdate() {

		EntityCommandBuffer commandBuffer = new EntityCommandBuffer(Allocator.TempJob);

		Entities.WithAll<OutOfBoundsData, Translation>().ForEach((Entity entity, in OutOfBoundsData boundsData, in Translation translation) => {
			if (CheckOutOfBounds(translation.Value, boundsData.min, boundsData.max)) {
				commandBuffer.AddComponent<DeleteEntityTag>(entity);
			}
		}).Run();

		commandBuffer.Playback(EntityManager);
		commandBuffer.Dispose();
	}

    private static bool CheckOutOfBounds(float3 translation, float3 minBounds, float3 maxBounds) {
        return translation.x < minBounds.x ||
                translation.y < minBounds.y ||
                translation.z < minBounds.z ||
                translation.x > maxBounds.x ||
                translation.y > maxBounds.y ||
                translation.z > maxBounds.z;
    }
}