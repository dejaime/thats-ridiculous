using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class GooHeightSystem : SystemBase {
	public const float MAX_CUBE_HEIGHT = 350f;
	protected override void OnUpdate() {
		Entities
		.WithAll<PurpleGooCubeData, PurpleGooTweenData>()
		.ForEach((ref Translation translation, in PurpleGooTweenData tweenData, in PurpleGooCubeData cubeData) => {
			float height = tweenData.visual_height > MAX_CUBE_HEIGHT ? MAX_CUBE_HEIGHT : tweenData.visual_height;
			
			if (cubeData.active)
				translation.Value.y = height - MAX_CUBE_HEIGHT;
			else
				translation.Value.y = GooCubeGrid.DEACTIVATED_CUBE_Y_POSITION;
		}).Schedule();
	}
}
