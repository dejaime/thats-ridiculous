using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

public class TweenCubeHeightSystem : SystemBase {
	protected override void OnUpdate() {
		float deltaTime = Time.DeltaTime;
        bool test = true;

		GooCubeGrid cubeGrid = GooCubeGrid.Instance;
		Entities
		.WithAll<PurpleGooTweenData, PurpleGooCubeData>()
		.ForEach((ref PurpleGooTweenData tweenData, in PurpleGooCubeData cubeData) => {
            float deltaPosition = cubeData.height - tweenData.visual_height;
            if (deltaPosition * deltaPosition < 1) {
                return;
            } else if (!cubeData.active) {
                deltaPosition = -200f;
            }

			// if (deltaPosition > 0) {
            //     tweenData.visual_height -= deltaPosition * deltaTime;
            // } else {
                tweenData.visual_height += deltaPosition * deltaTime * 0.8f;
            // }
            tweenData.visual_height = Mathf.Clamp(tweenData.visual_height, -1f, GooHeightSystem.MAX_CUBE_HEIGHT);

            if (test && cubeData.active) {
                test = false;
                Debug.Log ("Tween " +  tweenData.visual_height + " cubeData " + cubeData.height + " delta " + deltaPosition);
            }

		}).WithoutBurst().Run();
	}
} 
