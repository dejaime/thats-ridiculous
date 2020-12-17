using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

public class PurpleGooHeightSystem : SystemBase {
	protected override void OnUpdate() {
		float deltaTime = Time.DeltaTime;

		Entities
		.WithAll<NonUniformScale, PhysicsCollider, PurpleGooCubeData>()
		.ForEach((ref NonUniformScale nonUniformScale, ref Translation translation, ref PhysicsCollider collider, in PurpleGooCubeData cubeData) => {
			nonUniformScale.Value.y = cubeData.height;
			//Look, I know... unsafe code is bad, ugly, and all
			//		sorry all, rustaceans especially
			//		there's no other way of modifying a collider at runtime...
			unsafe {
				Unity.Physics.BoxCollider* boxColliderPtr = (Unity.Physics.BoxCollider*)collider.ColliderPtr;
				BoxGeometry geometry = boxColliderPtr->Geometry;
				geometry.Size = new float3 {
					x = geometry.Size.x,
					z = geometry.Size.z,
					y = cubeData.height > 0.01f ? cubeData.height : 0.01f
				};
				boxColliderPtr->Geometry = geometry;
			}
		}).Schedule();
	}
}
