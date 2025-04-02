using Arch.Bus;
using Foster.Framework;

namespace Content.Test;

public struct SomeTest
{
    public string name;

    public SomeTest(string something)
    {
        this.name = something;
    }
}

public static class SomeTestHandler
{
    [Event(order:0)]
    public static void OnSomeTest(ref SomeTest test)
    {
        Log.Info(test.name);
        
    }
    
    [Event(order:1)]
    public static void OnSomeTest1(ref SomeTest test)
    {
        Log.Info("测试");
    }
}

