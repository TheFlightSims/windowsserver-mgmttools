// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.JsonTextWriter
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: C:\Users\Administrator.THEFLIGHTSIMS\Downloads\Release.v1.4.2203.19\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System;
using System.Globalization;
using System.IO;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;


#nullable enable
namespace Newtonsoft.Json
{
  public class JsonTextWriter : JsonWriter
  {
    private readonly bool _safeAsync;
    private const int IndentCharBufferSize = 12;
    private readonly TextWriter _writer;
    private Base64Encoder? _base64Encoder;
    private char _indentChar;
    private int _indentation;
    private char _quoteChar;
    private bool _quoteName;
    private bool[]? _charEscapeFlags;
    private char[]? _writeBuffer;
    private IArrayPool<char>? _arrayPool;
    private char[]? _indentChars;

    public override Task FlushAsync(CancellationToken cancellationToken = default (CancellationToken)) => !this._safeAsync ? base.FlushAsync(cancellationToken) : this.DoFlushAsync(cancellationToken);

    internal Task DoFlushAsync(CancellationToken cancellationToken) => cancellationToken.CancelIfRequestedAsync() ?? this._writer.FlushAsync();

    protected override Task WriteValueDelimiterAsync(CancellationToken cancellationToken) => !this._safeAsync ? base.WriteValueDelimiterAsync(cancellationToken) : this.DoWriteValueDelimiterAsync(cancellationToken);

    internal Task DoWriteValueDelimiterAsync(CancellationToken cancellationToken) => this._writer.WriteAsync(',', cancellationToken);

    protected override Task WriteEndAsync(JsonToken token, CancellationToken cancellationToken) => !this._safeAsync ? base.WriteEndAsync(token, cancellationToken) : this.DoWriteEndAsync(token, cancellationToken);

    internal Task DoWriteEndAsync(JsonToken token, CancellationToken cancellationToken)
    {
      switch (token)
      {
        case JsonToken.EndObject:
          return this._writer.WriteAsync('}', cancellationToken);
        case JsonToken.EndArray:
          return this._writer.WriteAsync(']', cancellationToken);
        case JsonToken.EndConstructor:
          return this._writer.WriteAsync(')', cancellationToken);
        default:
          throw JsonWriterException.Create((JsonWriter) this, "Invalid JsonToken: " + token.ToString(), (Exception) null);
      }
    }

    public override Task CloseAsync(CancellationToken cancellationToken = default (CancellationToken)) => !this._safeAsync ? base.CloseAsync(cancellationToken) : this.DoCloseAsync(cancellationToken);

    internal async Task DoCloseAsync(CancellationToken cancellationToken)
    {
      JsonTextWriter jsonTextWriter = this;
      if (jsonTextWriter.Top == 0)
        cancellationToken.ThrowIfCancellationRequested();
      while (jsonTextWriter.Top > 0)
        await jsonTextWriter.WriteEndAsync(cancellationToken).ConfigureAwait(false);
      jsonTextWriter.CloseBufferAndWriter();
    }

    public override Task WriteEndAsync(CancellationToken cancellationToken = default (CancellationToken)) => !this._safeAsync ? base.WriteEndAsync(cancellationToken) : this.WriteEndInternalAsync(cancellationToken);

    protected override Task WriteIndentAsync(CancellationToken cancellationToken) => !this._safeAsync ? base.WriteIndentAsync(cancellationToken) : this.DoWriteIndentAsync(cancellationToken);

    internal Task DoWriteIndentAsync(CancellationToken cancellationToken)
    {
      int currentIndentCount = this.Top * this._indentation;
      int newLineLen = this.SetIndentChars();
      return currentIndentCount <= 12 ? this._writer.WriteAsync(this._indentChars, 0, newLineLen + currentIndentCount, cancellationToken) : this.WriteIndentAsync(currentIndentCount, newLineLen, cancellationToken);
    }

    private async Task WriteIndentAsync(
      int currentIndentCount,
      int newLineLen,
      CancellationToken cancellationToken)
    {
      await this._writer.WriteAsync(this._indentChars, 0, newLineLen + Math.Min(currentIndentCount, 12), cancellationToken).ConfigureAwait(false);
      while ((currentIndentCount -= 12) > 0)
        await this._writer.WriteAsync(this._indentChars, newLineLen, Math.Min(currentIndentCount, 12), cancellationToken).ConfigureAwait(false);
    }

    private Task WriteValueInternalAsync(
      JsonToken token,
      string value,
      CancellationToken cancellationToken)
    {
      Task task = this.InternalWriteValueAsync(token, cancellationToken);
      return task.IsCompletedSucessfully() ? this._writer.WriteAsync(value, cancellationToken) : this.WriteValueInternalAsync(task, value, cancellationToken);
    }

    private async Task WriteValueInternalAsync(
      Task task,
      string value,
      CancellationToken cancellationToken)
    {
      ConfiguredTaskAwaitable configuredTaskAwaitable = task.ConfigureAwait(false);
      await configuredTaskAwaitable;
      configuredTaskAwaitable = this._writer.WriteAsync(value, cancellationToken).ConfigureAwait(false);
      await configuredTaskAwaitable;
    }

    protected override Task WriteIndentSpaceAsync(CancellationToken cancellationToken) => !this._safeAsync ? base.WriteIndentSpaceAsync(cancellationToken) : this.DoWriteIndentSpaceAsync(cancellationToken);

    internal Task DoWriteIndentSpaceAsync(CancellationToken cancellationToken) => this._writer.WriteAsync(' ', cancellationToken);

    public override Task WriteRawAsync(string? json, CancellationToken cancellationToken = default (CancellationToken)) => !this._safeAsync ? base.WriteRawAsync(json, cancellationToken) : this.DoWriteRawAsync(json, cancellationToken);

    internal Task DoWriteRawAsync(string? json, CancellationToken cancellationToken) => this._writer.WriteAsync(json, cancellationToken);

    public override Task WriteNullAsync(CancellationToken cancellationToken = default (CancellationToken)) => !this._safeAsync ? base.WriteNullAsync(cancellationToken) : this.DoWriteNullAsync(cancellationToken);

    internal Task DoWriteNullAsync(CancellationToken cancellationToken) => this.WriteValueInternalAsync(JsonToken.Null, JsonConvert.Null, cancellationToken);

    private Task WriteDigitsAsync(ulong uvalue, bool negative, CancellationToken cancellationToken) => uvalue <= 9UL & !negative ? this._writer.WriteAsync((char) (48UL + uvalue), cancellationToken) : this._writer.WriteAsync(this._writeBuffer, 0, this.WriteNumberToBuffer(uvalue, negative), cancellationToken);

    private Task WriteIntegerValueAsync(
      ulong uvalue,
      bool negative,
      CancellationToken cancellationToken)
    {
      Task task = this.InternalWriteValueAsync(JsonToken.Integer, cancellationToken);
      return task.IsCompletedSucessfully() ? this.WriteDigitsAsync(uvalue, negative, cancellationToken) : this.WriteIntegerValueAsync(task, uvalue, negative, cancellationToken);
    }

    private async Task WriteIntegerValueAsync(
      Task task,
      ulong uvalue,
      bool negative,
      CancellationToken cancellationToken)
    {
      ConfiguredTaskAwaitable configuredTaskAwaitable = task.ConfigureAwait(false);
      await configuredTaskAwaitable;
      configuredTaskAwaitable = this.WriteDigitsAsync(uvalue, negative, cancellationToken).ConfigureAwait(false);
      await configuredTaskAwaitable;
    }

