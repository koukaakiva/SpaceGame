using Unity.Entities;
using Unity.Burst;
using Unity.Physics;
using Unity.Physics.Systems;
using UnityEngine;

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateAfter(typeof(PhysicsSystemGroup))]
[BurstCompile]
public partial struct ActerOrdersSystem : ISystem
{
    [BurstCompile]
    void OnCreate(ref SystemState state) {
    }

    [BurstCompile]
    void OnUpdate(ref SystemState state)
    {
        PhysicsWorldSingleton physicsWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>();
        foreach(var input in SystemAPI.Query<DynamicBuffer<ClickedPosition>>()) {
            foreach(var placementInput in input) {
                if(physicsWorld.CastRay(placementInput.value, out var hit)) {
                    Debug.Log($"{hit.Entity.Index}");
                }
            }
            input.Clear();
        }
    }
}