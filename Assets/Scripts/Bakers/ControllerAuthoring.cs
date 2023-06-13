using UnityEngine;
using Unity.Mathematics;
using Unity.Entities;

public class ControllerAuthoring : MonoBehaviour {
    public float2 fieldDimensions;
    public int numberActersToSpawn;
    public GameObject acterPrefab;
    public uint randomSeed;
}

public struct ControllerProperties : IComponentData {
    public float2 fieldDimensions;
    public int numberActersToSpawn;
    public Entity acterPrefab;
}

public class ControllerBaker : Baker<ControllerAuthoring> {
    public override void Bake(ControllerAuthoring authoring) {
        Entity entity = GetEntity(TransformUsageFlags.None);
        AddComponent(entity, new ControllerProperties {
            fieldDimensions = authoring.fieldDimensions,
            numberActersToSpawn = authoring.numberActersToSpawn,
            acterPrefab = GetEntity(authoring.acterPrefab, TransformUsageFlags.Dynamic)
        });
        AddComponent(entity, new RandomNumberGenerator {
            value = Unity.Mathematics.Random.CreateFromIndex(authoring.randomSeed)
        });
    }
}
