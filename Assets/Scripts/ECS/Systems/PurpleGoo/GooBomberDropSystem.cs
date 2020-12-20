using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

public class GooBomberDropSystem : SystemBase {
	protected override void OnUpdate() {
		float deltaTime = Time.DeltaTime;

		Entities
		.WithAll<GooBomberData, Translation>()
		.ForEach((ref GooBomberData gooBomberData, in Translation translation) => {
            gooBomberData.timeSinceLastDrop += deltaTime;
            if (gooBomberData.timeSinceLastDrop > gooBomberData.cooldown) {
                gooBomberData.timeSinceLastDrop -= gooBomberData.cooldown;
                //DROP
            }
		}).Schedule();
	}
}
