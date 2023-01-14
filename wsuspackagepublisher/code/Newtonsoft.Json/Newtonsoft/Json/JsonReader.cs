// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.JsonReader
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: C:\Users\Administrator.THEFLIGHTSIMS\Downloads\Release.v1.4.2203.19\Newtonsoft.Json.dll

using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;


#nullable enable
namespace Newtonsoft.Json
{
  public abstract class JsonReader : IDisposable
  {
    private JsonToken _tokenType;
    private object? _value;
    internal char _quoteChar;
    internal JsonReader.State _currentState;
    private JsonPosition _currentPosition;
    private CultureInfo? _culture;
    private DateTimeZoneHandling _dateTimeZoneHandling;
    private int? _maxDepth;
    private bool _hasExceededMaxDepth;
    internal DateParseHandling _dateParseHandling;
    internal FloatParseHandling _floatParseHandling;
    private string? _dateFormatString;
    private List<JsonPosition>? _stack;

    public virtual Task<bool> ReadAsync(CancellationToken cancellationToken = default (CancellationToken)) => cancellationToken.CancelIfRequestedAsync<bool>() ?? this.Read().ToAsync();

    public async Task SkipAsync(CancellationToken cancellationToken = default (CancellationToken))
    {
      ConfiguredTaskAwaitable<bool> configuredTaskAwaitable;
      if (this.TokenType == JsonToken.PropertyName)
      {
        configuredTaskAwaitable = this.ReadAsync(cancellationToken).ConfigureAwait(false);
        int num = await configuredTaskAwaitable ? 1 : 0;
      }
      if (!JsonTokenUtils.IsStartToken(this.TokenType))
        return;
      int depth = this.Depth;
      do
      {
        configuredTaskAwaitable = this.ReadAsync(cancellationToken).ConfigureAwait(false);
      }
      while (await configuredTaskAwaitable && depth < this.Depth);
    }

    internal async Task ReaderReadAndAssertAsync(CancellationToken cancellationToken)
    {
      if (!await this.ReadAsync(cancellationToken).ConfigureAwait(false))
        throw this.CreateUnexpectedEndException();
    }

    public virtual Task<bool?> ReadAsBooleanAsync(CancellationToken cancellationToken = default (CancellationToken)) => cancellationToken.CancelIfRequestedAsync<bool?>() ?? Task.FromResult<bool?>(this.ReadAsBoolean());

    public virtual Task<byte[]?> ReadAsBytesAsync(CancellationToken cancellationToken = default (CancellationToken)) => cancellationToken.CancelIfRequestedAsync<byte[]>() ?? Task.FromResult<byte[]>(this.ReadAsBytes());

    internal async Task<byte[]?> ReadArrayIntoByteArrayAsync(CancellationToken cancellationToken)
    {
      List<byte> buffer = new List<byte>();
      do
      {
        if (!await this.ReadAsync(cancellationToken).ConfigureAwait(false))
          this.SetToken(JsonToken.None);
      }
      while (!this.ReadArrayElementIntoByteArrayReportDone(buffer));
      byte[] array = buffer.ToArray();
      this.SetToken(JsonToken.Bytes, (object) array, false);
      return array;
    }

    public virtual Task<DateTime?> ReadAsDateTimeAsync(CancellationToken cancellationToken = default (CancellationToken)) => cancellationToken.CancelIfRequestedAsync<DateTime?>() ?? Task.FromResult<DateTime?>(this.ReadAsDateTime());

    public virtual Task<DateTimeOffset?> ReadAsDateTimeOffsetAsync(
      CancellationToken cancellationToken = default (CancellationToken))
    {
      return cancellationToken.CancelIfRequestedAsync<DateTimeOffset?>() ?? Task.FromResult<DateTimeOffset?>(this.ReadAsDateTimeOffset());
    }

    public virtual Task<Decimal?> ReadAsDecimalAsync(CancellationToken cancellationToken = default (CancellationToken)) => cancellationToken.CancelIfRequestedAsync<Decimal?>() ?? Task.FromResult<Decimal?>(this.ReadAsDecimal());

