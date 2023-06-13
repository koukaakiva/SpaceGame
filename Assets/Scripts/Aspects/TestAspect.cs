using Unity.Entities;

public readonly partial struct TestAspect : IAspect {
    public readonly Entity entity;
    private readonly RefRO<ControllerProperties> _controllerProperties;
    private readonly RefRW<RandomNumberGenerator> _randomNumberGenerator;
}
