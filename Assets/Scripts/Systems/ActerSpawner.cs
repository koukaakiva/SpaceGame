using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

[BurstCompile]
public partial struct ActerSpawner : ISystem {
    void OnCreate(ref SystemState state) { }

    void OnDestroy(ref SystemState state) { }

    [BurstCompile]
    void OnUpdate(ref SystemState state) {
        state.Enabled = false;
        Entity controllerPropertiesEntity = SystemAPI.GetSingletonEntity<ControllerProperties>();
        ControllerProperties controllerProperties = SystemAPI.GetComponent<ControllerProperties>(controllerPropertiesEntity);
        EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);
        for(int i = 0; i < controllerProperties.numberActersToSpawn; i++) {
            Entity acter = ecb.Instantiate(controllerProperties.acterPrefab);
            ecb.SetComponent(acter, new LocalTransform {
                Position = new float3(3, 0, 0),
                Scale = 1
            }) ;
        }
        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}