    internal Task WriteIntegerValueAsync(long value, CancellationToken cancellationToken)
    {
      bool negative = value < 0L;
      if (negative)
        value = -value;
      return this.WriteIntegerValueAsync((ulong) value, negative, cancellationToken);
    }

    internal Task WriteIntegerValueAsync(ulong uvalue, CancellationToken cancellationToken) => this.WriteIntegerValueAsync(uvalue, false, cancellationToken);

    private Task WriteEscapedStringAsync(
      string value,
      bool quote,
      CancellationToken cancellationToken)
    {
      return JavaScriptUtils.WriteEscapedJavaScriptStringAsync(this._writer, value, this._quoteChar, quote, this._charEscapeFlags, this.StringEscapeHandling, this, this._writeBuffer, cancellationToken);
    }

    public override Task WritePropertyNameAsync(string name, CancellationToken cancellationToken = default (CancellationToken)) => !this._safeAsync ? base.WritePropertyNameAsync(name, cancellationToken) : this.DoWritePropertyNameAsync(name, cancellationToken);

    internal Task DoWritePropertyNameAsync(string name, CancellationToken cancellationToken)
    {
      Task task1 = this.InternalWritePropertyNameAsync(name, cancellationToken);
      if (!task1.IsCompletedSucessfully())
        return this.DoWritePropertyNameAsync(task1, name, cancellationToken);
      Task task2 = this.WriteEscapedStringAsync(name, this._quoteName, cancellationToken);
      return task2.IsCompletedSucessfully() ? this._writer.WriteAsync(':', cancellationToken) : JavaScriptUtils.WriteCharAsync(task2, this._writer, ':', cancellationToken);
    }

    private async Task DoWritePropertyNameAsync(
      Task task,
      string name,
      CancellationToken cancellationToken)
    {
      ConfiguredTaskAwaitable configuredTaskAwaitable = task.ConfigureAwait(false);
      await configuredTaskAwaitable;
      configuredTaskAwaitable = this.WriteEscapedStringAsync(name, this._quoteName, cancellationToken).ConfigureAwait(false);
      await configuredTaskAwaitable;
      configuredTaskAwaitable = this._writer.WriteAsync(':').ConfigureAwait(false);
      await configuredTaskAwaitable;
    }

    public override Task WritePropertyNameAsync(
      string name,
      bool escape,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      return !this._safeAsync ? base.WritePropertyNameAsync(name, escape, cancellationToken) : this.DoWritePropertyNameAsync(name, escape, cancellationToken);
    }

    internal async Task DoWritePropertyNameAsync(
      string name,
      bool escape,
      CancellationToken cancellationToken)
    {
      JsonTextWriter jsonTextWriter = this;
      await jsonTextWriter.InternalWritePropertyNameAsync(name, cancellationToken).ConfigureAwait(false);
      ConfiguredTaskAwaitable configuredTaskAwaitable;
      if (escape)
      {
        await jsonTextWriter.WriteEscapedStringAsync(name, jsonTextWriter._quoteName, cancellationToken).ConfigureAwait(false);
      }
      else
      {
        if (jsonTextWriter._quoteName)
        {
          configuredTaskAwaitable = jsonTextWriter._writer.WriteAsync(jsonTextWriter._quoteChar).ConfigureAwait(false);
          await configuredTaskAwaitable;
        }
        configuredTaskAwaitable = jsonTextWriter._writer.WriteAsync(name, cancellationToken).ConfigureAwait(false);
        await configuredTaskAwaitable;
        if (jsonTextWriter._quoteName)
        {
          configuredTaskAwaitable = jsonTextWriter._writer.WriteAsync(jsonTextWriter._quoteChar).ConfigureAwait(false);
          await configuredTaskAwaitable;
        }
      }
      configuredTaskAwaitable = jsonTextWriter._writer.WriteAsync(':').ConfigureAwait(false);
      await configuredTaskAwaitable;
    }

    public override Task WriteStartArrayAsync(CancellationToken cancellationToken = default (CancellationToken)) => !this._safeAsync ? base.WriteStartArrayAsync(cancellationToken) : this.DoWriteStartArrayAsync(cancellationToken);

    internal Task DoWriteStartArrayAsync(CancellationToken cancellationToken)
    {
      Task task = this.InternalWriteStartAsync(JsonToken.StartArray, JsonContainerType.Array, cancellationToken);
      return task.IsCompletedSucessfully() ? this._writer.WriteAsync('[', cancellationToken) : this.DoWriteStartArrayAsync(task, cancellationToken);
    }

    internal async Task DoWriteStartArrayAsync(Task task, CancellationToken cancellationToken)
    {
      ConfiguredTaskAwaitable configuredTaskAwaitable = task.ConfigureAwait(false);
      await configuredTaskAwaitable;
      configuredTaskAwaitable = this._writer.WriteAsync('[', cancellationToken).ConfigureAwait(false);
      await configuredTaskAwaitable;
    }

    public override Task WriteStartObjectAsync(CancellationToken cancellationToken = default (CancellationToken)) => !this._safeAsync ? base.WriteStartObjectAsync(cancellationToken) : this.DoWriteStartObjectAsync(cancellationToken);

    internal Task DoWriteStartObjectAsync(CancellationToken cancellationToken)
    {
      Task task = this.InternalWriteStartAsync(JsonToken.StartObject, JsonContainerType.Object, cancellationToken);
      return task.IsCompletedSucessfully() ? this._writer.WriteAsync('{', cancellationToken) : this.DoWriteStartObjectAsync(task, cancellationToken);
    }

    internal async Task DoWriteStartObjectAsync(Task task, CancellationToken cancellationToken)
    {
      ConfiguredTaskAwaitable configuredTaskAwaitable = task.ConfigureAwait(false);
      await configuredTaskAwaitable;
      configuredTaskAwaitable = this._writer.WriteAsync('{', cancellationToken).ConfigureAwait(false);
      await configuredTaskAwaitable;
    }

    public override Task WriteStartConstructorAsync(
      string name,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      return !this._safeAsync ? base.WriteStartConstructorAsync(name, cancellationToken) : this.DoWriteStartConstructorAsync(name, cancellationToken);
    }

    internal async Task DoWriteStartConstructorAsync(
      string name,
      CancellationToken cancellationToken)
    {
      JsonTextWriter jsonTextWriter = this;
      await jsonTextWriter.InternalWriteStartAsync(JsonToken.StartConstructor, JsonContainerType.Constructor, cancellationToken).ConfigureAwait(false);
      await jsonTextWriter._writer.WriteAsync("new ", cancellationToken).ConfigureAwait(false);
      await jsonTextWriter._writer.WriteAsync(name, cancellationToken).ConfigureAwait(false);
      await jsonTextWriter._writer.WriteAsync('(').ConfigureAwait(false);
    }

    public override Task WriteUndefinedAsync(CancellationToken cancellationToken = default (CancellationToken)) => !this._safeAsync ? base.WriteUndefinedAsync(cancellationToken) : this.DoWriteUndefinedAsync(cancellationToken);

    internal Task DoWriteUndefinedAsync(CancellationToken cancellationToken)
    {
      Task task = this.InternalWriteValueAsync(JsonToken.Undefined, cancellationToken);
      return task.IsCompletedSucessfully() ? this._writer.WriteAsync(JsonConvert.Undefined, cancellationToken) : this.DoWriteUndefinedAsync(task, cancellationToken);
    }

