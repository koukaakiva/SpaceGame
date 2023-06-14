using Unity.Entities;
using Unity.Mathematics;

public struct Movement : IComponentData {
    public float3 velocity;
    public float3 destination;
}
