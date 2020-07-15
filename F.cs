using System;
using Unit = System.ValueTuple;
using static F;

public static class F{

    public static R Using<TDisp, R>(TDisp disposable
        , Func<TDisp, R> func) where TDisp : IDisposable
    {
        using (disposable) return func(disposable);
    }

    public static R UsingF<TDisp, R>(TDisp disposable
        , Func<TDisp, R> func) where TDisp : IDisposable
    {
        try
        {
            return func(disposable);
        }
        finally
        {
            disposable?.Dispose();
        }
    }

    public static Unit Unit() => default(Unit);

}

public static class ActionExtensions{
    public static Func<Unit> ToFunc(this Action action)
        => () => { action(); return Unit(); };
    
    public static Func<T, Unit> ToFunc<T>(this Action<T> action)
        => (t) => { action(t); return Unit(); };
}