    private async Task DoWriteUndefinedAsync(Task task, CancellationToken cancellationToken)
    {
      ConfiguredTaskAwaitable configuredTaskAwaitable = task.ConfigureAwait(false);
      await configuredTaskAwaitable;
      configuredTaskAwaitable = this._writer.WriteAsync(JsonConvert.Undefined, cancellationToken).ConfigureAwait(false);
      await configuredTaskAwaitable;
    }

    public override Task WriteWhitespaceAsync(string ws, CancellationToken cancellationToken = default (CancellationToken)) => !this._safeAsync ? base.WriteWhitespaceAsync(ws, cancellationToken) : this.DoWriteWhitespaceAsync(ws, cancellationToken);

    internal Task DoWriteWhitespaceAsync(string ws, CancellationToken cancellationToken)
    {
      this.InternalWriteWhitespace(ws);
      return this._writer.WriteAsync(ws, cancellationToken);
    }

    public override Task WriteValueAsync(bool value, CancellationToken cancellationToken = default (CancellationToken)) => !this._safeAsync ? base.WriteValueAsync(value, cancellationToken) : this.DoWriteValueAsync(value, cancellationToken);

    internal Task DoWriteValueAsync(bool value, CancellationToken cancellationToken) => this.WriteValueInternalAsync(JsonToken.Boolean, JsonConvert.ToString(value), cancellationToken);

    public override Task WriteValueAsync(bool? value, CancellationToken cancellationToken = default (CancellationToken)) => !this._safeAsync ? base.WriteValueAsync(value, cancellationToken) : this.DoWriteValueAsync(value, cancellationToken);

    internal Task DoWriteValueAsync(bool? value, CancellationToken cancellationToken) => value.HasValue ? this.DoWriteValueAsync(value.GetValueOrDefault(), cancellationToken) : this.DoWriteNullAsync(cancellationToken);

    public override Task WriteValueAsync(byte value, CancellationToken cancellationToken = default (CancellationToken)) => !this._safeAsync ? base.WriteValueAsync(value, cancellationToken) : this.WriteIntegerValueAsync((long) value, cancellationToken);

    public override Task WriteValueAsync(byte? value, CancellationToken cancellationToken = default (CancellationToken)) => !this._safeAsync ? base.WriteValueAsync(value, cancellationToken) : this.DoWriteValueAsync(value, cancellationToken);

    internal Task DoWriteValueAsync(byte? value, CancellationToken cancellationToken) => value.HasValue ? this.WriteIntegerValueAsync((long) value.GetValueOrDefault(), cancellationToken) : this.DoWriteNullAsync(cancellationToken);

    public override Task WriteValueAsync(byte[]? value, CancellationToken cancellationToken = default (CancellationToken))
    {
      if (!this._safeAsync)
        return base.WriteValueAsync(value, cancellationToken);
      return value != null ? this.WriteValueNonNullAsync(value, cancellationToken) : this.WriteNullAsync(cancellationToken);
    }

    internal async Task WriteValueNonNullAsync(byte[] value, CancellationToken cancellationToken)
    {
      JsonTextWriter jsonTextWriter = this;
      ConfiguredTaskAwaitable configuredTaskAwaitable = jsonTextWriter.InternalWriteValueAsync(JsonToken.Bytes, cancellationToken).ConfigureAwait(false);
      await configuredTaskAwaitable;
      configuredTaskAwaitable = jsonTextWriter._writer.WriteAsync(jsonTextWriter._quoteChar).ConfigureAwait(false);
      await configuredTaskAwaitable;
      configuredTaskAwaitable = jsonTextWriter.Base64Encoder.EncodeAsync(value, 0, value.Length, cancellationToken).ConfigureAwait(false);
      await configuredTaskAwaitable;
      configuredTaskAwaitable = jsonTextWriter.Base64Encoder.FlushAsync(cancellationToken).ConfigureAwait(false);
      await configuredTaskAwaitable;
      configuredTaskAwaitable = jsonTextWriter._writer.WriteAsync(jsonTextWriter._quoteChar).ConfigureAwait(false);
      await configuredTaskAwaitable;
    }

    public override Task WriteValueAsync(char value, CancellationToken cancellationToken = default (CancellationToken)) => !this._safeAsync ? base.WriteValueAsync(value, cancellationToken) : this.DoWriteValueAsync(value, cancellationToken);

    internal Task DoWriteValueAsync(char value, CancellationToken cancellationToken) => this.WriteValueInternalAsync(JsonToken.String, JsonConvert.ToString(value), cancellationToken);

    public override Task WriteValueAsync(char? value, CancellationToken cancellationToken = default (CancellationToken)) => !this._safeAsync ? base.WriteValueAsync(value, cancellationToken) : this.DoWriteValueAsync(value, cancellationToken);

    internal Task DoWriteValueAsync(char? value, CancellationToken cancellationToken) => value.HasValue ? this.DoWriteValueAsync(value.GetValueOrDefault(), cancellationToken) : this.DoWriteNullAsync(cancellationToken);

    public override Task WriteValueAsync(DateTime value, CancellationToken cancellationToken = default (CancellationToken)) => !this._safeAsync ? base.WriteValueAsync(value, cancellationToken) : this.DoWriteValueAsync(value, cancellationToken);

    internal async Task DoWriteValueAsync(DateTime value, CancellationToken cancellationToken)
    {
      JsonTextWriter jsonTextWriter = this;
      ConfiguredTaskAwaitable configuredTaskAwaitable = jsonTextWriter.InternalWriteValueAsync(JsonToken.Date, cancellationToken).ConfigureAwait(false);
      await configuredTaskAwaitable;
      value = DateTimeUtils.EnsureDateTime(value, jsonTextWriter.DateTimeZoneHandling);
      if (StringUtils.IsNullOrEmpty(jsonTextWriter.DateFormatString))
      {
        int buffer = jsonTextWriter.WriteValueToBuffer(value);
        configuredTaskAwaitable = jsonTextWriter._writer.WriteAsync(jsonTextWriter._writeBuffer, 0, buffer, cancellationToken).ConfigureAwait(false);
        await configuredTaskAwaitable;
      }
      else
      {
        configuredTaskAwaitable = jsonTextWriter._writer.WriteAsync(jsonTextWriter._quoteChar).ConfigureAwait(false);
        await configuredTaskAwaitable;
        configuredTaskAwaitable = jsonTextWriter._writer.WriteAsync(value.ToString(jsonTextWriter.DateFormatString, (IFormatProvider) jsonTextWriter.Culture), cancellationToken).ConfigureAwait(false);
        await configuredTaskAwaitable;
        configuredTaskAwaitable = jsonTextWriter._writer.WriteAsync(jsonTextWriter._quoteChar).ConfigureAwait(false);
        await configuredTaskAwaitable;
      }
    }

    public override Task WriteValueAsync(DateTime? value, CancellationToken cancellationToken = default (CancellationToken)) => !this._safeAsync ? base.WriteValueAsync(value, cancellationToken) : this.DoWriteValueAsync(value, cancellationToken);

    internal Task DoWriteValueAsync(DateTime? value, CancellationToken cancellationToken) => value.HasValue ? this.DoWriteValueAsync(value.GetValueOrDefault(), cancellationToken) : this.DoWriteNullAsync(cancellationToken);

    public override Task WriteValueAsync(DateTimeOffset value, CancellationToken cancellationToken = default (CancellationToken)) => !this._safeAsync ? base.WriteValueAsync(value, cancellationToken) : this.DoWriteValueAsync(value, cancellationToken);

