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
	EntityQuery projectileGroup, purpleCubeGroup, bombGroup, groundGroup;

	protected override void OnCreate() {
		buildPhysicsWorldSystem = World.GetOrCreateSystem<BuildPhysicsWorld>();
		stepPhysicsWorldSystem = World.GetOrCreateSystem<StepPhysicsWorld>();

		projectileGroup = GetEntityQuery(new EntityQueryDesc {
			All = new ComponentType[] { typeof(ProjectileData) }
		});

		purpleCubeGroup = GetEntityQuery(new EntityQueryDesc {
			All = new ComponentType[] { typeof(PurpleGooCubeData) }
		});

		bombGroup = GetEntityQuery(new EntityQueryDesc {
			All = new ComponentType[] { typeof(GooBombData) }
		});

		groundGroup = GetEntityQuery(new EntityQueryDesc {
			All = new ComponentType[] { typeof(GroundTag) }
		});
	}

	[BurstCompile]
	struct CollisionCubeHitJob : ICollisionEventsJob {
		public ComponentDataFromEntity<ProjectileData> innerProjectileGroup;
		public ComponentDataFromEntity<PurpleGooCubeData> innerCubeGroup;

		public ComponentDataFromEntity<GroundTag> innerGroundGroup;
		public ComponentDataFromEntity<GooBombData> innerBombGroup;

		public void Execute(CollisionEvent collisionEvent) {
			Debug.Log("COLLISION EVENT");
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
				bool isBodyAGround = innerGroundGroup.HasComponent(entityA);
				bool isBodyBGround = innerGroundGroup.HasComponent(entityB);

				bool isBodyABomb = innerBombGroup.HasComponent(entityA);
				bool isBodyBBomb = innerBombGroup.HasComponent(entityB);

				Entity bomb;
				if (isBodyAGround && isBodyBBomb) {
					bomb = entityB;
				} else if (isBodyBGround && isBodyABomb) {
					bomb = entityA;
				} else {
					return;
				}

				GooBombData bombData = innerBombGroup[bomb];
				bombData.hitGround = true;

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
		if (projectileGroup.CalculateEntityCount() == 0 && bombGroup.CalculateEntityCount() == 0) {
			return;
		}

		Dependency = new CollisionCubeHitJob {
			innerProjectileGroup = GetComponentDataFromEntity<ProjectileData>(),
			innerCubeGroup = GetComponentDataFromEntity<PurpleGooCubeData>(),
			innerGroundGroup = GetComponentDataFromEntity<GroundTag>(),
			innerBombGroup = GetComponentDataFromEntity<GooBombData>(),
		}.Schedule(stepPhysicsWorldSystem.Simulation,
			ref buildPhysicsWorldSystem.PhysicsWorld, Dependency);

		Dependency.Complete();
	}
}
