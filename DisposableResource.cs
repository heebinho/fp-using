using System;
using Xunit;
using static F;
using Unit = System.ValueTuple;


public class DisposableResource : IDisposable
{
    private bool disposed = false;

    public void Dispose()
    {
        Console.WriteLine($"Dispose: {T}");
        disposed = true;
    }
    public void Do() => TryWrite();

    void TryWrite()
    {
        if (disposed) throw new ObjectDisposedException("object disposed.");
        Console.WriteLine($"Write: {T}");
    }

    public long T => DateTime.Now.Ticks;
}

public class DisposableResourceTest
{
    [Fact]
    public void ShouldDisposeResource()
    {
        var resource = new DisposableResource();
        Action a = () => resource.Do();
        //Classic using statement to illustrate that the Dispose() method is called after the using block
        using (resource){ a(); }
        Assert.Throws<ObjectDisposedException>(a);
    }

    [Fact]
    public void ShouldDisposeResourceAfterUsingFunction()
    {
        var resource = new DisposableResource();
        Action<DisposableResource> a = (r) => resource.Do();
        //An extension method which turns an Action<T> into a Func<T, R>
        Func<DisposableResource, Unit> f = a.ToFunc(); 

        //Now we can use our Using HOF as intended
        Using(resource, f); 

        //The resource is disposed and should throw an ObjectDisposedException
        Assert.Throws<ObjectDisposedException>(()=>resource.Do());
    }
}