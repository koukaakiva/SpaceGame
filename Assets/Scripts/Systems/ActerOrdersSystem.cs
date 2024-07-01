#if false

using Unity.Entities;
using Unity.Burst;
using Unity.Physics;
using Unity.Physics.Systems;
using UnityEngine;
using Unity.Collections;
using Unity.Mathematics;
using System;

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateAfter(typeof(PhysicsSystemGroup))]
[BurstCompile]
public partial struct ActerOrdersSystem : ISystem {

	private EntityCommandBuffer ecb;

	[BurstCompile]
	void OnUpdate(ref SystemState state) {
		PhysicsWorldSingleton physicsWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>();
		ecb = new EntityCommandBuffer(Allocator.Temp);
		foreach (var input in SystemAPI.Query<DynamicBuffer<ClickedPosition>>()) {
			foreach (var placementInput in input) {
				if (physicsWorld.CastRay(placementInput.value, out var hit)) {
					Debug.Log($"{hit.Entity.Index}");
					try { //TODO: Hacky. Check if the entity has the movement component instead.
						ecb.SetComponent(hit.Entity, new Movement {
							velocity = float3.zero,
							destination = new float3(0, 5, 0)
						});
					} catch (Exception) { };
				}
			}
			input.Clear();
		}
		ecb.Playback(state.EntityManager);
	}
}

#endif