    internal async Task DoWriteValueAsync(DateTimeOffset value, CancellationToken cancellationToken)
    {
      JsonTextWriter jsonTextWriter = this;
      ConfiguredTaskAwaitable configuredTaskAwaitable = jsonTextWriter.InternalWriteValueAsync(JsonToken.Date, cancellationToken).ConfigureAwait(false);
      await configuredTaskAwaitable;
      if (StringUtils.IsNullOrEmpty(jsonTextWriter.DateFormatString))
      {
        int buffer = jsonTextWriter.WriteValueToBuffer(value);
        configuredTaskAwaitable = jsonTextWriter._writer.WriteAsync(jsonTextWriter._writeBuffer, 0, buffer, cancellationToken).ConfigureAwait(false);
        await configuredTaskAwaitable;
      }
      else
      {
        configuredTaskAwaitable = jsonTextWriter._writer.WriteAsync(jsonTextWriter._quoteChar).ConfigureAwait(false);
        await configuredTaskAwaitable;
        configuredTaskAwaitable = jsonTextWriter._writer.WriteAsync(value.ToString(jsonTextWriter.DateFormatString, (IFormatProvider) jsonTextWriter.Culture), cancellationToken).ConfigureAwait(false);
        await configuredTaskAwaitable;
        configuredTaskAwaitable = jsonTextWriter._writer.WriteAsync(jsonTextWriter._quoteChar).ConfigureAwait(false);
        await configuredTaskAwaitable;
      }
    }

    public override Task WriteValueAsync(DateTimeOffset? value, CancellationToken cancellationToken = default (CancellationToken)) => !this._safeAsync ? base.WriteValueAsync(value, cancellationToken) : this.DoWriteValueAsync(value, cancellationToken);

    internal Task DoWriteValueAsync(DateTimeOffset? value, CancellationToken cancellationToken) => value.HasValue ? this.DoWriteValueAsync(value.GetValueOrDefault(), cancellationToken) : this.DoWriteNullAsync(cancellationToken);

    public override Task WriteValueAsync(Decimal value, CancellationToken cancellationToken = default (CancellationToken)) => !this._safeAsync ? base.WriteValueAsync(value, cancellationToken) : this.DoWriteValueAsync(value, cancellationToken);

    internal Task DoWriteValueAsync(Decimal value, CancellationToken cancellationToken) => this.WriteValueInternalAsync(JsonToken.Float, JsonConvert.ToString(value), cancellationToken);

    public override Task WriteValueAsync(Decimal? value, CancellationToken cancellationToken = default (CancellationToken)) => !this._safeAsync ? base.WriteValueAsync(value, cancellationToken) : this.DoWriteValueAsync(value, cancellationToken);

    internal Task DoWriteValueAsync(Decimal? value, CancellationToken cancellationToken) => value.HasValue ? this.DoWriteValueAsync(value.GetValueOrDefault(), cancellationToken) : this.DoWriteNullAsync(cancellationToken);

    public override Task WriteValueAsync(double value, CancellationToken cancellationToken = default (CancellationToken)) => !this._safeAsync ? base.WriteValueAsync(value, cancellationToken) : this.WriteValueAsync(value, false, cancellationToken);

    internal Task WriteValueAsync(double value, bool nullable, CancellationToken cancellationToken) => this.WriteValueInternalAsync(JsonToken.Float, JsonConvert.ToString(value, this.FloatFormatHandling, this.QuoteChar, nullable), cancellationToken);

    public override Task WriteValueAsync(double? value, CancellationToken cancellationToken = default (CancellationToken))
    {
      if (!this._safeAsync)
        return base.WriteValueAsync(value, cancellationToken);
      return !value.HasValue ? this.WriteNullAsync(cancellationToken) : this.WriteValueAsync(value.GetValueOrDefault(), true, cancellationToken);
    }

    public override Task WriteValueAsync(float value, CancellationToken cancellationToken = default (CancellationToken)) => !this._safeAsync ? base.WriteValueAsync(value, cancellationToken) : this.WriteValueAsync(value, false, cancellationToken);

    internal Task WriteValueAsync(float value, bool nullable, CancellationToken cancellationToken) => this.WriteValueInternalAsync(JsonToken.Float, JsonConvert.ToString(value, this.FloatFormatHandling, this.QuoteChar, nullable), cancellationToken);

    public override Task WriteValueAsync(float? value, CancellationToken cancellationToken = default (CancellationToken))
    {
      if (!this._safeAsync)
        return base.WriteValueAsync(value, cancellationToken);
      return !value.HasValue ? this.WriteNullAsync(cancellationToken) : this.WriteValueAsync(value.GetValueOrDefault(), true, cancellationToken);
    }

    public override Task WriteValueAsync(Guid value, CancellationToken cancellationToken = default (CancellationToken)) => !this._safeAsync ? base.WriteValueAsync(value, cancellationToken) : this.DoWriteValueAsync(value, cancellationToken);

    internal async Task DoWriteValueAsync(Guid value, CancellationToken cancellationToken)
    {
      JsonTextWriter jsonTextWriter = this;
      ConfiguredTaskAwaitable configuredTaskAwaitable = jsonTextWriter.InternalWriteValueAsync(JsonToken.String, cancellationToken).ConfigureAwait(false);
      await configuredTaskAwaitable;
      configuredTaskAwaitable = jsonTextWriter._writer.WriteAsync(jsonTextWriter._quoteChar).ConfigureAwait(false);
      await configuredTaskAwaitable;
      configuredTaskAwaitable = jsonTextWriter._writer.WriteAsync(value.ToString("D", (IFormatProvider) CultureInfo.InvariantCulture), cancellationToken).ConfigureAwait(false);
      await configuredTaskAwaitable;
      configuredTaskAwaitable = jsonTextWriter._writer.WriteAsync(jsonTextWriter._quoteChar).ConfigureAwait(false);
      await configuredTaskAwaitable;
    }

    public override Task WriteValueAsync(Guid? value, CancellationToken cancellationToken = default (CancellationToken)) => !this._safeAsync ? base.WriteValueAsync(value, cancellationToken) : this.DoWriteValueAsync(value, cancellationToken);

    internal Task DoWriteValueAsync(Guid? value, CancellationToken cancellationToken) => value.HasValue ? this.DoWriteValueAsync(value.GetValueOrDefault(), cancellationToken) : this.DoWriteNullAsync(cancellationToken);

    public override Task WriteValueAsync(int value, CancellationToken cancellationToken = default (CancellationToken)) => !this._safeAsync ? base.WriteValueAsync(value, cancellationToken) : this.WriteIntegerValueAsync((long) value, cancellationToken);

    public override Task WriteValueAsync(int? value, CancellationToken cancellationToken = default (CancellationToken)) => !this._safeAsync ? base.WriteValueAsync(value, cancellationToken) : this.DoWriteValueAsync(value, cancellationToken);

    internal Task DoWriteValueAsync(int? value, CancellationToken cancellationToken) => value.HasValue ? this.WriteIntegerValueAsync((long) value.GetValueOrDefault(), cancellationToken) : this.DoWriteNullAsync(cancellationToken);

    public override Task WriteValueAsync(long value, CancellationToken cancellationToken = default (CancellationToken)) => !this._safeAsync ? base.WriteValueAsync(value, cancellationToken) : this.WriteIntegerValueAsync(value, cancellationToken);

