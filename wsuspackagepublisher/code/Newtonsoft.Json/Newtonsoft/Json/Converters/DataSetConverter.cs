// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.DataSetConverter
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: C:\Users\Administrator.THEFLIGHTSIMS\Downloads\Release.v1.4.2203.19\Newtonsoft.Json.dll

using Newtonsoft.Json.Serialization;
using System;
using System.Data;


#nullable enable
namespace Newtonsoft.Json.Converters
{
  public class DataSetConverter : JsonConverter
  {
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
      if (value == null)
      {
        writer.WriteNull();
      }
      else
      {
        DataSet dataSet = (DataSet) value;
        DefaultContractResolver contractResolver = serializer.ContractResolver as DefaultContractResolver;
        DataTableConverter dataTableConverter = new DataTableConverter();
        writer.WriteStartObject();
        foreach (DataTable table in (InternalDataCollectionBase) dataSet.Tables)
        {
          writer.WritePropertyName(contractResolver != null ? contractResolver.GetResolvedPropertyName(table.TableName) : table.TableName);
          dataTableConverter.WriteJson(writer, (object) table, serializer);
        }
        writer.WriteEndObject();
      }
    }

    public override object? ReadJson(
      JsonReader reader,
      Type objectType,
      object? existingValue,
      JsonSerializer serializer)
    {
      if (reader.TokenType == JsonToken.Null)
        return (object) null;
      DataSet dataSet = objectType == typeof (DataSet) ? new DataSet() : (DataSet) Activator.CreateInstance(objectType);
      DataTableConverter dataTableConverter = new DataTableConverter();
      reader.ReadAndAssert();
      while (reader.TokenType == JsonToken.PropertyName)
      {
        DataTable table1 = dataSet.Tables[(string) reader.Value];
        int num = table1 != null ? 1 : 0;
        DataTable table2 = (DataTable) dataTableConverter.ReadJson(reader, typeof (DataTable), (object) table1, serializer);
        if (num == 0)
          dataSet.Tables.Add(table2);
        reader.ReadAndAssert();
      }
      return (object) dataSet;
    }

    public override bool CanConvert(Type valueType) => typeof (DataSet).IsAssignableFrom(valueType);
  }
}
