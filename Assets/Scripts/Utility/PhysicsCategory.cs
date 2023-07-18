public static class PhysicsCategory {
    public readonly static uint All = ~0u;
    public readonly static uint Acter = 1u << 0;
    public readonly static uint Ground = 1u << 1;
    //public readonly static uint Dynamic = 1u << 2;
    //public readonly static uint Interactor = 1u << 3;
    //public readonly static uint Interactable = 1u << 4;
    public static uint Not(uint value) => ~value;
}