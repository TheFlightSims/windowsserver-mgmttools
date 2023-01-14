// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Utilities.FSharpFunction
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: C:\Users\Administrator.THEFLIGHTSIMS\Downloads\Release.v1.4.2203.19\Newtonsoft.Json.dll


#nullable enable
namespace Newtonsoft.Json.Utilities
{
  internal class FSharpFunction
  {
    private readonly object? _instance;
    private readonly MethodCall<object?, object> _invoker;

    public FSharpFunction(object? instance, MethodCall<object?, object> invoker)
    {
      this._instance = instance;
      this._invoker = invoker;
    }

    public object Invoke(params object[] args) => this._invoker(this._instance, args);
  }
}