    public virtual Task<double?> ReadAsDoubleAsync(CancellationToken cancellationToken = default (CancellationToken)) => Task.FromResult<double?>(this.ReadAsDouble());

    public virtual Task<int?> ReadAsInt32Async(CancellationToken cancellationToken = default (CancellationToken)) => cancellationToken.CancelIfRequestedAsync<int?>() ?? Task.FromResult<int?>(this.ReadAsInt32());

    public virtual Task<string?> ReadAsStringAsync(CancellationToken cancellationToken = default (CancellationToken)) => cancellationToken.CancelIfRequestedAsync<string>() ?? Task.FromResult<string>(this.ReadAsString());

    internal async Task<bool> ReadAndMoveToContentAsync(CancellationToken cancellationToken)
    {
      ConfiguredTaskAwaitable<bool> configuredTaskAwaitable = this.ReadAsync(cancellationToken).ConfigureAwait(false);
      bool contentAsync = await configuredTaskAwaitable;
      if (contentAsync)
      {
        configuredTaskAwaitable = this.MoveToContentAsync(cancellationToken).ConfigureAwait(false);
        contentAsync = await configuredTaskAwaitable;
      }
      return contentAsync;
    }

    internal Task<bool> MoveToContentAsync(CancellationToken cancellationToken)
    {
      switch (this.TokenType)
      {
        case JsonToken.None:
        case JsonToken.Comment:
          return this.MoveToContentFromNonContentAsync(cancellationToken);
        default:
          return AsyncUtils.True;
      }
    }

    private async Task<bool> MoveToContentFromNonContentAsync(CancellationToken cancellationToken)
    {
label_1:
      if (!await this.ReadAsync(cancellationToken).ConfigureAwait(false))
        return false;
      switch (this.TokenType)
      {
        case JsonToken.None:
        case JsonToken.Comment:
          goto label_1;
        default:
          return true;
      }
    }

    protected JsonReader.State CurrentState => this._currentState;

    public bool CloseInput { get; set; }

    public bool SupportMultipleContent { get; set; }

    public virtual char QuoteChar
    {
      get => this._quoteChar;
      protected internal set => this._quoteChar = value;
    }

    public DateTimeZoneHandling DateTimeZoneHandling
    {
      get => this._dateTimeZoneHandling;
      set => this._dateTimeZoneHandling = value >= DateTimeZoneHandling.Local && value <= DateTimeZoneHandling.RoundtripKind ? value : throw new ArgumentOutOfRangeException(nameof (value));
    }

    public DateParseHandling DateParseHandling
    {
      get => this._dateParseHandling;
      set => this._dateParseHandling = value >= DateParseHandling.None && value <= DateParseHandling.DateTimeOffset ? value : throw new ArgumentOutOfRangeException(nameof (value));
    }

    public FloatParseHandling FloatParseHandling
    {
      get => this._floatParseHandling;
      set => this._floatParseHandling = value >= FloatParseHandling.Double && value <= FloatParseHandling.Decimal ? value : throw new ArgumentOutOfRangeException(nameof (value));
    }

    public string? DateFormatString
    {
      get => this._dateFormatString;
      set => this._dateFormatString = value;
    }

    public int? MaxDepth
    {
      get => this._maxDepth;
      set
      {
        int? nullable = value;
        int num = 0;
        if (nullable.GetValueOrDefault() <= num & nullable.HasValue)
          throw new ArgumentException("Value must be positive.", nameof (value));
        this._maxDepth = value;
      }
    }

    public virtual JsonToken TokenType => this._tokenType;

    public virtual object? Value => this._value;

    public virtual Type? ValueType => this._value?.GetType();

    public virtual int Depth
    {
      get
      {
        List<JsonPosition> stack = this._stack;
        int num = stack != null ? __nonvirtual (stack.Count) : 0;
        return JsonTokenUtils.IsStartToken(this.TokenType) || this._currentPosition.Type == JsonContainerType.None ? num : num + 1;
      }
    }

