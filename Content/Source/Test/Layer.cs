namespace Content.Test;

public enum ELayer
{
    Line,
    Building,
    Frog
}

public static class ELayerExt
{
    public static int GetId(this ELayer layer)=>(int)layer;
}