    public override Task WriteValueAsync(long? value, CancellationToken cancellationToken = default (CancellationToken)) => !this._safeAsync ? base.WriteValueAsync(value, cancellationToken) : this.DoWriteValueAsync(value, cancellationToken);

    internal Task DoWriteValueAsync(long? value, CancellationToken cancellationToken) => value.HasValue ? this.WriteIntegerValueAsync(value.GetValueOrDefault(), cancellationToken) : this.DoWriteNullAsync(cancellationToken);

    internal Task WriteValueAsync(BigInteger value, CancellationToken cancellationToken) => this.WriteValueInternalAsync(JsonToken.Integer, value.ToString((IFormatProvider) CultureInfo.InvariantCulture), cancellationToken);

    public override Task WriteValueAsync(object? value, CancellationToken cancellationToken = default (CancellationToken))
    {
      if (!this._safeAsync)
        return base.WriteValueAsync(value, cancellationToken);
      if (value == null)
        return this.WriteNullAsync(cancellationToken);
      return value is BigInteger bigInteger ? this.WriteValueAsync(bigInteger, cancellationToken) : JsonWriter.WriteValueAsync((JsonWriter) this, ConvertUtils.GetTypeCode(value.GetType()), value, cancellationToken);
    }

    [CLSCompliant(false)]
    public override Task WriteValueAsync(sbyte value, CancellationToken cancellationToken = default (CancellationToken)) => !this._safeAsync ? base.WriteValueAsync(value, cancellationToken) : this.WriteIntegerValueAsync((long) value, cancellationToken);

    [CLSCompliant(false)]
    public override Task WriteValueAsync(sbyte? value, CancellationToken cancellationToken = default (CancellationToken)) => !this._safeAsync ? base.WriteValueAsync(value, cancellationToken) : this.DoWriteValueAsync(value, cancellationToken);

    internal Task DoWriteValueAsync(sbyte? value, CancellationToken cancellationToken) => value.HasValue ? this.WriteIntegerValueAsync((long) value.GetValueOrDefault(), cancellationToken) : this.DoWriteNullAsync(cancellationToken);

    public override Task WriteValueAsync(short value, CancellationToken cancellationToken = default (CancellationToken)) => !this._safeAsync ? base.WriteValueAsync(value, cancellationToken) : this.WriteIntegerValueAsync((long) value, cancellationToken);

    public override Task WriteValueAsync(short? value, CancellationToken cancellationToken = default (CancellationToken)) => !this._safeAsync ? base.WriteValueAsync(value, cancellationToken) : this.DoWriteValueAsync(value, cancellationToken);

    internal Task DoWriteValueAsync(short? value, CancellationToken cancellationToken) => value.HasValue ? this.WriteIntegerValueAsync((long) value.GetValueOrDefault(), cancellationToken) : this.DoWriteNullAsync(cancellationToken);

    public override Task WriteValueAsync(string? value, CancellationToken cancellationToken = default (CancellationToken)) => !this._safeAsync ? base.WriteValueAsync(value, cancellationToken) : this.DoWriteValueAsync(value, cancellationToken);

    internal Task DoWriteValueAsync(string? value, CancellationToken cancellationToken)
    {
      Task task = this.InternalWriteValueAsync(JsonToken.String, cancellationToken);
      if (!task.IsCompletedSucessfully())
        return this.DoWriteValueAsync(task, value, cancellationToken);
      return value != null ? this.WriteEscapedStringAsync(value, true, cancellationToken) : this._writer.WriteAsync(JsonConvert.Null, cancellationToken);
    }

    private async Task DoWriteValueAsync(
      Task task,
      string? value,
      CancellationToken cancellationToken)
    {
      ConfiguredTaskAwaitable configuredTaskAwaitable = task.ConfigureAwait(false);
      await configuredTaskAwaitable;
      configuredTaskAwaitable = (value == null ? this._writer.WriteAsync(JsonConvert.Null, cancellationToken) : this.WriteEscapedStringAsync(value, true, cancellationToken)).ConfigureAwait(false);
      await configuredTaskAwaitable;
    }

    public override Task WriteValueAsync(TimeSpan value, CancellationToken cancellationToken = default (CancellationToken)) => !this._safeAsync ? base.WriteValueAsync(value, cancellationToken) : this.DoWriteValueAsync(value, cancellationToken);

    internal async Task DoWriteValueAsync(TimeSpan value, CancellationToken cancellationToken)
    {
      JsonTextWriter jsonTextWriter = this;
      ConfiguredTaskAwaitable configuredTaskAwaitable = jsonTextWriter.InternalWriteValueAsync(JsonToken.String, cancellationToken).ConfigureAwait(false);
      await configuredTaskAwaitable;
      configuredTaskAwaitable = jsonTextWriter._writer.WriteAsync(jsonTextWriter._quoteChar, cancellationToken).ConfigureAwait(false);
      await configuredTaskAwaitable;
      configuredTaskAwaitable = jsonTextWriter._writer.WriteAsync(value.ToString((string) null, (IFormatProvider) CultureInfo.InvariantCulture), cancellationToken).ConfigureAwait(false);
      await configuredTaskAwaitable;
      configuredTaskAwaitable = jsonTextWriter._writer.WriteAsync(jsonTextWriter._quoteChar, cancellationToken).ConfigureAwait(false);
      await configuredTaskAwaitable;
    }

    public override Task WriteValueAsync(TimeSpan? value, CancellationToken cancellationToken = default (CancellationToken)) => !this._safeAsync ? base.WriteValueAsync(value, cancellationToken) : this.DoWriteValueAsync(value, cancellationToken);

    internal Task DoWriteValueAsync(TimeSpan? value, CancellationToken cancellationToken) => value.HasValue ? this.DoWriteValueAsync(value.GetValueOrDefault(), cancellationToken) : this.DoWriteNullAsync(cancellationToken);

    [CLSCompliant(false)]
    public override Task WriteValueAsync(uint value, CancellationToken cancellationToken = default (CancellationToken)) => !this._safeAsync ? base.WriteValueAsync(value, cancellationToken) : this.WriteIntegerValueAsync((long) value, cancellationToken);

    [CLSCompliant(false)]
    public override Task WriteValueAsync(uint? value, CancellationToken cancellationToken = default (CancellationToken)) => !this._safeAsync ? base.WriteValueAsync(value, cancellationToken) : this.DoWriteValueAsync(value, cancellationToken);

    internal Task DoWriteValueAsync(uint? value, CancellationToken cancellationToken) => value.HasValue ? this.WriteIntegerValueAsync((long) value.GetValueOrDefault(), cancellationToken) : this.DoWriteNullAsync(cancellationToken);

    [CLSCompliant(false)]
    public override Task WriteValueAsync(ulong value, CancellationToken cancellationToken = default (CancellationToken)) => !this._safeAsync ? base.WriteValueAsync(value, cancellationToken) : this.WriteIntegerValueAsync(value, cancellationToken);

    [CLSCompliant(false)]
    public override Task WriteValueAsync(ulong? value, CancellationToken cancellationToken = default (CancellationToken)) => !this._safeAsync ? base.WriteValueAsync(value, cancellationToken) : this.DoWriteValueAsync(value, cancellationToken);

    internal Task DoWriteValueAsync(ulong? value, CancellationToken cancellationToken) => value.HasValue ? this.WriteIntegerValueAsync(value.GetValueOrDefault(), cancellationToken) : this.DoWriteNullAsync(cancellationToken);

