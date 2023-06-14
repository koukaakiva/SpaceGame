using Unity.Entities;
using Unity.Transforms;

public readonly partial struct TestAspect : IAspect {
    public readonly Entity entity;
    private readonly RefRO<LocalTransform> _localTransform;
}
