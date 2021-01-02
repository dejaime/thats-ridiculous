using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

[AddComponentMenu("Scripts/ECS/Components/GooBomberData")]
[ConverterVersion("GooBomber", 1)]

public class GooBomberAuthoring : MonoBehaviour, IDeclareReferencedPrefabs, IConvertGameObjectToEntity {
    public GameObject bombPrefab;

    public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs) {
        referencedPrefabs.Add(bombPrefab);
    }

    public void Convert (Entity entity, EntityManager entityManager, GameObjectConversionSystem gameObjectConversionSystem) {
        var bomberData = new GooBomberData {
            bombPrefab = gameObjectConversionSystem.GetPrimaryEntity(bombPrefab),
            timeSinceLastDrop = 0,
            cooldown = 1
        };

        entityManager.AddComponentData<GooBomberData>(entity, bomberData);
    }
}
