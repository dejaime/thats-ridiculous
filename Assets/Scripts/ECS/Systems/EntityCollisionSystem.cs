using UnityEngine;
using Unity.Burst;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateAfter(typeof(EndFramePhysicsSystem))]
public class EntityCollisionSystem : SystemBase {
	BuildPhysicsWorld buildPhysicsWorldSystem;
	StepPhysicsWorld stepPhysicsWorldSystem;
	EntityQuery projectileGroup, purpleCubeGroup;

	protected override void OnCreate() {
		buildPhysicsWorldSystem = World.GetOrCreateSystem<BuildPhysicsWorld>();
		stepPhysicsWorldSystem = World.GetOrCreateSystem<StepPhysicsWorld>();

		projectileGroup = GetEntityQuery(new EntityQueryDesc {
			All = new ComponentType[] { typeof(ProjectileData) }
		});

		purpleCubeGroup = GetEntityQuery(new EntityQueryDesc {
			All = new ComponentType[] { typeof(PurpleGooCubeData) }
		});
	}

	[BurstCompile]
	struct CollisionCubeHitJob : ICollisionEventsJob {
		public ComponentDataFromEntity<ProjectileData> innerProjectileGroup;
		public ComponentDataFromEntity<PurpleGooCubeData> innerCubeGroup;

		public void Execute(CollisionEvent collisionEvent) {
			Entity entityA = collisionEvent.EntityA;
			Entity entityB = collisionEvent.EntityB;

			bool isBodyAProjectile = innerProjectileGroup.HasComponent(entityA);
			bool isBodyBProjectile = innerProjectileGroup.HasComponent(entityB);

			bool isBodyACube = innerCubeGroup.HasComponent(entityA);
			bool isBodyBCube = innerCubeGroup.HasComponent(entityB);

			Entity projectile, cube;
			if (isBodyAProjectile && isBodyBCube) {
				projectile = entityA;
				cube = entityB;
			} else if (isBodyBProjectile && isBodyACube) {
				projectile = entityB;
				cube = entityA;
			} else {
				//Not a hit, not cube X projectile
				return;
			}

			ProjectileData projectileData = innerProjectileGroup[projectile];
			PurpleGooCubeData cubeData = innerCubeGroup[cube];
			if (projectileData.hitsLeft > 0 && cubeData.height > 0) {
				cubeData.height -= 5f;
				innerCubeGroup[cube] = cubeData;

				projectileData.hitsLeft -= 1;
				innerProjectileGroup[projectile] = projectileData;
			}
		}
	}


	protected override void OnUpdate() {
		if (projectileGroup.CalculateEntityCount() == 0) {
			return;
		}

		Dependency = new CollisionCubeHitJob {
			innerProjectileGroup = GetComponentDataFromEntity<ProjectileData>(),
			innerCubeGroup = GetComponentDataFromEntity<PurpleGooCubeData>(),
		}.Schedule(stepPhysicsWorldSystem.Simulation,
			ref buildPhysicsWorldSystem.PhysicsWorld, Dependency);

		Dependency.Complete();
	}
}