    public override Task WriteValueAsync(Uri? value, CancellationToken cancellationToken = default (CancellationToken))
    {
      if (!this._safeAsync)
        return base.WriteValueAsync(value, cancellationToken);
      return !(value == (Uri) null) ? this.WriteValueNotNullAsync(value, cancellationToken) : this.WriteNullAsync(cancellationToken);
    }

    internal Task WriteValueNotNullAsync(Uri value, CancellationToken cancellationToken)
    {
      Task task = this.InternalWriteValueAsync(JsonToken.String, cancellationToken);
      return task.IsCompletedSucessfully() ? this.WriteEscapedStringAsync(value.OriginalString, true, cancellationToken) : this.WriteValueNotNullAsync(task, value, cancellationToken);
    }

    internal async Task WriteValueNotNullAsync(
      Task task,
      Uri value,
      CancellationToken cancellationToken)
    {
      ConfiguredTaskAwaitable configuredTaskAwaitable = task.ConfigureAwait(false);
      await configuredTaskAwaitable;
      configuredTaskAwaitable = this.WriteEscapedStringAsync(value.OriginalString, true, cancellationToken).ConfigureAwait(false);
      await configuredTaskAwaitable;
    }

    [CLSCompliant(false)]
    public override Task WriteValueAsync(ushort value, CancellationToken cancellationToken = default (CancellationToken)) => !this._safeAsync ? base.WriteValueAsync(value, cancellationToken) : this.WriteIntegerValueAsync((long) value, cancellationToken);

    [CLSCompliant(false)]
    public override Task WriteValueAsync(ushort? value, CancellationToken cancellationToken = default (CancellationToken)) => !this._safeAsync ? base.WriteValueAsync(value, cancellationToken) : this.DoWriteValueAsync(value, cancellationToken);

    internal Task DoWriteValueAsync(ushort? value, CancellationToken cancellationToken) => value.HasValue ? this.WriteIntegerValueAsync((long) value.GetValueOrDefault(), cancellationToken) : this.DoWriteNullAsync(cancellationToken);

    public override Task WriteCommentAsync(string? text, CancellationToken cancellationToken = default (CancellationToken)) => !this._safeAsync ? base.WriteCommentAsync(text, cancellationToken) : this.DoWriteCommentAsync(text, cancellationToken);

    internal async Task DoWriteCommentAsync(string? text, CancellationToken cancellationToken)
    {
      JsonTextWriter jsonTextWriter = this;
      ConfiguredTaskAwaitable configuredTaskAwaitable = jsonTextWriter.InternalWriteCommentAsync(cancellationToken).ConfigureAwait(false);
      await configuredTaskAwaitable;
      configuredTaskAwaitable = jsonTextWriter._writer.WriteAsync("/*", cancellationToken).ConfigureAwait(false);
      await configuredTaskAwaitable;
      configuredTaskAwaitable = jsonTextWriter._writer.WriteAsync(text ?? string.Empty, cancellationToken).ConfigureAwait(false);
      await configuredTaskAwaitable;
      configuredTaskAwaitable = jsonTextWriter._writer.WriteAsync("*/", cancellationToken).ConfigureAwait(false);
      await configuredTaskAwaitable;
    }

    public override Task WriteEndArrayAsync(CancellationToken cancellationToken = default (CancellationToken)) => !this._safeAsync ? base.WriteEndArrayAsync(cancellationToken) : this.InternalWriteEndAsync(JsonContainerType.Array, cancellationToken);

    public override Task WriteEndConstructorAsync(CancellationToken cancellationToken = default (CancellationToken)) => !this._safeAsync ? base.WriteEndConstructorAsync(cancellationToken) : this.InternalWriteEndAsync(JsonContainerType.Constructor, cancellationToken);

    public override Task WriteEndObjectAsync(CancellationToken cancellationToken = default (CancellationToken)) => !this._safeAsync ? base.WriteEndObjectAsync(cancellationToken) : this.InternalWriteEndAsync(JsonContainerType.Object, cancellationToken);

    public override Task WriteRawValueAsync(string? json, CancellationToken cancellationToken = default (CancellationToken)) => !this._safeAsync ? base.WriteRawValueAsync(json, cancellationToken) : this.DoWriteRawValueAsync(json, cancellationToken);

    internal Task DoWriteRawValueAsync(string? json, CancellationToken cancellationToken)
    {
      this.UpdateScopeWithFinishedValue();
      Task task = this.AutoCompleteAsync(JsonToken.Undefined, cancellationToken);
      return task.IsCompletedSucessfully() ? this.WriteRawAsync(json, cancellationToken) : this.DoWriteRawValueAsync(task, json, cancellationToken);
    }

    private async Task DoWriteRawValueAsync(
      Task task,
      string? json,
      CancellationToken cancellationToken)
    {
      JsonTextWriter jsonTextWriter = this;
      ConfiguredTaskAwaitable configuredTaskAwaitable = task.ConfigureAwait(false);
      await configuredTaskAwaitable;
      configuredTaskAwaitable = jsonTextWriter.WriteRawAsync(json, cancellationToken).ConfigureAwait(false);
      await configuredTaskAwaitable;
    }

    internal char[] EnsureWriteBuffer(int length, int copyTo)
    {
      if (length < 35)
        length = 35;
      char[] writeBuffer = this._writeBuffer;
      if (writeBuffer == null)
        return this._writeBuffer = BufferUtils.RentBuffer(this._arrayPool, length);
      if (writeBuffer.Length >= length)
        return writeBuffer;
      char[] destinationArray = BufferUtils.RentBuffer(this._arrayPool, length);
      if (copyTo != 0)
        Array.Copy((Array) writeBuffer, (Array) destinationArray, copyTo);
      BufferUtils.ReturnBuffer(this._arrayPool, writeBuffer);
      this._writeBuffer = destinationArray;
      return destinationArray;
    }

    private Base64Encoder Base64Encoder
    {
      get
      {
        if (this._base64Encoder == null)
          this._base64Encoder = new Base64Encoder(this._writer);
        return this._base64Encoder;
      }
    }

    public IArrayPool<char>? ArrayPool
    {
      get => this._arrayPool;
      set => this._arrayPool = value != null ? value : throw new ArgumentNullException(nameof (value));
    }

    public int Indentation
    {
      get => this._indentation;
      set => this._indentation = value >= 0 ? value : throw new ArgumentException("Indentation value must be greater than 0.");
    }

    public char QuoteChar
    {
      get => this._quoteChar;
      set
      {
        this._quoteChar = value == '"' || value == '\'' ? value : throw new ArgumentException("Invalid JavaScript string quote character. Valid quote characters are ' and \".");
        this.UpdateCharEscapeFlags();
      }
    }

    public char IndentChar
    {
      get => this._indentChar;
      set
      {
        if ((int) value == (int) this._indentChar)
          return;
        this._indentChar = value;
        this._indentChars = (char[]) null;
      }
    }

    public bool QuoteName
    {
      get => this._quoteName;
      set => this._quoteName = value;
    }

    public JsonTextWriter(TextWriter textWriter)
    {
      this._writer = textWriter != null ? textWriter : throw new ArgumentNullException(nameof (textWriter));
      this._quoteChar = '"';
      this._quoteName = true;
      this._indentChar = ' ';
      this._indentation = 2;
      this.UpdateCharEscapeFlags();
      this._safeAsync = this.GetType() == typeof (JsonTextWriter);
    }

    public override void Flush() => this._writer.Flush();

