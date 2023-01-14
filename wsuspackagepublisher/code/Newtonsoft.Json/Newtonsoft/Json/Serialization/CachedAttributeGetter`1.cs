// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.CachedAttributeGetter`1
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: C:\Users\Administrator.THEFLIGHTSIMS\Downloads\Release.v1.4.2203.19\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System;


#nullable enable
namespace Newtonsoft.Json.Serialization
{
  internal static class CachedAttributeGetter<T> where T : Attribute
  {
    private static readonly ThreadSafeStore<object, T?> TypeAttributeCache = new ThreadSafeStore<object, T>(new Func<object, T>(JsonTypeReflector.GetAttribute<T>));

    public static T? GetAttribute(object type) => CachedAttributeGetter<T>.TypeAttributeCache.Get(type);
  }
}
