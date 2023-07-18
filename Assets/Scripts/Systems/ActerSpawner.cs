using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine;

[BurstCompile]
[UpdateInGroup(typeof(InitializationSystemGroup))]
public partial struct ActerSpawner : ISystem {

    void OnCreate(ref SystemState state) {
        state.RequireForUpdate<ControllerProperties>();
    }

    [BurstCompile]
    void OnUpdate(ref SystemState state) {
        state.Enabled = false;
        Entity controllerPropertiesEntity = SystemAPI.GetSingletonEntity<ControllerProperties>();
        ControllerProperties controllerProperties = SystemAPI.GetComponent<ControllerProperties>(controllerPropertiesEntity);
        EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);
        for(int i = 0; i < controllerProperties.numberActersToSpawn; i++) {
            Entity acter = ecb.Instantiate(controllerProperties.acterPrefab);
            ecb.AddComponent(acter, new ActerTag{});
            ecb.SetComponent(acter, new LocalTransform {
                Position = new float3(3, 0, 0),
                Scale = 1
            });
            ecb.AddComponent(acter, new Movement {
                velocity = float3.zero,
                destination = new float3(3, 0, 0)
            });
            ecb.AddComponent(acter, new RandomNumberGenerator {
                value = Unity.Mathematics.Random.CreateFromIndex((uint) i + 1)
            });
        }
        ecb.Playback(state.EntityManager);
        //ecb.Dispose();
    }
}