    public override void Close()
    {
      base.Close();
      this.CloseBufferAndWriter();
    }

    private void CloseBufferAndWriter()
    {
      if (this._writeBuffer != null)
      {
        BufferUtils.ReturnBuffer(this._arrayPool, this._writeBuffer);
        this._writeBuffer = (char[]) null;
      }
      if (!this.CloseOutput)
        return;
      this._writer?.Close();
    }

    public override void WriteStartObject()
    {
      this.InternalWriteStart(JsonToken.StartObject, JsonContainerType.Object);
      this._writer.Write('{');
    }

    public override void WriteStartArray()
    {
      this.InternalWriteStart(JsonToken.StartArray, JsonContainerType.Array);
      this._writer.Write('[');
    }

    public override void WriteStartConstructor(string name)
    {
      this.InternalWriteStart(JsonToken.StartConstructor, JsonContainerType.Constructor);
      this._writer.Write("new ");
      this._writer.Write(name);
      this._writer.Write('(');
    }

    protected override void WriteEnd(JsonToken token)
    {
      switch (token)
      {
        case JsonToken.EndObject:
          this._writer.Write('}');
          break;
        case JsonToken.EndArray:
          this._writer.Write(']');
          break;
        case JsonToken.EndConstructor:
          this._writer.Write(')');
          break;
        default:
          throw JsonWriterException.Create((JsonWriter) this, "Invalid JsonToken: " + token.ToString(), (Exception) null);
      }
    }

    public override void WritePropertyName(string name)
    {
      this.InternalWritePropertyName(name);
      this.WriteEscapedString(name, this._quoteName);
      this._writer.Write(':');
    }

    public override void WritePropertyName(string name, bool escape)
    {
      this.InternalWritePropertyName(name);
      if (escape)
      {
        this.WriteEscapedString(name, this._quoteName);
      }
      else
      {
        if (this._quoteName)
          this._writer.Write(this._quoteChar);
        this._writer.Write(name);
        if (this._quoteName)
          this._writer.Write(this._quoteChar);
      }
      this._writer.Write(':');
    }

    internal override void OnStringEscapeHandlingChanged() => this.UpdateCharEscapeFlags();

    private void UpdateCharEscapeFlags() => this._charEscapeFlags = JavaScriptUtils.GetCharEscapeFlags(this.StringEscapeHandling, this._quoteChar);

    protected override void WriteIndent()
    {
      int val1 = this.Top * this._indentation;
      int index = this.SetIndentChars();
      this._writer.Write(this._indentChars, 0, index + Math.Min(val1, 12));
      while ((val1 -= 12) > 0)
        this._writer.Write(this._indentChars, index, Math.Min(val1, 12));
    }

    private int SetIndentChars()
    {
      string newLine = this._writer.NewLine;
      int length = newLine.Length;
      bool flag = this._indentChars != null && this._indentChars.Length == 12 + length;
      if (flag)
      {
        for (int index = 0; index != length; ++index)
        {
          if ((int) newLine[index] != (int) this._indentChars[index])
          {
            flag = false;
            break;
          }
        }
      }
      if (!flag)
        this._indentChars = (newLine + new string(this._indentChar, 12)).ToCharArray();
      return length;
    }

    protected override void WriteValueDelimiter() => this._writer.Write(',');

    protected override void WriteIndentSpace() => this._writer.Write(' ');

    private void WriteValueInternal(string value, JsonToken token) => this._writer.Write(value);

    public override void WriteValue(object? value)
    {
      if (value is BigInteger bigInteger)
      {
        this.InternalWriteValue(JsonToken.Integer);
        this.WriteValueInternal(bigInteger.ToString((IFormatProvider) CultureInfo.InvariantCulture), JsonToken.String);
      }
      else
        base.WriteValue(value);
    }

    public override void WriteNull()
    {
      this.InternalWriteValue(JsonToken.Null);
      this.WriteValueInternal(JsonConvert.Null, JsonToken.Null);
    }

    public override void WriteUndefined()
    {
      this.InternalWriteValue(JsonToken.Undefined);
      this.WriteValueInternal(JsonConvert.Undefined, JsonToken.Undefined);
    }

    public override void WriteRaw(string? json)
    {
      this.InternalWriteRaw();
      this._writer.Write(json);
    }

    public override void WriteValue(string? value)
    {
      this.InternalWriteValue(JsonToken.String);
      if (value == null)
        this.WriteValueInternal(JsonConvert.Null, JsonToken.Null);
      else
        this.WriteEscapedString(value, true);
    }

    private void WriteEscapedString(string value, bool quote)
    {
      this.EnsureWriteBuffer();
      JavaScriptUtils.WriteEscapedJavaScriptString(this._writer, value, this._quoteChar, quote, this._charEscapeFlags, this.StringEscapeHandling, this._arrayPool, ref this._writeBuffer);
    }

    public override void WriteValue(int value)
    {
      this.InternalWriteValue(JsonToken.Integer);
      this.WriteIntegerValue(value);
    }

    [CLSCompliant(false)]
    public override void WriteValue(uint value)
    {
      this.InternalWriteValue(JsonToken.Integer);
      this.WriteIntegerValue((long) value);
    }

    public override void WriteValue(long value)
    {
      this.InternalWriteValue(JsonToken.Integer);
      this.WriteIntegerValue(value);
    }

    [CLSCompliant(false)]
    public override void WriteValue(ulong value)
    {
      this.InternalWriteValue(JsonToken.Integer);
      this.WriteIntegerValue(value, false);
    }

    public override void WriteValue(float value)
    {
      this.InternalWriteValue(JsonToken.Float);
      this.WriteValueInternal(JsonConvert.ToString(value, this.FloatFormatHandling, this.QuoteChar, false), JsonToken.Float);
    }

    public override void WriteValue(float? value)
    {
      if (!value.HasValue)
      {
        this.WriteNull();
      }
      else
      {
        this.InternalWriteValue(JsonToken.Float);
        this.WriteValueInternal(JsonConvert.ToString(value.GetValueOrDefault(), this.FloatFormatHandling, this.QuoteChar, true), JsonToken.Float);
      }
    }

    public override void WriteValue(double value)
    {
      this.InternalWriteValue(JsonToken.Float);
      this.WriteValueInternal(JsonConvert.ToString(value, this.FloatFormatHandling, this.QuoteChar, false), JsonToken.Float);
    }

    public override void WriteValue(double? value)
    {
      if (!value.HasValue)
      {
        this.WriteNull();
      }
      else
      {
        this.InternalWriteValue(JsonToken.Float);
        this.WriteValueInternal(JsonConvert.ToString(value.GetValueOrDefault(), this.FloatFormatHandling, this.QuoteChar, true), JsonToken.Float);
      }
    }

    public override void WriteValue(bool value)
    {
      this.InternalWriteValue(JsonToken.Boolean);
      this.WriteValueInternal(JsonConvert.ToString(value), JsonToken.Boolean);
    }

    public override void WriteValue(short value)
    {
      this.InternalWriteValue(JsonToken.Integer);
      this.WriteIntegerValue((int) value);
    }

    [CLSCompliant(false)]
    public override void WriteValue(ushort value)
    {
      this.InternalWriteValue(JsonToken.Integer);
      this.WriteIntegerValue((int) value);
    }

    public override void WriteValue(char value)
    {
      this.InternalWriteValue(JsonToken.String);
      this.WriteValueInternal(JsonConvert.ToString(value), JsonToken.String);
    }

