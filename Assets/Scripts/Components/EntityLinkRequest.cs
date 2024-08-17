using Unity.Entities;

public struct EntityLinkRequest : IComponentData {
    public LinkingMode mode;
    public int hash;
}

public enum LinkingMode {
    Parent,
    Child
}