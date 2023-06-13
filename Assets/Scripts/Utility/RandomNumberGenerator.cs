using Unity.Entities;
using Unity.Mathematics;

public struct RandomNumberGenerator : IComponentData {
    public Random value;
}
