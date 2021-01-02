using Unity.Entities;
using Unity.Burst;
using Unity.Mathematics;
using Unity.Transforms;

public class GooBomberDropSystem : SystemBase {

	BeginInitializationEntityCommandBufferSystem entityCommandBufferSystem;

	protected override void OnCreate() {
		base.OnCreate();
 		entityCommandBufferSystem = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
	}

	protected override void OnUpdate() {
		float deltaTime = Time.DeltaTime;
		EntityCommandBuffer.ParallelWriter commandBuffer = entityCommandBufferSystem.CreateCommandBuffer().AsParallelWriter();

		Entities
		.WithAll<GooBomberData, Translation>()
		.WithBurst(FloatMode.Default, FloatPrecision.Standard, true)
		.ForEach((int entityInQueryIndex, ref GooBomberData gooBomberData, in Translation translation) => {
			gooBomberData.timeSinceLastDrop += deltaTime;
			if (gooBomberData.timeSinceLastDrop > gooBomberData.cooldown) {
				gooBomberData.timeSinceLastDrop -= gooBomberData.cooldown;
				Entity bomb = commandBuffer.Instantiate(entityInQueryIndex, gooBomberData.bombPrefab);

				Translation bombTranslation = new Translation();
				bombTranslation.Value = new float3 {
					x = translation.Value.x,
					y = translation.Value.y,
					z = translation.Value.z
				};

				commandBuffer.SetComponent<Translation>(entityInQueryIndex, bomb, bombTranslation);
			}
		}).ScheduleParallel();

		entityCommandBufferSystem.AddJobHandleForProducer(Dependency);
	}

}
