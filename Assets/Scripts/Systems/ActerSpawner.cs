#if false

using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;
using Unity.Rendering;
using System.Collections.Generic;
using System.Linq;

[BurstCompile]
[UpdateInGroup(typeof(InitializationSystemGroup))]
public partial struct ActerSpawner : ISystem {
    //static NativeHashMap<int, Entity> children;
    //static NativeHashMap<int, Entity> parents;

    void OnCreate(ref SystemState state) {
        //state.RequireForUpdate<ControllerProperties>();
        state.RequireForUpdate<EntityLinkRequest>();
        //children = new();
        //parents = new();
    }

    //[BurstCompile]
    //void OnUpdate(ref SystemState state) {
    //    state.Enabled = false;
    //    Entity controllerPropertiesEntity = SystemAPI.GetSingletonEntity<ControllerProperties>();
    //    ControllerProperties controllerProperties = SystemAPI.GetComponent<ControllerProperties>(controllerPropertiesEntity);
    //    EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);
    //    for (int i = 0; i < controllerProperties.numberActersToSpawn; i++) {
    //        Entity acter = ecb.Instantiate(controllerProperties.acterPrefab);
    //        //Entity acter = ecb.CreateEntity();
    //        //ecb.AddComponent(acter, new RenderMesh {
    //        //    mesh = controllerProperties.acterMesh,
    //        //    material = controllerProperties.acterMaterial
    //        //});
    //        ecb.AddComponent(acter, new ActerTag { });
    //        ecb.SetComponent(acter, new LocalTransform {
    //            Position = new float3(0, 0, 0),
    //            Scale = 1
    //        });
    //        ecb.AddComponent(acter, new Movement {
    //            velocity = float3.zero,
    //            destination = new float3(0, 0, 0)
    //        });
    //        ecb.AddComponent(acter, new RandomNumberGenerator {
    //            value = Unity.Mathematics.Random.CreateFromIndex((uint) i + 1)
    //        });
    //    }
    //    ecb.Playback(state.EntityManager);
    //    //ecb.Dispose();
    //}

    [BurstCompile]
    void OnUpdate(ref SystemState state) {
        //NativeHashMap<int, Entity> children = new();
        //NativeHashMap<int, Entity> parents = new();
        EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);
        foreach ((RefRO<EntityLinkRequest> request, DynamicBuffer<LinkedEntityGroup> children, Entity entity) in SystemAPI.Query<RefRO<EntityLinkRequest>, DynamicBuffer<LinkedEntityGroup>>().WithEntityAccess()) {
            //if (request.ValueRO.mode == LinkingMode.Child) children.Add(request.ValueRO.hash, entity);
            //if (request.ValueRO.mode == LinkingMode.Parent) parents.Add(request.ValueRO.hash, entity);
            foreach(LinkedEntityGroup child in children.AsNativeArray()) {
                Debug.Log("Linked: " + child.Value + " to " + entity);
                //ecb.RemoveComponent(entity, typeof(EntityLinkRequest));
                ecb.AddComponent(child.Value, new Parent { Value = entity});
            }
        }
        //int[] matches = children.GetKeyArray(Allocator.Temp).Intersect(parents.GetKeyArray(Allocator.Temp)).ToArray();
        //if (matches.Length <= 0) return;
        //foreach (int match in matches) {
        //    Entity child = children[match];
        //    Entity parent = parents[match];
        //    ecb.AddComponent(child, new Parent { Value = parent });
        //    ecb.RemoveComponent(child, typeof(EntityLinkRequest));
        //    DynamicBuffer<LinkedEntityGroup> buffer = ecb.AddBuffer<LinkedEntityGroup>(parent);
        //    buffer.Add(child);
        //    ecb.RemoveComponent(parent, typeof(EntityLinkRequest));
        //}
        ecb.Playback(entityManager);
        ecb.Dispose();
    }
}

#endif