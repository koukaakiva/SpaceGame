using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;
using Unity.Jobs;
using System;

[BurstCompile]
public partial struct MovementSystem : ISystem {

    void OnCreate(ref SystemState state) { }

    void OnDestroy(ref SystemState state) { }

    [BurstCompile]
    void OnUpdate(ref SystemState state) {
        state.Dependency = new MovementJob {
            deltaTime = state.WorldUnmanaged.Time.DeltaTime,
        }.ScheduleParallel(state.Dependency);
    }

    [BurstCompile]
    public partial struct MovementJob : IJobEntity {
        public float deltaTime;

        private void Execute(ref LocalTransform transform, ref Movement movement, ref RandomNumberGenerator rng) {
            float3 delta = movement.destination - transform.Position;
            float dist = math.sqrt(delta.x * delta.x + delta.y * delta.y + delta.z * delta.z);
            if(dist <= 0.01f) {
                movement.destination = new float3(rng.value.NextInt(-5, 5), 0, rng.value.NextInt(-5, 5));
            }

            movement.velocity = delta;

            //var rotation = transform.Rotation;
            //var targetRotation = quaternion.LookRotation(math.normalize(movement.velocity), Vector3.up);
            //rotation = math.nlerp(rotation, targetRotation, deltaTime * 4);
            //transform.Rotation = rotation;

            transform.Position += movement.velocity * deltaTime;
        }
    }
}