    public virtual string Path => this._currentPosition.Type == JsonContainerType.None ? string.Empty : JsonPosition.BuildPath(this._stack, (this._currentState == JsonReader.State.ArrayStart || this._currentState == JsonReader.State.ConstructorStart ? 0 : (this._currentState != JsonReader.State.ObjectStart ? 1 : 0)) != 0 ? new JsonPosition?(this._currentPosition) : new JsonPosition?());

    public CultureInfo Culture
    {
      get => this._culture ?? CultureInfo.InvariantCulture;
      set => this._culture = value;
    }

    internal JsonPosition GetPosition(int depth) => this._stack != null && depth < this._stack.Count ? this._stack[depth] : this._currentPosition;

    protected JsonReader()
    {
      this._currentState = JsonReader.State.Start;
      this._dateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind;
      this._dateParseHandling = DateParseHandling.DateTime;
      this._floatParseHandling = FloatParseHandling.Double;
      this.CloseInput = true;
    }

    private void Push(JsonContainerType value)
    {
      this.UpdateScopeWithFinishedValue();
      if (this._currentPosition.Type == JsonContainerType.None)
      {
        this._currentPosition = new JsonPosition(value);
      }
      else
      {
        if (this._stack == null)
          this._stack = new List<JsonPosition>();
        this._stack.Add(this._currentPosition);
        this._currentPosition = new JsonPosition(value);
        if (!this._maxDepth.HasValue)
          return;
        int num = this.Depth + 1;
        int? maxDepth = this._maxDepth;
        int valueOrDefault = maxDepth.GetValueOrDefault();
        if (num > valueOrDefault & maxDepth.HasValue && !this._hasExceededMaxDepth)
        {
          this._hasExceededMaxDepth = true;
          throw JsonReaderException.Create(this, "The reader's MaxDepth of {0} has been exceeded.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this._maxDepth));
        }
      }
    }

    private JsonContainerType Pop()
    {
      JsonPosition currentPosition;
      if (this._stack != null && this._stack.Count > 0)
      {
        currentPosition = this._currentPosition;
        this._currentPosition = this._stack[this._stack.Count - 1];
        this._stack.RemoveAt(this._stack.Count - 1);
      }
      else
      {
        currentPosition = this._currentPosition;
        this._currentPosition = new JsonPosition();
      }
      if (this._maxDepth.HasValue)
      {
        int depth = this.Depth;
        int? maxDepth = this._maxDepth;
        int valueOrDefault = maxDepth.GetValueOrDefault();
        if (depth <= valueOrDefault & maxDepth.HasValue)
          this._hasExceededMaxDepth = false;
      }
      return currentPosition.Type;
    }

    private JsonContainerType Peek() => this._currentPosition.Type;

    public abstract bool Read();

    public virtual int? ReadAsInt32()
    {
      JsonToken contentToken = this.GetContentToken();
      switch (contentToken)
      {
        case JsonToken.None:
        case JsonToken.Null:
        case JsonToken.EndArray:
          return new int?();
        case JsonToken.Integer:
        case JsonToken.Float:
          object obj = this.Value;
          int num1;
          switch (obj)
          {
            case int num2:
              return new int?(num2);
            case BigInteger bigInteger:
              num1 = (int) bigInteger;
              break;
            default:
              try
              {
                num1 = Convert.ToInt32(obj, (IFormatProvider) CultureInfo.InvariantCulture);
                break;
              }
              catch (Exception ex)
              {
                throw JsonReaderException.Create(this, "Could not convert to integer: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, obj), ex);
              }
          }
          this.SetToken(JsonToken.Integer, (object) num1, false);
          return new int?(num1);
        case JsonToken.String:
          return this.ReadInt32String((string) this.Value);
        default:
          throw JsonReaderException.Create(this, "Error reading integer. Unexpected token: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) contentToken));
      }
    }

    internal int? ReadInt32String(string? s)
    {
      if (StringUtils.IsNullOrEmpty(s))
      {
        this.SetToken(JsonToken.Null, (object) null, false);
        return new int?();
      }
      int result;
      if (int.TryParse(s, NumberStyles.Integer, (IFormatProvider) this.Culture, out result))
      {
        this.SetToken(JsonToken.Integer, (object) result, false);
        return new int?(result);
      }
      this.SetToken(JsonToken.String, (object) s, false);
      throw JsonReaderException.Create(this, "Could not convert string to integer: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) s));
    }

    public virtual string? ReadAsString()
    {
      JsonToken contentToken = this.GetContentToken();
      switch (contentToken)
      {
        case JsonToken.None:
        case JsonToken.Null:
        case JsonToken.EndArray:
          return (string) null;
        case JsonToken.String:
          return (string) this.Value;
        default:
          if (JsonTokenUtils.IsPrimitiveToken(contentToken))
          {
            object obj = this.Value;
            if (obj != null)
            {
              string str;
              if (obj is IFormattable formattable)
              {
                str = formattable.ToString((string) null, (IFormatProvider) this.Culture);
              }
              else
              {
                Uri uri = obj as Uri;
                str = (object) uri != null ? uri.OriginalString : obj.ToString();
              }
              this.SetToken(JsonToken.String, (object) str, false);
              return str;
            }
          }
          throw JsonReaderException.Create(this, "Error reading string. Unexpected token: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) contentToken));
      }
    }

    public virtual byte[]? ReadAsBytes()
    {
      JsonToken contentToken = this.GetContentToken();
      switch (contentToken)
      {
        case JsonToken.None:
        case JsonToken.Null:
        case JsonToken.EndArray:
          return (byte[]) null;
        case JsonToken.StartObject:
          this.ReadIntoWrappedTypeObject();
          byte[] numArray1 = this.ReadAsBytes();
          this.ReaderReadAndAssert();
          if (this.TokenType != JsonToken.EndObject)
            throw JsonReaderException.Create(this, "Error reading bytes. Unexpected token: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this.TokenType));
          this.SetToken(JsonToken.Bytes, (object) numArray1, false);
          return numArray1;
        case JsonToken.StartArray:
          return this.ReadArrayIntoByteArray();
        case JsonToken.String:
          string s = (string) this.Value;
          Guid g;
          byte[] numArray2 = s.Length != 0 ? (!ConvertUtils.TryConvertGuid(s, out g) ? Convert.FromBase64String(s) : g.ToByteArray()) : CollectionUtils.ArrayEmpty<byte>();
          this.SetToken(JsonToken.Bytes, (object) numArray2, false);
          return numArray2;
        case JsonToken.Bytes:
          if (!(this.Value is Guid guid))
            return (byte[]) this.Value;
          byte[] byteArray = guid.ToByteArray();
          this.SetToken(JsonToken.Bytes, (object) byteArray, false);
          return byteArray;
        default:
          throw JsonReaderException.Create(this, "Error reading bytes. Unexpected token: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) contentToken));
      }
    }

    internal byte[] ReadArrayIntoByteArray()
    {
      List<byte> buffer = new List<byte>();
      do
      {
        if (!this.Read())
          this.SetToken(JsonToken.None);
      }
      while (!this.ReadArrayElementIntoByteArrayReportDone(buffer));
      byte[] array = buffer.ToArray();
      this.SetToken(JsonToken.Bytes, (object) array, false);
      return array;
    }

    private bool ReadArrayElementIntoByteArrayReportDone(List<byte> buffer)
    {
      switch (this.TokenType)
      {
        case JsonToken.None:
          throw JsonReaderException.Create(this, "Unexpected end when reading bytes.");
        case JsonToken.Comment:
          return false;
        case JsonToken.Integer:
          buffer.Add(Convert.ToByte(this.Value, (IFormatProvider) CultureInfo.InvariantCulture));
          return false;
        case JsonToken.EndArray:
          return true;
        default:
          throw JsonReaderException.Create(this, "Unexpected token when reading bytes: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this.TokenType));
      }
    }

    public virtual double? ReadAsDouble()
    {
      JsonToken contentToken = this.GetContentToken();
      switch (contentToken)
      {
        case JsonToken.None:
        case JsonToken.Null:
        case JsonToken.EndArray:
          return new double?();
        case JsonToken.Integer:
        case JsonToken.Float:
          object obj = this.Value;
          double num1;
          switch (obj)
          {
            case double num2:
              return new double?(num2);
            case BigInteger bigInteger:
              num1 = (double) bigInteger;
              break;
            default:
              num1 = Convert.ToDouble(obj, (IFormatProvider) CultureInfo.InvariantCulture);
              break;
          }
          this.SetToken(JsonToken.Float, (object) num1, false);
          return new double?(num1);
        case JsonToken.String:
          return this.ReadDoubleString((string) this.Value);
        default:
          throw JsonReaderException.Create(this, "Error reading double. Unexpected token: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) contentToken));
      }
    }

    internal double? ReadDoubleString(string? s)
    {
      if (StringUtils.IsNullOrEmpty(s))
      {
        this.SetToken(JsonToken.Null, (object) null, false);
        return new double?();
      }
      double result;
      if (double.TryParse(s, NumberStyles.Float | NumberStyles.AllowThousands, (IFormatProvider) this.Culture, out result))
      {
        this.SetToken(JsonToken.Float, (object) result, false);
        return new double?(result);
      }
      this.SetToken(JsonToken.String, (object) s, false);
      throw JsonReaderException.Create(this, "Could not convert string to double: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) s));
    }

    public virtual bool? ReadAsBoolean()
    {
      JsonToken contentToken = this.GetContentToken();
      switch (contentToken)
      {
        case JsonToken.None:
        case JsonToken.Null:
        case JsonToken.EndArray:
          return new bool?();
        case JsonToken.Integer:
        case JsonToken.Float:
          bool flag = !(this.Value is BigInteger bigInteger) ? Convert.ToBoolean(this.Value, (IFormatProvider) CultureInfo.InvariantCulture) : bigInteger != 0L;
          this.SetToken(JsonToken.Boolean, (object) flag, false);
          return new bool?(flag);
        case JsonToken.String:
          return this.ReadBooleanString((string) this.Value);
        case JsonToken.Boolean:
          return new bool?((bool) this.Value);
        default:
          throw JsonReaderException.Create(this, "Error reading boolean. Unexpected token: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) contentToken));
      }
    }

    internal bool? ReadBooleanString(string? s)
    {
      if (StringUtils.IsNullOrEmpty(s))
      {
        this.SetToken(JsonToken.Null, (object) null, false);
        return new bool?();
      }
      bool result;
      if (bool.TryParse(s, out result))
      {
        this.SetToken(JsonToken.Boolean, (object) result, false);
        return new bool?(result);
      }
      this.SetToken(JsonToken.String, (object) s, false);
      throw JsonReaderException.Create(this, "Could not convert string to boolean: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) s));
    }

    public virtual Decimal? ReadAsDecimal()
    {
      JsonToken contentToken = this.GetContentToken();
      switch (contentToken)
      {
        case JsonToken.None:
        case JsonToken.Null:
        case JsonToken.EndArray:
          return new Decimal?();
        case JsonToken.Integer:
        case JsonToken.Float:
          object obj = this.Value;
          Decimal num1;
          switch (obj)
          {
            case Decimal num2:
              return new Decimal?(num2);
            case BigInteger bigInteger:
              num1 = (Decimal) bigInteger;
              break;
            default:
              try
              {
                num1 = Convert.ToDecimal(obj, (IFormatProvider) CultureInfo.InvariantCulture);
                break;
              }
              catch (Exception ex)
              {
                throw JsonReaderException.Create(this, "Could not convert to decimal: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, obj), ex);
              }
          }
          this.SetToken(JsonToken.Float, (object) num1, false);
          return new Decimal?(num1);
        case JsonToken.String:
          return this.ReadDecimalString((string) this.Value);
        default:
          throw JsonReaderException.Create(this, "Error reading decimal. Unexpected token: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) contentToken));
      }
    }

    internal Decimal? ReadDecimalString(string? s)
    {
      if (StringUtils.IsNullOrEmpty(s))
      {
        this.SetToken(JsonToken.Null, (object) null, false);
        return new Decimal?();
      }
      Decimal result;
      if (Decimal.TryParse(s, NumberStyles.Number, (IFormatProvider) this.Culture, out result))
      {
        this.SetToken(JsonToken.Float, (object) result, false);
        return new Decimal?(result);
      }
      if (ConvertUtils.DecimalTryParse(s.ToCharArray(), 0, s.Length, out result) == ParseResult.Success)
      {
        this.SetToken(JsonToken.Float, (object) result, false);
        return new Decimal?(result);
      }
      this.SetToken(JsonToken.String, (object) s, false);
      throw JsonReaderException.Create(this, "Could not convert string to decimal: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) s));
    }

    public virtual DateTime? ReadAsDateTime()
    {
      switch (this.GetContentToken())
      {
        case JsonToken.None:
        case JsonToken.Null:
        case JsonToken.EndArray:
          return new DateTime?();
        case JsonToken.String:
          return this.ReadDateTimeString((string) this.Value);
        case JsonToken.Date:
          if (this.Value is DateTimeOffset dateTimeOffset)
            this.SetToken(JsonToken.Date, (object) dateTimeOffset.DateTime, false);
          return new DateTime?((DateTime) this.Value);
        default:
          throw JsonReaderException.Create(this, "Error reading date. Unexpected token: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this.TokenType));
      }
    }

    internal DateTime? ReadDateTimeString(string? s)
    {
      if (StringUtils.IsNullOrEmpty(s))
      {
        this.SetToken(JsonToken.Null, (object) null, false);
        return new DateTime?();
      }
      DateTime dateTime;
      if (DateTimeUtils.TryParseDateTime(s, this.DateTimeZoneHandling, this._dateFormatString, this.Culture, out dateTime))
      {
        dateTime = DateTimeUtils.EnsureDateTime(dateTime, this.DateTimeZoneHandling);
        this.SetToken(JsonToken.Date, (object) dateTime, false);
        return new DateTime?(dateTime);
      }
      if (!DateTime.TryParse(s, (IFormatProvider) this.Culture, DateTimeStyles.RoundtripKind, out dateTime))
        throw JsonReaderException.Create(this, "Could not convert string to DateTime: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) s));
      dateTime = DateTimeUtils.EnsureDateTime(dateTime, this.DateTimeZoneHandling);
      this.SetToken(JsonToken.Date, (object) dateTime, false);
      return new DateTime?(dateTime);
    }

    public virtual DateTimeOffset? ReadAsDateTimeOffset()
    {
      JsonToken contentToken = this.GetContentToken();
      switch (contentToken)
      {
        case JsonToken.None:
        case JsonToken.Null:
        case JsonToken.EndArray:
          return new DateTimeOffset?();
        case JsonToken.String:
          return this.ReadDateTimeOffsetString((string) this.Value);
        case JsonToken.Date:
          if (this.Value is DateTime dateTime)
            this.SetToken(JsonToken.Date, (object) new DateTimeOffset(dateTime), false);
          return new DateTimeOffset?((DateTimeOffset) this.Value);
        default:
          throw JsonReaderException.Create(this, "Error reading date. Unexpected token: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) contentToken));
      }
    }

    internal DateTimeOffset? ReadDateTimeOffsetString(string? s)
    {
      if (StringUtils.IsNullOrEmpty(s))
      {
        this.SetToken(JsonToken.Null, (object) null, false);
        return new DateTimeOffset?();
      }
      DateTimeOffset dateTimeOffset;
      if (DateTimeUtils.TryParseDateTimeOffset(s, this._dateFormatString, this.Culture, out dateTimeOffset))
      {
        this.SetToken(JsonToken.Date, (object) dateTimeOffset, false);
        return new DateTimeOffset?(dateTimeOffset);
      }
      if (DateTimeOffset.TryParse(s, (IFormatProvider) this.Culture, DateTimeStyles.RoundtripKind, out dateTimeOffset))
      {
        this.SetToken(JsonToken.Date, (object) dateTimeOffset, false);
        return new DateTimeOffset?(dateTimeOffset);
      }
      this.SetToken(JsonToken.String, (object) s, false);
      throw JsonReaderException.Create(this, "Could not convert string to DateTimeOffset: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) s));
    }

    internal void ReaderReadAndAssert()
    {
      if (!this.Read())
        throw this.CreateUnexpectedEndException();
    }

    internal JsonReaderException CreateUnexpectedEndException() => JsonReaderException.Create(this, "Unexpected end when reading JSON.");

    internal void ReadIntoWrappedTypeObject()
    {
      this.ReaderReadAndAssert();
      if (this.Value != null && this.Value.ToString() == "$type")
      {
        this.ReaderReadAndAssert();
        if (this.Value != null && this.Value.ToString().StartsWith("System.Byte[]", StringComparison.Ordinal))
        {
          this.ReaderReadAndAssert();
          if (this.Value.ToString() == "$value")
            return;
        }
      }
      throw JsonReaderException.Create(this, "Error reading bytes. Unexpected token: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JsonToken.StartObject));
    }

    public void Skip()
    {
      if (this.TokenType == JsonToken.PropertyName)
        this.Read();
      if (!JsonTokenUtils.IsStartToken(this.TokenType))
        return;
      int depth = this.Depth;
      do
        ;
      while (this.Read() && depth < this.Depth);
    }

    protected void SetToken(JsonToken newToken) => this.SetToken(newToken, (object) null, true);

    protected void SetToken(JsonToken newToken, object? value) => this.SetToken(newToken, value, true);

    protected void SetToken(JsonToken newToken, object? value, bool updateIndex)
    {
      this._tokenType = newToken;
      this._value = value;
      switch (newToken)
      {
        case JsonToken.StartObject:
          this._currentState = JsonReader.State.ObjectStart;
          this.Push(JsonContainerType.Object);
          break;
        case JsonToken.StartArray:
          this._currentState = JsonReader.State.ArrayStart;
          this.Push(JsonContainerType.Array);
          break;
        case JsonToken.StartConstructor:
          this._currentState = JsonReader.State.ConstructorStart;
          this.Push(JsonContainerType.Constructor);
          break;
        case JsonToken.PropertyName:
          this._currentState = JsonReader.State.Property;
          this._currentPosition.PropertyName = (string) value;
          break;
        case JsonToken.Raw:
        case JsonToken.Integer:
        case JsonToken.Float:
        case JsonToken.String:
        case JsonToken.Boolean:
        case JsonToken.Null:
        case JsonToken.Undefined:
        case JsonToken.Date:
        case JsonToken.Bytes:
          this.SetPostValueState(updateIndex);
          break;
        case JsonToken.EndObject:
          this.ValidateEnd(JsonToken.EndObject);
          break;
        case JsonToken.EndArray:
          this.ValidateEnd(JsonToken.EndArray);
          break;
        case JsonToken.EndConstructor:
          this.ValidateEnd(JsonToken.EndConstructor);
          break;
      }
    }

    internal void SetPostValueState(bool updateIndex)
    {
      if (this.Peek() != JsonContainerType.None || this.SupportMultipleContent)
        this._currentState = JsonReader.State.PostValue;
      else
        this.SetFinished();
      if (!updateIndex)
        return;
      this.UpdateScopeWithFinishedValue();
    }

    private void UpdateScopeWithFinishedValue()
    {
      if (!this._currentPosition.HasIndex)
        return;
      ++this._currentPosition.Position;
    }

    private void ValidateEnd(JsonToken endToken)
    {
      JsonContainerType jsonContainerType = this.Pop();
      if (this.GetTypeForCloseToken(endToken) != jsonContainerType)
        throw JsonReaderException.Create(this, "JsonToken {0} is not valid for closing JsonType {1}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) endToken, (object) jsonContainerType));
      if (this.Peek() != JsonContainerType.None || this.SupportMultipleContent)
        this._currentState = JsonReader.State.PostValue;
      else
        this.SetFinished();
    }

    protected void SetStateBasedOnCurrent()
    {
      JsonContainerType jsonContainerType = this.Peek();
      switch (jsonContainerType)
      {
        case JsonContainerType.None:
          this.SetFinished();
          break;
        case JsonContainerType.Object:
          this._currentState = JsonReader.State.Object;
          break;
        case JsonContainerType.Array:
          this._currentState = JsonReader.State.Array;
          break;
        case JsonContainerType.Constructor:
          this._currentState = JsonReader.State.Constructor;
          break;
        default:
          throw JsonReaderException.Create(this, "While setting the reader state back to current object an unexpected JsonType was encountered: {0}".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) jsonContainerType));
      }
    }

    private void SetFinished() => this._currentState = this.SupportMultipleContent ? JsonReader.State.Start : JsonReader.State.Finished;

    private JsonContainerType GetTypeForCloseToken(JsonToken token)
    {
      switch (token)
      {
        case JsonToken.EndObject:
          return JsonContainerType.Object;
        case JsonToken.EndArray:
          return JsonContainerType.Array;
        case JsonToken.EndConstructor:
          return JsonContainerType.Constructor;
        default:
          throw JsonReaderException.Create(this, "Not a valid close JsonToken: {0}".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) token));
      }
    }

    void IDisposable.Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (!(this._currentState != JsonReader.State.Closed & disposing))
        return;
      this.Close();
    }

    public virtual void Close()
    {
      this._currentState = JsonReader.State.Closed;
      this._tokenType = JsonToken.None;
      this._value = (object) null;
    }

    internal void ReadAndAssert()
    {
      if (!this.Read())
        throw JsonSerializationException.Create(this, "Unexpected end when reading JSON.");
    }

    internal void ReadForTypeAndAssert(JsonContract? contract, bool hasConverter)
    {
      if (!this.ReadForType(contract, hasConverter))
        throw JsonSerializationException.Create(this, "Unexpected end when reading JSON.");
    }

    internal bool ReadForType(JsonContract? contract, bool hasConverter)
    {
      if (hasConverter)
        return this.Read();
      switch (contract != null ? (int) contract.InternalReadType : 0)
      {
        case 0:
          return this.ReadAndMoveToContent();
        case 1:
          this.ReadAsInt32();
          break;
        case 2:
          int num = this.ReadAndMoveToContent() ? 1 : 0;
          if (this.TokenType != JsonToken.Undefined)
            return num != 0;
          CultureInfo invariantCulture = CultureInfo.InvariantCulture;
          Type type = contract?.UnderlyingType;
          if ((object) type == null)
            type = typeof (long);
          throw JsonReaderException.Create(this, "An undefined token is not a valid {0}.".FormatWith((IFormatProvider) invariantCulture, (object) type));
        case 3:
          this.ReadAsBytes();
          break;
        case 4:
          this.ReadAsString();
          break;
        case 5:
          this.ReadAsDecimal();
          break;
        case 6:
          this.ReadAsDateTime();
          break;
        case 7:
          this.ReadAsDateTimeOffset();
          break;
        case 8:
          this.ReadAsDouble();
          break;
        case 9:
          this.ReadAsBoolean();
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
      return this.TokenType != 0;
    }

    internal bool ReadAndMoveToContent() => this.Read() && this.MoveToContent();

    internal bool MoveToContent()
    {
      for (JsonToken tokenType = this.TokenType; tokenType == JsonToken.None || tokenType == JsonToken.Comment; tokenType = this.TokenType)
      {
        if (!this.Read())
          return false;
      }
      return true;
    }

    private JsonToken GetContentToken()
    {
      while (this.Read())
      {
        JsonToken tokenType = this.TokenType;
        if (tokenType != JsonToken.Comment)
          return tokenType;
      }
      this.SetToken(JsonToken.None);
      return JsonToken.None;
    }

    protected internal enum State
    {
      Start,
      Complete,
      Property,
      ObjectStart,
      Object,
      ArrayStart,
      Array,
      Closed,
      PostValue,
      ConstructorStart,
      Constructor,
      Error,
      Finished,
    }
  }
}
