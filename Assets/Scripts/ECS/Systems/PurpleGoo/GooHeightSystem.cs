using Unity.Entities;
using Unity.Transforms;

public class GooHeightSystem : SystemBase {
	public const float MAX_CUBE_HEIGHT = 350f;
	protected override void OnUpdate() {
		Entities
		.WithAll<PurpleGooCubeData>()
		.ForEach((ref Translation translation, in PurpleGooCubeData cubeData) => {
			float height = cubeData.height > MAX_CUBE_HEIGHT ? MAX_CUBE_HEIGHT : cubeData.height;
			
			if (cubeData.active)
				translation.Value.y = height - MAX_CUBE_HEIGHT;
			else
				translation.Value.y = GooCubeGrid.DEACTIVATED_CUBE_Y_POSITION;
		}).Schedule();
	}
}
