using Unity.Entities;

public class HealthSystem : SystemBase {
	protected override void OnUpdate() {
		Entities.WithAll<HealthData>().WithNone<DeleteEntityTag>().ForEach((Entity entity, ref HealthData healthData) => {
            healthData.health -= healthData.pendingDamage;
		}).ScheduleParallel();
	}
}
