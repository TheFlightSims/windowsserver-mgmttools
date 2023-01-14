// Decompiled with JetBrains decompiler
// Type: Microsoft.Deployment.Compression.Cab.HandleManager`1
// Assembly: Microsoft.Deployment.Compression.Cab, Version=3.0.0.0, Culture=neutral, PublicKeyToken=ce35f76fcda82bad
// MVID: FD30E6CD-702E-43F8-86E3-5A8B35FB59B3
// Assembly location: C:\Users\Administrator.THEFLIGHTSIMS\Downloads\Release.v1.4.2203.19\Microsoft.Deployment.Compression.Cab.dll

using System.Collections.Generic;

namespace Microsoft.Deployment.Compression.Cab
{
  internal sealed class HandleManager<T> where T : class
  {
    private List<T> handles;

    public HandleManager() => this.handles = new List<T>();

    public T this[int handle] => handle > 0 && handle <= this.handles.Count ? this.handles[checked (handle - 1)] : default (T);

    public int AllocHandle(T obj)
    {
      this.handles.Add(obj);
      return this.handles.Count;
    }

    public void FreeHandle(int handle)
    {
      if (handle <= 0 || handle > this.handles.Count)
        return;
      this.handles[checked (handle - 1)] = default (T);
    }
  }
}
