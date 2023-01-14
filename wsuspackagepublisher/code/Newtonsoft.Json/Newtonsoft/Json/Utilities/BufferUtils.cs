// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Utilities.BufferUtils
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: C:\Users\Administrator.THEFLIGHTSIMS\Downloads\Release.v1.4.2203.19\Newtonsoft.Json.dll


#nullable enable
namespace Newtonsoft.Json.Utilities
{
  internal static class BufferUtils
  {
    public static char[] RentBuffer(IArrayPool<char>? bufferPool, int minSize) => bufferPool == null ? new char[minSize] : bufferPool.Rent(minSize);

    public static void ReturnBuffer(IArrayPool<char>? bufferPool, char[]? buffer) => bufferPool?.Return(buffer);

    public static char[] EnsureBufferSize(IArrayPool<char>? bufferPool, int size, char[]? buffer)
    {
      if (bufferPool == null)
        return new char[size];
      if (buffer != null)
        bufferPool.Return(buffer);
      return bufferPool.Rent(size);
    }
  }
}
