// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.JRaw
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: C:\Users\Administrator.THEFLIGHTSIMS\Downloads\Release.v1.4.2203.19\Newtonsoft.Json.dll

using System;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;


#nullable enable
namespace Newtonsoft.Json.Linq
{
  public class JRaw : JValue
  {
    public static async Task<JRaw> CreateAsync(
      JsonReader reader,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      JRaw async;
      using (StringWriter sw = new StringWriter((IFormatProvider) CultureInfo.InvariantCulture))
      {
        using (JsonTextWriter jsonWriter = new JsonTextWriter((TextWriter) sw))
        {
          await jsonWriter.WriteTokenSyncReadingAsync(reader, cancellationToken).ConfigureAwait(false);
          async = new JRaw((object) sw.ToString());
        }
      }
      return async;
    }

    public JRaw(JRaw other)
      : base((JValue) other)
    {
    }

    public JRaw(object? rawJson)
      : base(rawJson, JTokenType.Raw)
    {
    }

    public static JRaw Create(JsonReader reader)
    {
      using (StringWriter stringWriter = new StringWriter((IFormatProvider) CultureInfo.InvariantCulture))
      {
        using (JsonTextWriter jsonTextWriter = new JsonTextWriter((TextWriter) stringWriter))
        {
          jsonTextWriter.WriteToken(reader);
          return new JRaw((object) stringWriter.ToString());
        }
      }
    }

    internal override JToken CloneToken() => (JToken) new JRaw(this);
  }
}
