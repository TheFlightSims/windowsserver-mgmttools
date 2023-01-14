﻿// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Schema.JsonSchema
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: C:\Users\Administrator.THEFLIGHTSIMS\Downloads\Release.v1.4.2203.19\Newtonsoft.Json.dll

using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Newtonsoft.Json.Schema
{
  [Obsolete("JSON Schema validation has been moved to its own package. See https://www.newtonsoft.com/jsonschema for more details.")]
  public class JsonSchema
  {
    private readonly string _internalId = Guid.NewGuid().ToString("N");

    public string Id { get; set; }

    public string Title { get; set; }

    public bool? Required { get; set; }

    public bool? ReadOnly { get; set; }

    public bool? Hidden { get; set; }

    public bool? Transient { get; set; }

    public string Description { get; set; }

    public JsonSchemaType? Type { get; set; }

    public string Pattern { get; set; }

    public int? MinimumLength { get; set; }

    public int? MaximumLength { get; set; }

    public double? DivisibleBy { get; set; }

    public double? Minimum { get; set; }

    public double? Maximum { get; set; }

    public bool? ExclusiveMinimum { get; set; }

    public bool? ExclusiveMaximum { get; set; }

    public int? MinimumItems { get; set; }

    public int? MaximumItems { get; set; }

    public IList<JsonSchema> Items { get; set; }

    public bool PositionalItemsValidation { get; set; }

    public JsonSchema AdditionalItems { get; set; }

    public bool AllowAdditionalItems { get; set; }

    public bool UniqueItems { get; set; }

    public IDictionary<string, JsonSchema> Properties { get; set; }

    public JsonSchema AdditionalProperties { get; set; }

    public IDictionary<string, JsonSchema> PatternProperties { get; set; }

    public bool AllowAdditionalProperties { get; set; }

    public string Requires { get; set; }

    public IList<JToken> Enum { get; set; }

    public JsonSchemaType? Disallow { get; set; }

    public JToken Default { get; set; }

    public IList<JsonSchema> Extends { get; set; }

    public string Format { get; set; }

    internal string Location { get; set; }

    internal string InternalId => this._internalId;

    internal string DeferredReference { get; set; }

    internal bool ReferencesResolved { get; set; }

    public JsonSchema()
    {
      this.AllowAdditionalProperties = true;
      this.AllowAdditionalItems = true;
    }

    public static JsonSchema Read(JsonReader reader) => JsonSchema.Read(reader, new JsonSchemaResolver());

    public static JsonSchema Read(JsonReader reader, JsonSchemaResolver resolver)
    {
      ValidationUtils.ArgumentNotNull((object) reader, nameof (reader));
      ValidationUtils.ArgumentNotNull((object) resolver, nameof (resolver));
      return new JsonSchemaBuilder(resolver).Read(reader);
    }

    public static JsonSchema Parse(string json) => JsonSchema.Parse(json, new JsonSchemaResolver());

    public static JsonSchema Parse(string json, JsonSchemaResolver resolver)
    {
      ValidationUtils.ArgumentNotNull((object) json, nameof (json));
      using (JsonReader reader = (JsonReader) new JsonTextReader((TextReader) new StringReader(json)))
        return JsonSchema.Read(reader, resolver);
    }

    public void WriteTo(JsonWriter writer) => this.WriteTo(writer, new JsonSchemaResolver());

    public void WriteTo(JsonWriter writer, JsonSchemaResolver resolver)
    {
      ValidationUtils.ArgumentNotNull((object) writer, nameof (writer));
      ValidationUtils.ArgumentNotNull((object) resolver, nameof (resolver));
      new JsonSchemaWriter(writer, resolver).WriteSchema(this);
    }

    public override string ToString()
    {
      StringWriter stringWriter = new StringWriter((IFormatProvider) CultureInfo.InvariantCulture);
      JsonTextWriter writer = new JsonTextWriter((TextWriter) stringWriter);
      writer.Formatting = Formatting.Indented;
      this.WriteTo((JsonWriter) writer);
      return stringWriter.ToString();
    }
  }
}