    public override void WriteValue(byte value)
    {
      this.InternalWriteValue(JsonToken.Integer);
      this.WriteIntegerValue((int) value);
    }

    [CLSCompliant(false)]
    public override void WriteValue(sbyte value)
    {
      this.InternalWriteValue(JsonToken.Integer);
      this.WriteIntegerValue((int) value);
    }

    public override void WriteValue(Decimal value)
    {
      this.InternalWriteValue(JsonToken.Float);
      this.WriteValueInternal(JsonConvert.ToString(value), JsonToken.Float);
    }

    public override void WriteValue(DateTime value)
    {
      this.InternalWriteValue(JsonToken.Date);
      value = DateTimeUtils.EnsureDateTime(value, this.DateTimeZoneHandling);
      if (StringUtils.IsNullOrEmpty(this.DateFormatString))
      {
        this._writer.Write(this._writeBuffer, 0, this.WriteValueToBuffer(value));
      }
      else
      {
        this._writer.Write(this._quoteChar);
        this._writer.Write(value.ToString(this.DateFormatString, (IFormatProvider) this.Culture));
        this._writer.Write(this._quoteChar);
      }
    }

    private int WriteValueToBuffer(DateTime value)
    {
      this.EnsureWriteBuffer();
      int num1 = 0;
      char[] writeBuffer1 = this._writeBuffer;
      int index1 = num1;
      int start = index1 + 1;
      int quoteChar1 = (int) this._quoteChar;
      writeBuffer1[index1] = (char) quoteChar1;
      int num2 = DateTimeUtils.WriteDateTimeString(this._writeBuffer, start, value, new TimeSpan?(), value.Kind, this.DateFormatHandling);
      char[] writeBuffer2 = this._writeBuffer;
      int index2 = num2;
      int buffer = index2 + 1;
      int quoteChar2 = (int) this._quoteChar;
      writeBuffer2[index2] = (char) quoteChar2;
      return buffer;
    }

    public override void WriteValue(byte[]? value)
    {
      if (value == null)
      {
        this.WriteNull();
      }
      else
      {
        this.InternalWriteValue(JsonToken.Bytes);
        this._writer.Write(this._quoteChar);
        this.Base64Encoder.Encode(value, 0, value.Length);
        this.Base64Encoder.Flush();
        this._writer.Write(this._quoteChar);
      }
    }

    public override void WriteValue(DateTimeOffset value)
    {
      this.InternalWriteValue(JsonToken.Date);
      if (StringUtils.IsNullOrEmpty(this.DateFormatString))
      {
        this._writer.Write(this._writeBuffer, 0, this.WriteValueToBuffer(value));
      }
      else
      {
        this._writer.Write(this._quoteChar);
        this._writer.Write(value.ToString(this.DateFormatString, (IFormatProvider) this.Culture));
        this._writer.Write(this._quoteChar);
      }
    }

    private int WriteValueToBuffer(DateTimeOffset value)
    {
      this.EnsureWriteBuffer();
      int num1 = 0;
      char[] writeBuffer1 = this._writeBuffer;
      int index1 = num1;
      int start = index1 + 1;
      int quoteChar1 = (int) this._quoteChar;
      writeBuffer1[index1] = (char) quoteChar1;
      int num2 = DateTimeUtils.WriteDateTimeString(this._writeBuffer, start, this.DateFormatHandling == DateFormatHandling.IsoDateFormat ? value.DateTime : value.UtcDateTime, new TimeSpan?(value.Offset), DateTimeKind.Local, this.DateFormatHandling);
      char[] writeBuffer2 = this._writeBuffer;
      int index2 = num2;
      int buffer = index2 + 1;
      int quoteChar2 = (int) this._quoteChar;
      writeBuffer2[index2] = (char) quoteChar2;
      return buffer;
    }

    public override void WriteValue(Guid value)
    {
      this.InternalWriteValue(JsonToken.String);
      string str = value.ToString("D", (IFormatProvider) CultureInfo.InvariantCulture);
      this._writer.Write(this._quoteChar);
      this._writer.Write(str);
      this._writer.Write(this._quoteChar);
    }

    public override void WriteValue(TimeSpan value)
    {
      this.InternalWriteValue(JsonToken.String);
      string str = value.ToString((string) null, (IFormatProvider) CultureInfo.InvariantCulture);
      this._writer.Write(this._quoteChar);
      this._writer.Write(str);
      this._writer.Write(this._quoteChar);
    }

    public override void WriteValue(Uri? value)
    {
      if (value == (Uri) null)
      {
        this.WriteNull();
      }
      else
      {
        this.InternalWriteValue(JsonToken.String);
        this.WriteEscapedString(value.OriginalString, true);
      }
    }

    public override void WriteComment(string? text)
    {
      this.InternalWriteComment();
      this._writer.Write("/*");
      this._writer.Write(text);
      this._writer.Write("*/");
    }

    public override void WriteWhitespace(string ws)
    {
      this.InternalWriteWhitespace(ws);
      this._writer.Write(ws);
    }

    private void EnsureWriteBuffer()
    {
      if (this._writeBuffer != null)
        return;
      this._writeBuffer = BufferUtils.RentBuffer(this._arrayPool, 35);
    }

    private void WriteIntegerValue(long value)
    {
      if (value >= 0L && value <= 9L)
      {
        this._writer.Write((char) (48UL + (ulong) value));
      }
      else
      {
        bool negative = value < 0L;
        this.WriteIntegerValue(negative ? (ulong) -value : (ulong) value, negative);
      }
    }

    private void WriteIntegerValue(ulong value, bool negative)
    {
      if (!negative & value <= 9UL)
        this._writer.Write((char) (48UL + value));
      else
        this._writer.Write(this._writeBuffer, 0, this.WriteNumberToBuffer(value, negative));
    }

    private int WriteNumberToBuffer(ulong value, bool negative)
    {
      if (value <= (ulong) uint.MaxValue)
        return this.WriteNumberToBuffer((uint) value, negative);
      this.EnsureWriteBuffer();
      int buffer = MathUtils.IntLength(value);
      if (negative)
      {
        ++buffer;
        this._writeBuffer[0] = '-';
      }
      int num1 = buffer;
      do
      {
        ulong num2 = value / 10UL;
        ulong num3 = value - num2 * 10UL;
        this._writeBuffer[--num1] = (char) (48UL + num3);
        value = num2;
      }
      while (value != 0UL);
      return buffer;
    }

    private void WriteIntegerValue(int value)
    {
      if (value >= 0 && value <= 9)
      {
        this._writer.Write((char) (48 + value));
      }
      else
      {
        bool negative = value < 0;
        this.WriteIntegerValue(negative ? (uint) -value : (uint) value, negative);
      }
    }

    private void WriteIntegerValue(uint value, bool negative)
    {
      if (!negative & value <= 9U)
        this._writer.Write((char) (48U + value));
      else
        this._writer.Write(this._writeBuffer, 0, this.WriteNumberToBuffer(value, negative));
    }

    private int WriteNumberToBuffer(uint value, bool negative)
    {
      this.EnsureWriteBuffer();
      int buffer = MathUtils.IntLength((ulong) value);
      if (negative)
      {
        ++buffer;
        this._writeBuffer[0] = '-';
      }
      int num1 = buffer;
      do
      {
        uint num2 = value / 10U;
        uint num3 = value - num2 * 10U;
        this._writeBuffer[--num1] = (char) (48U + num3);
        value = num2;
      }
      while (value != 0U);
      return buffer;
    }
  }
}
