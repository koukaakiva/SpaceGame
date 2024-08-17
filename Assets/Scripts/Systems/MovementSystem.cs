using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Physics;


[BurstCompile]
public partial struct MovementSystem : ISystem {

    //[BurstCompile]
    //void OnUpdate(ref SystemState state) {
    //    foreach ((RefRW<LocalTransform> transform, RefRW<PhysicsVelocity> velocity, RefRO<Movement> movement)
    //        in SystemAPI.Query<RefRW<LocalTransform>, RefRW<PhysicsVelocity>, RefRO<Movement>>()) {

    //        //float3 movementDirection = math.normalize(movement.ValueRO.destination - transform.ValueRO.Position);
    //        float3 movementDirection = math.normalize(new float3(-10, 0, 0) - transform.ValueRO.Position);
    //        transform.ValueRW.Rotation = quaternion.LookRotation(movementDirection, math.up());
    //        //transform.ValueRW.Position += movementDirection * SystemAPI.Time.DeltaTime;
    //        velocity.ValueRW.Linear = movementDirection * 5;
    //        velocity.ValueRW.Angular = float3.zero;
    //    }
    //}

    [BurstCompile]
    void OnUpdate(ref SystemState state) {
        state.Dependency = new MovementJob {
            deltaTime = state.WorldUnmanaged.Time.DeltaTime,
        }.ScheduleParallel(state.Dependency);
    }

    [BurstCompile]
    public partial struct MovementJob : IJobEntity {
        public float deltaTime;

        //private void Execute(ref LocalTransform transform, ref Movement movement, ref RandomNumberGenerator rng) {
        private void Execute(ref LocalTransform transform, ref Movement movement) {
                float3 delta = movement.destination - transform.Position;
            float dist = math.sqrt(delta.x * delta.x + delta.y * delta.y + delta.z * delta.z);
            if (dist <= 0.01f) {
                //movement.destination = new float3(rng.value.NextInt(-5, 5), 0, rng.value.NextInt(-5, 5));
                movement.destination = new float3(-5, 0, 5);
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

