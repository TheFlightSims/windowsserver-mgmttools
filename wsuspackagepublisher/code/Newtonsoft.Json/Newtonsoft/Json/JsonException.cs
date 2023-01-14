// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.JsonException
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: C:\Users\Administrator.THEFLIGHTSIMS\Downloads\Release.v1.4.2203.19\Newtonsoft.Json.dll

using System;
using System.Runtime.Serialization;


#nullable enable
namespace Newtonsoft.Json
{
  [Serializable]
  public class JsonException : Exception
  {
    public JsonException()
    {
    }

    public JsonException(string message)
      : base(message)
    {
    }

    public JsonException(string message, Exception? innerException)
      : base(message, innerException)
    {
    }

    public JsonException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }

    internal static JsonException Create(IJsonLineInfo lineInfo, string path, string message)
    {
      message = JsonPosition.FormatMessage(lineInfo, path, message);
      return new JsonException(message);
    }
  }
}
