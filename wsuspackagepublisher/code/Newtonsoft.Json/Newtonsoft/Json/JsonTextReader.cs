// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.JsonTextReader
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
  public class JsonTextReader : JsonReader, IJsonLineInfo
  {
    private readonly bool _safeAsync;
    private const char UnicodeReplacementChar = '�';
    private const int MaximumJavascriptIntegerCharacterLength = 380;
    private const int LargeBufferLength = 1073741823;
    private readonly TextReader _reader;
    private char[]? _chars;
    private int _charsUsed;
    private int _charPos;
    private int _lineStartPos;
    private int _lineNumber;
    private bool _isEndOfFile;
    private StringBuffer _stringBuffer;
    private StringReference _stringReference;
    private IArrayPool<char>? _arrayPool;

    public override Task<bool> ReadAsync(CancellationToken cancellationToken = default (CancellationToken)) => !this._safeAsync ? base.ReadAsync(cancellationToken) : this.DoReadAsync(cancellationToken);

    internal Task<bool> DoReadAsync(CancellationToken cancellationToken)
    {
      this.EnsureBuffer();
      Task<bool> postValueAsync;
      do
      {
        switch (this._currentState)
        {
          case JsonReader.State.Start:
          case JsonReader.State.Property:
          case JsonReader.State.ArrayStart:
          case JsonReader.State.Array:
          case JsonReader.State.ConstructorStart:
          case JsonReader.State.Constructor:
            return this.ParseValueAsync(cancellationToken);
          case JsonReader.State.ObjectStart:
          case JsonReader.State.Object:
            return this.ParseObjectAsync(cancellationToken);
          case JsonReader.State.PostValue:
            postValueAsync = this.ParsePostValueAsync(false, cancellationToken);
            if (postValueAsync.IsCompletedSucessfully())
              continue;
            goto label_7;
          case JsonReader.State.Finished:
            goto label_8;
          default:
            goto label_9;
        }
      }
      while (!postValueAsync.Result);
      return AsyncUtils.True;
label_7:
      return this.DoReadAsync(postValueAsync, cancellationToken);
label_8:
      return this.ReadFromFinishedAsync(cancellationToken);
label_9:
      throw JsonReaderException.Create((JsonReader) this, "Unexpected state: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this.CurrentState));
    }

    private async Task<bool> DoReadAsync(Task<bool> task, CancellationToken cancellationToken)
    {
      ConfiguredTaskAwaitable<bool> configuredTaskAwaitable = task.ConfigureAwait(false);
      if (await configuredTaskAwaitable)
        return true;
      configuredTaskAwaitable = this.DoReadAsync(cancellationToken).ConfigureAwait(false);
      return await configuredTaskAwaitable;
    }

    private async Task<bool> ParsePostValueAsync(
      bool ignoreComments,
      CancellationToken cancellationToken)
    {
      JsonTextReader reader = this;
      char c;
      while (true)
      {
        do
        {
          do
          {
            c = reader._chars[reader._charPos];
            switch (c)
            {
              case char.MinValue:
                if (reader._charsUsed == reader._charPos)
                  continue;
                goto label_5;
              case '\t':
              case ' ':
                goto label_12;
              case '\n':
                goto label_14;
              case '\r':
                goto label_13;
              case ')':
                goto label_8;
              case ',':
                goto label_11;
              case '/':
                goto label_9;
              case ']':
                goto label_7;
              case '}':
                goto label_6;
              default:
                goto label_15;
            }
          }
          while (await reader.ReadDataAsync(false, cancellationToken).ConfigureAwait(false) != 0);
          reader._currentState = JsonReader.State.Finished;
          return false;
label_5:
          ++reader._charPos;
          continue;
label_6:
          ++reader._charPos;
          reader.SetToken(JsonToken.EndObject);
          return true;
label_7:
          ++reader._charPos;
          reader.SetToken(JsonToken.EndArray);
          return true;
label_8:
          ++reader._charPos;
          reader.SetToken(JsonToken.EndConstructor);
          return true;
label_9:
          await reader.ParseCommentAsync(!ignoreComments, cancellationToken).ConfigureAwait(false);
        }
        while (ignoreComments);
        break;
label_12:
        ++reader._charPos;
        continue;
label_13:
        await reader.ProcessCarriageReturnAsync(false, cancellationToken).ConfigureAwait(false);
        continue;
label_14:
        reader.ProcessLineFeed();
        continue;
label_15:
        if (char.IsWhiteSpace(c))
          ++reader._charPos;
        else
          goto label_17;
      }
      return true;
label_11:
      ++reader._charPos;
      reader.SetStateBasedOnCurrent();
      return false;
label_17:
      if (!reader.SupportMultipleContent || reader.Depth != 0)
        throw JsonReaderException.Create((JsonReader) reader, "After parsing a value an unexpected character was encountered: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) c));
      reader.SetStateBasedOnCurrent();
      return false;
    }

    private async Task<bool> ReadFromFinishedAsync(CancellationToken cancellationToken)
    {
      JsonTextReader reader = this;
      if (await reader.EnsureCharsAsync(0, false, cancellationToken).ConfigureAwait(false))
      {
        ConfiguredTaskAwaitable configuredTaskAwaitable = reader.EatWhitespaceAsync(cancellationToken).ConfigureAwait(false);
        await configuredTaskAwaitable;
        if (reader._isEndOfFile)
        {
          reader.SetToken(JsonToken.None);
          return false;
        }
        if (reader._chars[reader._charPos] != '/')
          throw JsonReaderException.Create((JsonReader) reader, "Additional text encountered after finished reading JSON content: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) reader._chars[reader._charPos]));
        configuredTaskAwaitable = reader.ParseCommentAsync(true, cancellationToken).ConfigureAwait(false);
        await configuredTaskAwaitable;
        return true;
      }
      reader.SetToken(JsonToken.None);
      return false;
    }

    private Task<int> ReadDataAsync(bool append, CancellationToken cancellationToken) => this.ReadDataAsync(append, 0, cancellationToken);

    private async Task<int> ReadDataAsync(
      bool append,
      int charsRequired,
      CancellationToken cancellationToken)
    {
      if (this._isEndOfFile)
        return 0;
      this.PrepareBufferForReadData(append, charsRequired);
      int num = await this._reader.ReadAsync(this._chars, this._charsUsed, this._chars.Length - this._charsUsed - 1, cancellationToken).ConfigureAwait(false);
      this._charsUsed += num;
      if (num == 0)
        this._isEndOfFile = true;
      this._chars[this._charsUsed] = char.MinValue;
      return num;
    }

    private async Task<bool> ParseValueAsync(CancellationToken cancellationToken)
    {
      JsonTextReader jsonTextReader = this;
      char ch;
      while (true)
      {
        do
        {
          ch = jsonTextReader._chars[jsonTextReader._charPos];
          switch (ch)
          {
            case char.MinValue:
              if (jsonTextReader._charsUsed == jsonTextReader._charPos)
                continue;
              goto label_5;
            case '\t':
            case ' ':
              goto label_32;
            case '\n':
              goto label_31;
            case '\r':
              goto label_30;
            case '"':
            case '\'':
              goto label_6;
            case ')':
              goto label_29;
            case ',':
              goto label_28;
            case '-':
              goto label_19;
            case '/':
              goto label_23;
            case 'I':
              goto label_18;
            case 'N':
              goto label_17;
            case '[':
              goto label_26;
            case ']':
              goto label_27;
            case 'f':
              goto label_9;
            case 'n':
              goto label_10;
            case 't':
              goto label_8;
            case 'u':
              goto label_24;
            case '{':
              goto label_25;
            default:
              goto label_33;
          }
        }
        while (await jsonTextReader.ReadDataAsync(false, cancellationToken).ConfigureAwait(false) != 0);
        break;
label_5:
        ++jsonTextReader._charPos;
        continue;
label_30:
        await jsonTextReader.ProcessCarriageReturnAsync(false, cancellationToken).ConfigureAwait(false);
        continue;
label_31:
        jsonTextReader.ProcessLineFeed();
        continue;
label_32:
        ++jsonTextReader._charPos;
        continue;
label_33:
        if (char.IsWhiteSpace(ch))
          ++jsonTextReader._charPos;
        else
          goto label_35;
      }
      return false;
label_6:
      await jsonTextReader.ParseStringAsync(ch, ReadType.Read, cancellationToken).ConfigureAwait(false);
      return true;
label_8:
      await jsonTextReader.ParseTrueAsync(cancellationToken).ConfigureAwait(false);
      return true;
label_9:
      await jsonTextReader.ParseFalseAsync(cancellationToken).ConfigureAwait(false);
      return true;
label_10:
      if (await jsonTextReader.EnsureCharsAsync(1, true, cancellationToken).ConfigureAwait(false))
      {
        switch (jsonTextReader._chars[jsonTextReader._charPos + 1])
        {
          case 'e':
            await jsonTextReader.ParseConstructorAsync(cancellationToken).ConfigureAwait(false);
            break;
          case 'u':
            await jsonTextReader.ParseNullAsync(cancellationToken).ConfigureAwait(false);
            break;
          default:
            throw jsonTextReader.CreateUnexpectedCharacterException(jsonTextReader._chars[jsonTextReader._charPos]);
        }
        return true;
      }
      ++jsonTextReader._charPos;
      throw jsonTextReader.CreateUnexpectedEndException();
label_17:
      object obj1 = await jsonTextReader.ParseNumberNaNAsync(ReadType.Read, cancellationToken).ConfigureAwait(false);
      return true;
label_18:
      object obj2 = await jsonTextReader.ParseNumberPositiveInfinityAsync(ReadType.Read, cancellationToken).ConfigureAwait(false);
      return true;
label_19:
      if (await jsonTextReader.EnsureCharsAsync(1, true, cancellationToken).ConfigureAwait(false) && jsonTextReader._chars[jsonTextReader._charPos + 1] == 'I')
      {
        object obj3 = await jsonTextReader.ParseNumberNegativeInfinityAsync(ReadType.Read, cancellationToken).ConfigureAwait(false);
      }
      else
        await jsonTextReader.ParseNumberAsync(ReadType.Read, cancellationToken).ConfigureAwait(false);
      return true;
label_23:
      await jsonTextReader.ParseCommentAsync(true, cancellationToken).ConfigureAwait(false);
      return true;
label_24:
      await jsonTextReader.ParseUndefinedAsync(cancellationToken).ConfigureAwait(false);
      return true;
label_25:
      ++jsonTextReader._charPos;
      jsonTextReader.SetToken(JsonToken.StartObject);
      return true;
label_26:
      ++jsonTextReader._charPos;
      jsonTextReader.SetToken(JsonToken.StartArray);
      return true;
label_27:
      ++jsonTextReader._charPos;
      jsonTextReader.SetToken(JsonToken.EndArray);
      return true;
label_28:
      jsonTextReader.SetToken(JsonToken.Undefined);
      return true;
label_29:
      ++jsonTextReader._charPos;
      jsonTextReader.SetToken(JsonToken.EndConstructor);
      return true;
label_35:
      if (!char.IsNumber(ch) && ch != '-' && ch != '.')
        throw jsonTextReader.CreateUnexpectedCharacterException(ch);
      await jsonTextReader.ParseNumberAsync(ReadType.Read, cancellationToken).ConfigureAwait(false);
      return true;
    }

    private async Task ReadStringIntoBufferAsync(char quote, CancellationToken cancellationToken)
    {
      JsonTextReader reader = this;
      int charPos = reader._charPos;
      int initialPosition = reader._charPos;
      int lastWritePosition = reader._charPos;
      reader._stringBuffer.Position = 0;
      do
      {
        char ch1 = reader._chars[charPos++];
        if (ch1 <= '\r')
        {
          if (ch1 != char.MinValue)
          {
            if (ch1 != '\n')
            {
              if (ch1 == '\r')
              {
                reader._charPos = charPos - 1;
                await reader.ProcessCarriageReturnAsync(true, cancellationToken).ConfigureAwait(false);
                charPos = reader._charPos;
              }
            }
            else
            {
              reader._charPos = charPos - 1;
              reader.ProcessLineFeed();
              charPos = reader._charPos;
            }
          }
          else if (reader._charsUsed == charPos - 1)
          {
            --charPos;
            if (await reader.ReadDataAsync(true, cancellationToken).ConfigureAwait(false) == 0)
            {
              reader._charPos = charPos;
              throw JsonReaderException.Create((JsonReader) reader, "Unterminated string. Expected delimiter: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) quote));
            }
          }
        }
        else if (ch1 != '"' && ch1 != '\'')
        {
          if (ch1 == '\\')
          {
            reader._charPos = charPos;
            if (!await reader.EnsureCharsAsync(0, true, cancellationToken).ConfigureAwait(false))
              throw JsonReaderException.Create((JsonReader) reader, "Unterminated string. Expected delimiter: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) quote));
            int escapeStartPos = charPos - 1;
            char ch2 = reader._chars[charPos];
            ++charPos;
            char writeChar;
            switch (ch2)
            {
              case '"':
              case '\'':
              case '/':
                writeChar = ch2;
                break;
              case '\\':
                writeChar = '\\';
                break;
              case 'b':
                writeChar = '\b';
                break;
              case 'f':
                writeChar = '\f';
                break;
              case 'n':
                writeChar = '\n';
                break;
              case 'r':
                writeChar = '\r';
                break;
              case 't':
                writeChar = '\t';
                break;
              case 'u':
                reader._charPos = charPos;
                ConfiguredTaskAwaitable<char> configuredTaskAwaitable = reader.ParseUnicodeAsync(cancellationToken).ConfigureAwait(false);
                writeChar = await configuredTaskAwaitable;
                if (StringUtils.IsLowSurrogate(writeChar))
                  writeChar = '�';
                else if (StringUtils.IsHighSurrogate(writeChar))
                {
                  bool anotherHighSurrogate;
                  do
                  {
                    anotherHighSurrogate = false;
                    if (await reader.EnsureCharsAsync(2, true, cancellationToken).ConfigureAwait(false) && reader._chars[reader._charPos] == '\\' && reader._chars[reader._charPos + 1] == 'u')
                    {
                      char highSurrogate = writeChar;
                      reader._charPos += 2;
                      configuredTaskAwaitable = reader.ParseUnicodeAsync(cancellationToken).ConfigureAwait(false);
                      writeChar = await configuredTaskAwaitable;
                      if (!StringUtils.IsLowSurrogate(writeChar))
                      {
                        if (StringUtils.IsHighSurrogate(writeChar))
                        {
                          highSurrogate = '�';
                          anotherHighSurrogate = true;
                        }
                        else
                          highSurrogate = '�';
                      }
                      reader.EnsureBufferNotEmpty();
                      reader.WriteCharToBuffer(highSurrogate, lastWritePosition, escapeStartPos);
                      lastWritePosition = reader._charPos;
                    }
                    else
                      writeChar = '�';
                  }
                  while (anotherHighSurrogate);
                }
                charPos = reader._charPos;
                break;
              default:
                reader._charPos = charPos;
                throw JsonReaderException.Create((JsonReader) reader, "Bad JSON escape sequence: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) ("\\" + ch2.ToString())));
            }
            reader.EnsureBufferNotEmpty();
            reader.WriteCharToBuffer(writeChar, lastWritePosition, escapeStartPos);
            lastWritePosition = charPos;
          }
        }
      }
      while ((int) reader._chars[charPos - 1] != (int) quote);
      reader.FinishReadStringIntoBuffer(charPos - 1, initialPosition, lastWritePosition);
    }

    private Task ProcessCarriageReturnAsync(bool append, CancellationToken cancellationToken)
    {
      ++this._charPos;
      Task<bool> task = this.EnsureCharsAsync(1, append, cancellationToken);
      if (!task.IsCompletedSucessfully())
        return this.ProcessCarriageReturnAsync(task);
      this.SetNewLine(task.Result);
      return AsyncUtils.CompletedTask;
    }

    private async Task ProcessCarriageReturnAsync(Task<bool> task) => this.SetNewLine(await task.ConfigureAwait(false));

    private async Task<char> ParseUnicodeAsync(CancellationToken cancellationToken) => this.ConvertUnicode(await this.EnsureCharsAsync(4, true, cancellationToken).ConfigureAwait(false));

    private Task<bool> EnsureCharsAsync(
      int relativePosition,
      bool append,
      CancellationToken cancellationToken)
    {
      if (this._charPos + relativePosition < this._charsUsed)
        return AsyncUtils.True;
      return this._isEndOfFile ? AsyncUtils.False : this.ReadCharsAsync(relativePosition, append, cancellationToken);
    }

    private async Task<bool> ReadCharsAsync(
      int relativePosition,
      bool append,
      CancellationToken cancellationToken)
    {
      int charsRequired = this._charPos + relativePosition - this._charsUsed + 1;
      do
      {
        int num = await this.ReadDataAsync(append, charsRequired, cancellationToken).ConfigureAwait(false);
        if (num == 0)
          return false;
        charsRequired -= num;
      }
      while (charsRequired > 0);
      return true;
    }

    private async Task<bool> ParseObjectAsync(CancellationToken cancellationToken)
    {
      JsonTextReader jsonTextReader = this;
      while (true)
      {
        char c;
        do
        {
          c = jsonTextReader._chars[jsonTextReader._charPos];
          switch (c)
          {
            case char.MinValue:
              if (jsonTextReader._charsUsed == jsonTextReader._charPos)
                continue;
              goto label_5;
            case '\t':
            case ' ':
              goto label_10;
            case '\n':
              goto label_9;
            case '\r':
              goto label_8;
            case '/':
              goto label_7;
            case '}':
              goto label_6;
            default:
              goto label_11;
          }
        }
        while (await jsonTextReader.ReadDataAsync(false, cancellationToken).ConfigureAwait(false) != 0);
        break;
label_5:
        ++jsonTextReader._charPos;
        continue;
label_8:
        await jsonTextReader.ProcessCarriageReturnAsync(false, cancellationToken).ConfigureAwait(false);
        continue;
label_9:
        jsonTextReader.ProcessLineFeed();
        continue;
label_10:
        ++jsonTextReader._charPos;
        continue;
label_11:
        if (char.IsWhiteSpace(c))
          ++jsonTextReader._charPos;
        else
          goto label_13;
      }
      return false;
label_6:
      jsonTextReader.SetToken(JsonToken.EndObject);
      ++jsonTextReader._charPos;
      return true;
label_7:
      await jsonTextReader.ParseCommentAsync(true, cancellationToken).ConfigureAwait(false);
      return true;
label_13:
      return await jsonTextReader.ParsePropertyAsync(cancellationToken).ConfigureAwait(false);
    }

    private async Task ParseCommentAsync(bool setToken, CancellationToken cancellationToken)
    {
      JsonTextReader reader = this;
      ++reader._charPos;
      if (!await reader.EnsureCharsAsync(1, false, cancellationToken).ConfigureAwait(false))
        throw JsonReaderException.Create((JsonReader) reader, "Unexpected end while parsing comment.");
      bool singlelineComment;
      if (reader._chars[reader._charPos] == '*')
      {
        singlelineComment = false;
      }
      else
      {
        if (reader._chars[reader._charPos] != '/')
          throw JsonReaderException.Create((JsonReader) reader, "Error parsing comment. Expected: *, got {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) reader._chars[reader._charPos]));
        singlelineComment = true;
      }
      ++reader._charPos;
      int initialPosition = reader._charPos;
      while (true)
      {
        do
        {
          do
          {
            do
            {
              switch (reader._chars[reader._charPos])
              {
                case char.MinValue:
                  if (reader._charsUsed == reader._charPos)
                    continue;
                  goto label_15;
                case '\n':
                  goto label_22;
                case '\r':
                  goto label_19;
                case '*':
                  goto label_16;
                default:
                  goto label_25;
              }
            }
            while (await reader.ReadDataAsync(true, cancellationToken).ConfigureAwait(false) != 0);
            if (!singlelineComment)
              throw JsonReaderException.Create((JsonReader) reader, "Unexpected end while parsing comment.");
            reader.EndComment(setToken, initialPosition, reader._charPos);
            return;
label_15:
            ++reader._charPos;
            continue;
label_16:
            ++reader._charPos;
          }
          while (singlelineComment);
        }
        while (!await reader.EnsureCharsAsync(0, true, cancellationToken).ConfigureAwait(false) || reader._chars[reader._charPos] != '/');
        break;
label_19:
        if (!singlelineComment)
        {
          await reader.ProcessCarriageReturnAsync(true, cancellationToken).ConfigureAwait(false);
          continue;
        }
        goto label_20;
label_22:
        if (!singlelineComment)
        {
          reader.ProcessLineFeed();
          continue;
        }
        goto label_23;
label_25:
        ++reader._charPos;
      }
      reader.EndComment(setToken, initialPosition, reader._charPos - 1);
      ++reader._charPos;
      return;
label_20:
      reader.EndComment(setToken, initialPosition, reader._charPos);
      return;
label_23:
      reader.EndComment(setToken, initialPosition, reader._charPos);
    }

    private async Task EatWhitespaceAsync(CancellationToken cancellationToken)
    {
      while (true)
      {
        char c;
        do
        {
          c = this._chars[this._charPos];
          switch (c)
          {
            case char.MinValue:
              if (this._charsUsed == this._charPos)
                continue;
              goto label_5;
            case '\n':
              goto label_8;
            case '\r':
              goto label_7;
            case ' ':
              goto label_10;
            default:
              goto label_9;
          }
        }
        while (await this.ReadDataAsync(false, cancellationToken).ConfigureAwait(false) != 0);
        break;
label_5:
        ++this._charPos;
        continue;
label_7:
        await this.ProcessCarriageReturnAsync(false, cancellationToken).ConfigureAwait(false);
        continue;
label_8:
        this.ProcessLineFeed();
        continue;
label_9:
        if (!char.IsWhiteSpace(c))
          goto label_6;
label_10:
        ++this._charPos;
      }
      return;
label_6:;
    }

    private async Task ParseStringAsync(
      char quote,
      ReadType readType,
      CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();
      ++this._charPos;
      this.ShiftBufferIfNeeded();
      await this.ReadStringIntoBufferAsync(quote, cancellationToken).ConfigureAwait(false);
      this.ParseReadString(quote, readType);
    }

    private async Task<bool> MatchValueAsync(string value, CancellationToken cancellationToken) => this.MatchValue(await this.EnsureCharsAsync(value.Length - 1, true, cancellationToken).ConfigureAwait(false), value);

    private async Task<bool> MatchValueWithTrailingSeparatorAsync(
      string value,
      CancellationToken cancellationToken)
    {
      return await this.MatchValueAsync(value, cancellationToken).ConfigureAwait(false) && (!await this.EnsureCharsAsync(0, false, cancellationToken).ConfigureAwait(false) || this.IsSeparator(this._chars[this._charPos]) || this._chars[this._charPos] == char.MinValue);
    }

    private async Task MatchAndSetAsync(
      string value,
      JsonToken newToken,
      object? tokenValue,
      CancellationToken cancellationToken)
    {
      JsonTextReader reader = this;
      if (!await reader.MatchValueWithTrailingSeparatorAsync(value, cancellationToken).ConfigureAwait(false))
        throw JsonReaderException.Create((JsonReader) reader, "Error parsing " + newToken.ToString().ToLowerInvariant() + " value.");
      reader.SetToken(newToken, tokenValue);
    }

    private Task ParseTrueAsync(CancellationToken cancellationToken) => this.MatchAndSetAsync(JsonConvert.True, JsonToken.Boolean, (object) true, cancellationToken);

    private Task ParseFalseAsync(CancellationToken cancellationToken) => this.MatchAndSetAsync(JsonConvert.False, JsonToken.Boolean, (object) false, cancellationToken);

    private Task ParseNullAsync(CancellationToken cancellationToken) => this.MatchAndSetAsync(JsonConvert.Null, JsonToken.Null, (object) null, cancellationToken);

    private async Task ParseConstructorAsync(CancellationToken cancellationToken)
    {
      JsonTextReader reader = this;
      ConfiguredTaskAwaitable configuredTaskAwaitable = await reader.MatchValueWithTrailingSeparatorAsync("new", cancellationToken).ConfigureAwait(false) ? reader.EatWhitespaceAsync(cancellationToken).ConfigureAwait(false) : throw JsonReaderException.Create((JsonReader) reader, "Unexpected content while parsing JSON.");
      await configuredTaskAwaitable;
      int initialPosition = reader._charPos;
      char c;
      while (true)
      {
        do
        {
          c = reader._chars[reader._charPos];
          if (c == char.MinValue)
          {
            if (reader._charsUsed != reader._charPos)
              goto label_8;
          }
          else
            goto label_9;
        }
        while (await reader.ReadDataAsync(true, cancellationToken).ConfigureAwait(false) != 0);
        break;
label_9:
        if (char.IsLetterOrDigit(c))
          ++reader._charPos;
        else
          goto label_11;
      }
      throw JsonReaderException.Create((JsonReader) reader, "Unexpected end while parsing constructor.");
label_8:
      int endPosition = reader._charPos;
      ++reader._charPos;
      goto label_20;
label_11:
      switch (c)
      {
        case '\n':
          endPosition = reader._charPos;
          reader.ProcessLineFeed();
          break;
        case '\r':
          endPosition = reader._charPos;
          configuredTaskAwaitable = reader.ProcessCarriageReturnAsync(true, cancellationToken).ConfigureAwait(false);
          await configuredTaskAwaitable;
          break;
        default:
          if (char.IsWhiteSpace(c))
          {
            endPosition = reader._charPos;
            ++reader._charPos;
            break;
          }
          if (c != '(')
            throw JsonReaderException.Create((JsonReader) reader, "Unexpected character while parsing constructor: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) c));
          endPosition = reader._charPos;
          break;
      }
label_20:
      reader._stringReference = new StringReference(reader._chars, initialPosition, endPosition - initialPosition);
      string constructorName = reader._stringReference.ToString();
      configuredTaskAwaitable = reader.EatWhitespaceAsync(cancellationToken).ConfigureAwait(false);
      await configuredTaskAwaitable;
      if (reader._chars[reader._charPos] != '(')
        throw JsonReaderException.Create((JsonReader) reader, "Unexpected character while parsing constructor: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) reader._chars[reader._charPos]));
      ++reader._charPos;
      reader.ClearRecentString();
      reader.SetToken(JsonToken.StartConstructor, (object) constructorName);
      constructorName = (string) null;
    }

    private async Task<object> ParseNumberNaNAsync(
      ReadType readType,
      CancellationToken cancellationToken)
    {
      ReadType readType1 = readType;
      bool matched = await this.MatchValueWithTrailingSeparatorAsync(JsonConvert.NaN, cancellationToken).ConfigureAwait(false);
      return this.ParseNumberNaN(readType1, matched);
    }

    private async Task<object> ParseNumberPositiveInfinityAsync(
      ReadType readType,
      CancellationToken cancellationToken)
    {
      ReadType readType1 = readType;
      bool matched = await this.MatchValueWithTrailingSeparatorAsync(JsonConvert.PositiveInfinity, cancellationToken).ConfigureAwait(false);
      return this.ParseNumberPositiveInfinity(readType1, matched);
    }

    private async Task<object> ParseNumberNegativeInfinityAsync(
      ReadType readType,
      CancellationToken cancellationToken)
    {
      ReadType readType1 = readType;
      bool matched = await this.MatchValueWithTrailingSeparatorAsync(JsonConvert.NegativeInfinity, cancellationToken).ConfigureAwait(false);
      return this.ParseNumberNegativeInfinity(readType1, matched);
    }

    private async Task ParseNumberAsync(ReadType readType, CancellationToken cancellationToken)
    {
      this.ShiftBufferIfNeeded();
      char firstChar = this._chars[this._charPos];
      int initialPosition = this._charPos;
      await this.ReadNumberIntoBufferAsync(cancellationToken).ConfigureAwait(false);
      this.ParseReadNumber(readType, firstChar, initialPosition);
    }

    private Task ParseUndefinedAsync(CancellationToken cancellationToken) => this.MatchAndSetAsync(JsonConvert.Undefined, JsonToken.Undefined, (object) null, cancellationToken);

    private async Task<bool> ParsePropertyAsync(CancellationToken cancellationToken)
    {
      JsonTextReader reader = this;
      char ch = reader._chars[reader._charPos];
      char quoteChar;
      switch (ch)
      {
        case '"':
        case '\'':
          ++reader._charPos;
          quoteChar = ch;
          reader.ShiftBufferIfNeeded();
          await reader.ReadStringIntoBufferAsync(quoteChar, cancellationToken).ConfigureAwait(false);
          break;
        default:
          if (!reader.ValidIdentifierChar(ch))
            throw JsonReaderException.Create((JsonReader) reader, "Invalid property identifier character: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) reader._chars[reader._charPos]));
          quoteChar = char.MinValue;
          reader.ShiftBufferIfNeeded();
          await reader.ParseUnquotedPropertyAsync(cancellationToken).ConfigureAwait(false);
          break;
      }
      string propertyName = reader.PropertyNameTable == null ? reader._stringReference.ToString() : reader.PropertyNameTable.Get(reader._stringReference.Chars, reader._stringReference.StartIndex, reader._stringReference.Length) ?? reader._stringReference.ToString();
      await reader.EatWhitespaceAsync(cancellationToken).ConfigureAwait(false);
      if (reader._chars[reader._charPos] != ':')
        throw JsonReaderException.Create((JsonReader) reader, "Invalid character after parsing property name. Expected ':' but got: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) reader._chars[reader._charPos]));
      ++reader._charPos;
      reader.SetToken(JsonToken.PropertyName, (object) propertyName);
      reader._quoteChar = quoteChar;
      reader.ClearRecentString();
      return true;
    }

    private async Task ReadNumberIntoBufferAsync(CancellationToken cancellationToken)
    {
      int charPos = this._charPos;
      while (true)
      {
        char currentChar;
        do
        {
          currentChar = this._chars[charPos];
          if (currentChar == char.MinValue)
          {
            this._charPos = charPos;
            if (this._charsUsed != charPos)
              goto label_7;
          }
          else
            goto label_5;
        }
        while (await this.ReadDataAsync(true, cancellationToken).ConfigureAwait(false) != 0);
        goto label_8;
label_5:
        if (!this.ReadNumberCharIntoBuffer(currentChar, charPos))
          ++charPos;
        else
          goto label_3;
      }
label_7:
      return;
label_8:
      return;
label_3:;
    }

    private async Task ParseUnquotedPropertyAsync(CancellationToken cancellationToken)
    {
      JsonTextReader reader = this;
      int initialPosition = reader._charPos;
      char currentChar;
      do
      {
        currentChar = reader._chars[reader._charPos];
        if (currentChar == char.MinValue)
        {
          if (reader._charsUsed == reader._charPos)
          {
            if (await reader.ReadDataAsync(true, cancellationToken).ConfigureAwait(false) == 0)
              throw JsonReaderException.Create((JsonReader) reader, "Unexpected end while parsing unquoted property name.");
          }
          else
          {
            reader._stringReference = new StringReference(reader._chars, initialPosition, reader._charPos - initialPosition);
            break;
          }
        }
      }
      while (!reader.ReadUnquotedPropertyReportIfDone(currentChar, initialPosition));
    }

    private async Task<bool> ReadNullCharAsync(CancellationToken cancellationToken)
    {
      if (this._charsUsed == this._charPos)
      {
        if (await this.ReadDataAsync(false, cancellationToken).ConfigureAwait(false) == 0)
        {
          this._isEndOfFile = true;
          return true;
        }
      }
      else
        ++this._charPos;
      return false;
    }

    private async Task HandleNullAsync(CancellationToken cancellationToken)
    {
      JsonTextReader jsonTextReader = this;
      if (await jsonTextReader.EnsureCharsAsync(1, true, cancellationToken).ConfigureAwait(false))
      {
        if (jsonTextReader._chars[jsonTextReader._charPos + 1] == 'u')
        {
          await jsonTextReader.ParseNullAsync(cancellationToken).ConfigureAwait(false);
        }
        else
        {
          jsonTextReader._charPos += 2;
          throw jsonTextReader.CreateUnexpectedCharacterException(jsonTextReader._chars[jsonTextReader._charPos - 1]);
        }
      }
      else
      {
        jsonTextReader._charPos = jsonTextReader._charsUsed;
        throw jsonTextReader.CreateUnexpectedEndException();
      }
    }

    private async Task ReadFinishedAsync(CancellationToken cancellationToken)
    {
      JsonTextReader reader = this;
      if (await reader.EnsureCharsAsync(0, false, cancellationToken).ConfigureAwait(false))
      {
        ConfiguredTaskAwaitable configuredTaskAwaitable = reader.EatWhitespaceAsync(cancellationToken).ConfigureAwait(false);
        await configuredTaskAwaitable;
        if (reader._isEndOfFile)
        {
          reader.SetToken(JsonToken.None);
          return;
        }
        if (reader._chars[reader._charPos] != '/')
          throw JsonReaderException.Create((JsonReader) reader, "Additional text encountered after finished reading JSON content: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) reader._chars[reader._charPos]));
        configuredTaskAwaitable = reader.ParseCommentAsync(false, cancellationToken).ConfigureAwait(false);
        await configuredTaskAwaitable;
      }
      reader.SetToken(JsonToken.None);
    }

    private async Task<object?> ReadStringValueAsync(
      ReadType readType,
      CancellationToken cancellationToken)
    {
      JsonTextReader reader = this;
      reader.EnsureBuffer();
      object obj;
      switch (reader._currentState)
      {
        case JsonReader.State.Start:
        case JsonReader.State.Property:
        case JsonReader.State.ArrayStart:
        case JsonReader.State.Array:
        case JsonReader.State.ConstructorStart:
        case JsonReader.State.Constructor:
          string expected;
          char ch;
          ConfiguredTaskAwaitable<bool> configuredTaskAwaitable;
          while (true)
          {
            ch = reader._chars[reader._charPos];
            switch (ch)
            {
              case char.MinValue:
                configuredTaskAwaitable = reader.ReadNullCharAsync(cancellationToken).ConfigureAwait(false);
                if (!await configuredTaskAwaitable)
                  break;
                goto label_6;
              case '\t':
              case ' ':
                ++reader._charPos;
                break;
              case '\n':
                reader.ProcessLineFeed();
                break;
              case '\r':
                await reader.ProcessCarriageReturnAsync(false, cancellationToken).ConfigureAwait(false);
                break;
              case '"':
              case '\'':
                goto label_7;
              case ',':
                reader.ProcessValueComma();
                break;
              case '-':
                goto label_9;
              case '.':
              case '0':
              case '1':
              case '2':
              case '3':
              case '4':
              case '5':
              case '6':
              case '7':
              case '8':
              case '9':
                goto label_13;
              case '/':
                await reader.ParseCommentAsync(false, cancellationToken).ConfigureAwait(false);
                break;
              case 'I':
                goto label_22;
              case 'N':
                goto label_23;
              case ']':
                goto label_27;
              case 'f':
              case 't':
                goto label_16;
              case 'n':
                goto label_24;
              default:
                ++reader._charPos;
                if (char.IsWhiteSpace(ch))
                  break;
                goto label_34;
            }
            expected = (string) null;
          }
label_6:
          reader.SetToken(JsonToken.None, (object) null, false);
          return (object) null;
label_7:
          await reader.ParseStringAsync(ch, readType, cancellationToken).ConfigureAwait(false);
          return reader.FinishReadQuotedStringValue(readType);
label_9:
          configuredTaskAwaitable = reader.EnsureCharsAsync(1, true, cancellationToken).ConfigureAwait(false);
          if (await configuredTaskAwaitable && reader._chars[reader._charPos + 1] == 'I')
            return reader.ParseNumberNegativeInfinity(readType);
          await reader.ParseNumberAsync(readType, cancellationToken).ConfigureAwait(false);
          return reader.Value;
label_13:
          if (readType != ReadType.ReadAsString)
          {
            ++reader._charPos;
            throw reader.CreateUnexpectedCharacterException(ch);
          }
          await reader.ParseNumberAsync(ReadType.ReadAsString, cancellationToken).ConfigureAwait(false);
          return reader.Value;
label_16:
          if (readType != ReadType.ReadAsString)
          {
            ++reader._charPos;
            throw reader.CreateUnexpectedCharacterException(ch);
          }
          expected = ch == 't' ? JsonConvert.True : JsonConvert.False;
          configuredTaskAwaitable = reader.MatchValueWithTrailingSeparatorAsync(expected, cancellationToken).ConfigureAwait(false);
          if (!await configuredTaskAwaitable)
            throw reader.CreateUnexpectedCharacterException(reader._chars[reader._charPos]);
          reader.SetToken(JsonToken.String, (object) expected);
          return (object) expected;
label_22:
          return await reader.ParseNumberPositiveInfinityAsync(readType, cancellationToken).ConfigureAwait(false);
label_23:
          return await reader.ParseNumberNaNAsync(readType, cancellationToken).ConfigureAwait(false);
label_24:
          await reader.HandleNullAsync(cancellationToken).ConfigureAwait(false);
          return obj;
label_27:
          ++reader._charPos;
          if (reader._currentState != JsonReader.State.Array && reader._currentState != JsonReader.State.ArrayStart && reader._currentState != JsonReader.State.PostValue)
            throw reader.CreateUnexpectedCharacterException(ch);
          reader.SetToken(JsonToken.EndArray);
          return (object) null;
label_34:
          throw reader.CreateUnexpectedCharacterException(ch);
        case JsonReader.State.PostValue:
          if (await reader.ParsePostValueAsync(true, cancellationToken).ConfigureAwait(false))
            return (object) null;
          goto case JsonReader.State.Start;
        case JsonReader.State.Finished:
          await reader.ReadFinishedAsync(cancellationToken).ConfigureAwait(false);
          return obj;
        default:
          throw JsonReaderException.Create((JsonReader) reader, "Unexpected state: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) reader.CurrentState));
      }
    }

    private async Task<object?> ReadNumberValueAsync(
      ReadType readType,
      CancellationToken cancellationToken)
    {
      JsonTextReader reader = this;
      reader.EnsureBuffer();
      object obj;
      switch (reader._currentState)
      {
        case JsonReader.State.Start:
        case JsonReader.State.Property:
        case JsonReader.State.ArrayStart:
        case JsonReader.State.Array:
        case JsonReader.State.ConstructorStart:
        case JsonReader.State.Constructor:
          char ch;
          do
          {
            ConfiguredTaskAwaitable<bool> configuredTaskAwaitable;
            do
            {
              ch = reader._chars[reader._charPos];
              switch (ch)
              {
                case char.MinValue:
                  configuredTaskAwaitable = reader.ReadNullCharAsync(cancellationToken).ConfigureAwait(false);
                  continue;
                case '\t':
                case ' ':
                  goto label_25;
                case '\n':
                  goto label_24;
                case '\r':
                  goto label_23;
                case '"':
                case '\'':
                  goto label_7;
                case ',':
                  goto label_19;
                case '-':
                  goto label_12;
                case '.':
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                  goto label_16;
                case '/':
                  goto label_18;
                case 'I':
                  goto label_11;
                case 'N':
                  goto label_10;
                case ']':
                  goto label_20;
                case 'n':
                  goto label_9;
                default:
                  goto label_26;
              }
            }
            while (!await configuredTaskAwaitable);
            reader.SetToken(JsonToken.None, (object) null, false);
            return (object) null;
label_7:
            await reader.ParseStringAsync(ch, readType, cancellationToken).ConfigureAwait(false);
            return reader.FinishReadQuotedNumber(readType);
label_9:
            await reader.HandleNullAsync(cancellationToken).ConfigureAwait(false);
            return obj;
label_10:
            return await reader.ParseNumberNaNAsync(readType, cancellationToken).ConfigureAwait(false);
label_11:
            return await reader.ParseNumberPositiveInfinityAsync(readType, cancellationToken).ConfigureAwait(false);
label_12:
            configuredTaskAwaitable = reader.EnsureCharsAsync(1, true, cancellationToken).ConfigureAwait(false);
            if (await configuredTaskAwaitable && reader._chars[reader._charPos + 1] == 'I')
              return await reader.ParseNumberNegativeInfinityAsync(readType, cancellationToken).ConfigureAwait(false);
            await reader.ParseNumberAsync(readType, cancellationToken).ConfigureAwait(false);
            return reader.Value;
label_16:
            await reader.ParseNumberAsync(readType, cancellationToken).ConfigureAwait(false);
            return reader.Value;
label_18:
            await reader.ParseCommentAsync(false, cancellationToken).ConfigureAwait(false);
            continue;
label_19:
            reader.ProcessValueComma();
            continue;
label_20:
            ++reader._charPos;
            if (reader._currentState != JsonReader.State.Array && reader._currentState != JsonReader.State.ArrayStart && reader._currentState != JsonReader.State.PostValue)
              throw reader.CreateUnexpectedCharacterException(ch);
            reader.SetToken(JsonToken.EndArray);
            return (object) null;
label_23:
            await reader.ProcessCarriageReturnAsync(false, cancellationToken).ConfigureAwait(false);
            continue;
label_24:
            reader.ProcessLineFeed();
            continue;
label_25:
            ++reader._charPos;
            continue;
label_26:
            ++reader._charPos;
          }
          while (char.IsWhiteSpace(ch));
          throw reader.CreateUnexpectedCharacterException(ch);
        case JsonReader.State.PostValue:
          if (await reader.ParsePostValueAsync(true, cancellationToken).ConfigureAwait(false))
            return (object) null;
          goto case JsonReader.State.Start;
        case JsonReader.State.Finished:
          await reader.ReadFinishedAsync(cancellationToken).ConfigureAwait(false);
          return obj;
        default:
          throw JsonReaderException.Create((JsonReader) reader, "Unexpected state: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) reader.CurrentState));
      }
    }

    public override Task<bool?> ReadAsBooleanAsync(CancellationToken cancellationToken = default (CancellationToken)) => !this._safeAsync ? base.ReadAsBooleanAsync(cancellationToken) : this.DoReadAsBooleanAsync(cancellationToken);

    internal async Task<bool?> DoReadAsBooleanAsync(CancellationToken cancellationToken)
    {
      JsonTextReader reader = this;
      reader.EnsureBuffer();
      switch (reader._currentState)
      {
        case JsonReader.State.Start:
        case JsonReader.State.Property:
        case JsonReader.State.ArrayStart:
        case JsonReader.State.Array:
        case JsonReader.State.ConstructorStart:
        case JsonReader.State.Constructor:
          BigInteger i;
          char ch;
          ConfiguredTaskAwaitable<bool> configuredTaskAwaitable;
          while (true)
          {
            ch = reader._chars[reader._charPos];
            switch (ch)
            {
              case char.MinValue:
                configuredTaskAwaitable = reader.ReadNullCharAsync(cancellationToken).ConfigureAwait(false);
                if (!await configuredTaskAwaitable)
                  break;
                goto label_6;
              case '\t':
              case ' ':
                ++reader._charPos;
                break;
              case '\n':
                reader.ProcessLineFeed();
                break;
              case '\r':
                await reader.ProcessCarriageReturnAsync(false, cancellationToken).ConfigureAwait(false);
                break;
              case '"':
              case '\'':
                goto label_7;
              case ',':
                reader.ProcessValueComma();
                break;
              case '-':
              case '.':
              case '0':
              case '1':
              case '2':
              case '3':
              case '4':
              case '5':
              case '6':
              case '7':
              case '8':
              case '9':
                goto label_10;
              case '/':
                await reader.ParseCommentAsync(false, cancellationToken).ConfigureAwait(false);
                break;
              case ']':
                goto label_21;
              case 'f':
              case 't':
                goto label_15;
              case 'n':
                goto label_9;
              default:
                ++reader._charPos;
                if (char.IsWhiteSpace(ch))
                  break;
                goto label_28;
            }
            i = new BigInteger();
          }
label_6:
          reader.SetToken(JsonToken.None, (object) null, false);
          return new bool?();
label_7:
          await reader.ParseStringAsync(ch, ReadType.Read, cancellationToken).ConfigureAwait(false);
          return reader.ReadBooleanString(reader._stringReference.ToString());
label_9:
          await reader.HandleNullAsync(cancellationToken).ConfigureAwait(false);
          return new bool?();
label_10:
          await reader.ParseNumberAsync(ReadType.Read, cancellationToken).ConfigureAwait(false);
          bool flag;
          if (reader.Value is BigInteger bigInteger)
          {
            i = bigInteger;
            flag = i != 0L;
          }
          else
            flag = Convert.ToBoolean(reader.Value, (IFormatProvider) CultureInfo.InvariantCulture);
          reader.SetToken(JsonToken.Boolean, (object) flag, false);
          return new bool?(flag);
label_15:
          bool isTrue = ch == 't';
          configuredTaskAwaitable = reader.MatchValueWithTrailingSeparatorAsync(isTrue ? JsonConvert.True : JsonConvert.False, cancellationToken).ConfigureAwait(false);
          if (!await configuredTaskAwaitable)
            throw reader.CreateUnexpectedCharacterException(reader._chars[reader._charPos]);
          reader.SetToken(JsonToken.Boolean, (object) isTrue);
          return new bool?(isTrue);
label_21:
          ++reader._charPos;
          if (reader._currentState != JsonReader.State.Array && reader._currentState != JsonReader.State.ArrayStart && reader._currentState != JsonReader.State.PostValue)
            throw reader.CreateUnexpectedCharacterException(ch);
          reader.SetToken(JsonToken.EndArray);
          return new bool?();
label_28:
          throw reader.CreateUnexpectedCharacterException(ch);
        case JsonReader.State.PostValue:
          if (await reader.ParsePostValueAsync(true, cancellationToken).ConfigureAwait(false))
            return new bool?();
          goto case JsonReader.State.Start;
        case JsonReader.State.Finished:
          await reader.ReadFinishedAsync(cancellationToken).ConfigureAwait(false);
          return new bool?();
        default:
          throw JsonReaderException.Create((JsonReader) reader, "Unexpected state: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) reader.CurrentState));
      }
    }

    public override Task<byte[]?> ReadAsBytesAsync(CancellationToken cancellationToken = default (CancellationToken)) => !this._safeAsync ? base.ReadAsBytesAsync(cancellationToken) : this.DoReadAsBytesAsync(cancellationToken);

    internal async Task<byte[]?> DoReadAsBytesAsync(CancellationToken cancellationToken)
    {
      JsonTextReader reader = this;
      reader.EnsureBuffer();
      bool isWrapped = false;
      byte[] numArray;
      switch (reader._currentState)
      {
        case JsonReader.State.Start:
        case JsonReader.State.Property:
        case JsonReader.State.ArrayStart:
        case JsonReader.State.Array:
        case JsonReader.State.ConstructorStart:
        case JsonReader.State.Constructor:
          byte[] data;
          char ch;
          while (true)
          {
            ch = reader._chars[reader._charPos];
            switch (ch)
            {
              case char.MinValue:
                if (!await reader.ReadNullCharAsync(cancellationToken).ConfigureAwait(false))
                  break;
                goto label_5;
              case '\t':
              case ' ':
                ++reader._charPos;
                break;
              case '\n':
                reader.ProcessLineFeed();
                break;
              case '\r':
                await reader.ProcessCarriageReturnAsync(false, cancellationToken).ConfigureAwait(false);
                break;
              case '"':
              case '\'':
                goto label_6;
              case ',':
                reader.ProcessValueComma();
                break;
              case '/':
                await reader.ParseCommentAsync(false, cancellationToken).ConfigureAwait(false);
                break;
              case '[':
                goto label_15;
              case ']':
                goto label_20;
              case 'n':
                goto label_17;
              case '{':
                ++reader._charPos;
                reader.SetToken(JsonToken.StartObject);
                await reader.ReadIntoWrappedTypeObjectAsync(cancellationToken).ConfigureAwait(false);
                isWrapped = true;
                break;
              default:
                ++reader._charPos;
                if (char.IsWhiteSpace(ch))
                  break;
                goto label_27;
            }
            data = (byte[]) null;
          }
label_5:
          reader.SetToken(JsonToken.None, (object) null, false);
          return (byte[]) null;
label_6:
          ConfiguredTaskAwaitable configuredTaskAwaitable = reader.ParseStringAsync(ch, ReadType.ReadAsBytes, cancellationToken).ConfigureAwait(false);
          await configuredTaskAwaitable;
          data = (byte[]) reader.Value;
          if (isWrapped)
          {
            configuredTaskAwaitable = reader.ReaderReadAndAssertAsync(cancellationToken).ConfigureAwait(false);
            await configuredTaskAwaitable;
            if (reader.TokenType != JsonToken.EndObject)
              throw JsonReaderException.Create((JsonReader) reader, "Error reading bytes. Unexpected token: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) reader.TokenType));
            reader.SetToken(JsonToken.Bytes, (object) data, false);
          }
          return data;
label_15:
          ++reader._charPos;
          reader.SetToken(JsonToken.StartArray);
          return await reader.ReadArrayIntoByteArrayAsync(cancellationToken).ConfigureAwait(false);
label_17:
          await reader.HandleNullAsync(cancellationToken).ConfigureAwait(false);
          return numArray;
label_20:
          ++reader._charPos;
          if (reader._currentState != JsonReader.State.Array && reader._currentState != JsonReader.State.ArrayStart && reader._currentState != JsonReader.State.PostValue)
            throw reader.CreateUnexpectedCharacterException(ch);
          reader.SetToken(JsonToken.EndArray);
          return (byte[]) null;
label_27:
          throw reader.CreateUnexpectedCharacterException(ch);
        case JsonReader.State.PostValue:
          if (await reader.ParsePostValueAsync(true, cancellationToken).ConfigureAwait(false))
            return (byte[]) null;
          goto case JsonReader.State.Start;
        case JsonReader.State.Finished:
          await reader.ReadFinishedAsync(cancellationToken).ConfigureAwait(false);
          return numArray;
        default:
          throw JsonReaderException.Create((JsonReader) reader, "Unexpected state: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) reader.CurrentState));
      }
    }

    private async Task ReadIntoWrappedTypeObjectAsync(CancellationToken cancellationToken)
    {
      JsonTextReader reader = this;
      ConfiguredTaskAwaitable configuredTaskAwaitable = reader.ReaderReadAndAssertAsync(cancellationToken).ConfigureAwait(false);
      await configuredTaskAwaitable;
      configuredTaskAwaitable = reader.Value != null && reader.Value.ToString() == "$type" ? reader.ReaderReadAndAssertAsync(cancellationToken).ConfigureAwait(false) : throw JsonReaderException.Create((JsonReader) reader, "Error reading bytes. Unexpected token: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JsonToken.StartObject));
      await configuredTaskAwaitable;
      if (reader.Value != null && reader.Value.ToString().StartsWith("System.Byte[]", StringComparison.Ordinal))
      {
        configuredTaskAwaitable = reader.ReaderReadAndAssertAsync(cancellationToken).ConfigureAwait(false);
        await configuredTaskAwaitable;
        if (reader.Value.ToString() == "$value")
          ;
      }
    }

    public override Task<DateTime?> ReadAsDateTimeAsync(CancellationToken cancellationToken = default (CancellationToken)) => !this._safeAsync ? base.ReadAsDateTimeAsync(cancellationToken) : this.DoReadAsDateTimeAsync(cancellationToken);

    internal async Task<DateTime?> DoReadAsDateTimeAsync(CancellationToken cancellationToken) => (DateTime?) await this.ReadStringValueAsync(ReadType.ReadAsDateTime, cancellationToken).ConfigureAwait(false);

    public override Task<DateTimeOffset?> ReadAsDateTimeOffsetAsync(
      CancellationToken cancellationToken = default (CancellationToken))
    {
      return !this._safeAsync ? base.ReadAsDateTimeOffsetAsync(cancellationToken) : this.DoReadAsDateTimeOffsetAsync(cancellationToken);
    }

    internal async Task<DateTimeOffset?> DoReadAsDateTimeOffsetAsync(
      CancellationToken cancellationToken)
    {
      return (DateTimeOffset?) await this.ReadStringValueAsync(ReadType.ReadAsDateTimeOffset, cancellationToken).ConfigureAwait(false);
    }

    public override Task<Decimal?> ReadAsDecimalAsync(CancellationToken cancellationToken = default (CancellationToken)) => !this._safeAsync ? base.ReadAsDecimalAsync(cancellationToken) : this.DoReadAsDecimalAsync(cancellationToken);

    internal async Task<Decimal?> DoReadAsDecimalAsync(CancellationToken cancellationToken) => (Decimal?) await this.ReadNumberValueAsync(ReadType.ReadAsDecimal, cancellationToken).ConfigureAwait(false);

    public override Task<double?> ReadAsDoubleAsync(CancellationToken cancellationToken = default (CancellationToken)) => !this._safeAsync ? base.ReadAsDoubleAsync(cancellationToken) : this.DoReadAsDoubleAsync(cancellationToken);

    internal async Task<double?> DoReadAsDoubleAsync(CancellationToken cancellationToken) => (double?) await this.ReadNumberValueAsync(ReadType.ReadAsDouble, cancellationToken).ConfigureAwait(false);

    public override Task<int?> ReadAsInt32Async(CancellationToken cancellationToken = default (CancellationToken)) => !this._safeAsync ? base.ReadAsInt32Async(cancellationToken) : this.DoReadAsInt32Async(cancellationToken);

    internal async Task<int?> DoReadAsInt32Async(CancellationToken cancellationToken) => (int?) await this.ReadNumberValueAsync(ReadType.ReadAsInt32, cancellationToken).ConfigureAwait(false);

    public override Task<string?> ReadAsStringAsync(CancellationToken cancellationToken = default (CancellationToken)) => !this._safeAsync ? base.ReadAsStringAsync(cancellationToken) : this.DoReadAsStringAsync(cancellationToken);

    internal async Task<string?> DoReadAsStringAsync(CancellationToken cancellationToken) => (string) await this.ReadStringValueAsync(ReadType.ReadAsString, cancellationToken).ConfigureAwait(false);

    public JsonTextReader(TextReader reader)
    {
      this._reader = reader != null ? reader : throw new ArgumentNullException(nameof (reader));
      this._lineNumber = 1;
      this._safeAsync = this.GetType() == typeof (JsonTextReader);
    }

    public JsonNameTable? PropertyNameTable { get; set; }

    public IArrayPool<char>? ArrayPool
    {
      get => this._arrayPool;
      set => this._arrayPool = value != null ? value : throw new ArgumentNullException(nameof (value));
    }

    private void EnsureBufferNotEmpty()
    {
      if (!this._stringBuffer.IsEmpty)
        return;
      this._stringBuffer = new StringBuffer(this._arrayPool, 1024);
    }

    private void SetNewLine(bool hasNextChar)
    {
      if (hasNextChar && this._chars[this._charPos] == '\n')
        ++this._charPos;
      this.OnNewLine(this._charPos);
    }

    private void OnNewLine(int pos)
    {
      ++this._lineNumber;
      this._lineStartPos = pos;
    }

    private void ParseString(char quote, ReadType readType)
    {
      ++this._charPos;
      this.ShiftBufferIfNeeded();
      this.ReadStringIntoBuffer(quote);
      this.ParseReadString(quote, readType);
    }

    private void ParseReadString(char quote, ReadType readType)
    {
      this.SetPostValueState(true);
      switch (readType)
      {
        case ReadType.ReadAsInt32:
          break;
        case ReadType.ReadAsBytes:
          Guid g;
          this.SetToken(JsonToken.Bytes, this._stringReference.Length != 0 ? (this._stringReference.Length != 36 || !ConvertUtils.TryConvertGuid(this._stringReference.ToString(), out g) ? (object) Convert.FromBase64CharArray(this._stringReference.Chars, this._stringReference.StartIndex, this._stringReference.Length) : (object) g.ToByteArray()) : (object) CollectionUtils.ArrayEmpty<byte>(), false);
          break;
        case ReadType.ReadAsString:
          this.SetToken(JsonToken.String, (object) this._stringReference.ToString(), false);
          this._quoteChar = quote;
          break;
        case ReadType.ReadAsDecimal:
          break;
        case ReadType.ReadAsBoolean:
          break;
        default:
          if (this._dateParseHandling != DateParseHandling.None)
          {
            DateParseHandling dateParseHandling;
            switch (readType)
            {
              case ReadType.ReadAsDateTime:
                dateParseHandling = DateParseHandling.DateTime;
                break;
              case ReadType.ReadAsDateTimeOffset:
                dateParseHandling = DateParseHandling.DateTimeOffset;
                break;
              default:
                dateParseHandling = this._dateParseHandling;
                break;
            }
            if (dateParseHandling == DateParseHandling.DateTime)
            {
              DateTime dt;
              if (DateTimeUtils.TryParseDateTime(this._stringReference, this.DateTimeZoneHandling, this.DateFormatString, this.Culture, out dt))
              {
                this.SetToken(JsonToken.Date, (object) dt, false);
                break;
              }
            }
            else
            {
              DateTimeOffset dt;
              if (DateTimeUtils.TryParseDateTimeOffset(this._stringReference, this.DateFormatString, this.Culture, out dt))
              {
                this.SetToken(JsonToken.Date, (object) dt, false);
                break;
              }
            }
          }
          this.SetToken(JsonToken.String, (object) this._stringReference.ToString(), false);
          this._quoteChar = quote;
          break;
      }
    }

    private static void BlockCopyChars(
      char[] src,
      int srcOffset,
      char[] dst,
      int dstOffset,
      int count)
    {
      Buffer.BlockCopy((Array) src, srcOffset * 2, (Array) dst, dstOffset * 2, count * 2);
    }

    private void ShiftBufferIfNeeded()
    {
      int length = this._chars.Length;
      if ((double) (length - this._charPos) > (double) length * 0.1 && length < 1073741823)
        return;
      int count = this._charsUsed - this._charPos;
      if (count > 0)
        JsonTextReader.BlockCopyChars(this._chars, this._charPos, this._chars, 0, count);
      this._lineStartPos -= this._charPos;
      this._charPos = 0;
      this._charsUsed = count;
      this._chars[this._charsUsed] = char.MinValue;
    }

    private int ReadData(bool append) => this.ReadData(append, 0);

    private void PrepareBufferForReadData(bool append, int charsRequired)
    {
      if (this._charsUsed + charsRequired < this._chars.Length - 1)
        return;
      if (append)
      {
        int num = this._chars.Length * 2;
        char[] dst = BufferUtils.RentBuffer(this._arrayPool, Math.Max(num < 0 ? int.MaxValue : num, this._charsUsed + charsRequired + 1));
        JsonTextReader.BlockCopyChars(this._chars, 0, dst, 0, this._chars.Length);
        BufferUtils.ReturnBuffer(this._arrayPool, this._chars);
        this._chars = dst;
      }
      else
      {
        int count = this._charsUsed - this._charPos;
        if (count + charsRequired + 1 >= this._chars.Length)
        {
          char[] dst = BufferUtils.RentBuffer(this._arrayPool, count + charsRequired + 1);
          if (count > 0)
            JsonTextReader.BlockCopyChars(this._chars, this._charPos, dst, 0, count);
          BufferUtils.ReturnBuffer(this._arrayPool, this._chars);
          this._chars = dst;
        }
        else if (count > 0)
          JsonTextReader.BlockCopyChars(this._chars, this._charPos, this._chars, 0, count);
        this._lineStartPos -= this._charPos;
        this._charPos = 0;
        this._charsUsed = count;
      }
    }

    private int ReadData(bool append, int charsRequired)
    {
      if (this._isEndOfFile)
        return 0;
      this.PrepareBufferForReadData(append, charsRequired);
      int num = this._reader.Read(this._chars, this._charsUsed, this._chars.Length - this._charsUsed - 1);
      this._charsUsed += num;
      if (num == 0)
        this._isEndOfFile = true;
      this._chars[this._charsUsed] = char.MinValue;
      return num;
    }

    private bool EnsureChars(int relativePosition, bool append) => this._charPos + relativePosition < this._charsUsed || this.ReadChars(relativePosition, append);

    private bool ReadChars(int relativePosition, bool append)
    {
      if (this._isEndOfFile)
        return false;
      int num1 = this._charPos + relativePosition - this._charsUsed + 1;
      int num2 = 0;
      do
      {
        int num3 = this.ReadData(append, num1 - num2);
        if (num3 != 0)
          num2 += num3;
        else
          break;
      }
      while (num2 < num1);
      return num2 >= num1;
    }

    public override bool Read()
    {
      this.EnsureBuffer();
      do
      {
        switch (this._currentState)
        {
          case JsonReader.State.Start:
          case JsonReader.State.Property:
          case JsonReader.State.ArrayStart:
          case JsonReader.State.Array:
          case JsonReader.State.ConstructorStart:
          case JsonReader.State.Constructor:
            return this.ParseValue();
          case JsonReader.State.ObjectStart:
          case JsonReader.State.Object:
            return this.ParseObject();
          case JsonReader.State.PostValue:
            continue;
          case JsonReader.State.Finished:
            goto label_6;
          default:
            goto label_13;
        }
      }
      while (!this.ParsePostValue(false));
      return true;
label_6:
      if (this.EnsureChars(0, false))
      {
        this.EatWhitespace();
        if (this._isEndOfFile)
        {
          this.SetToken(JsonToken.None);
          return false;
        }
        if (this._chars[this._charPos] != '/')
          throw JsonReaderException.Create((JsonReader) this, "Additional text encountered after finished reading JSON content: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this._chars[this._charPos]));
        this.ParseComment(true);
        return true;
      }
      this.SetToken(JsonToken.None);
      return false;
label_13:
      throw JsonReaderException.Create((JsonReader) this, "Unexpected state: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this.CurrentState));
    }

    public override int? ReadAsInt32() => (int?) this.ReadNumberValue(ReadType.ReadAsInt32);

    public override DateTime? ReadAsDateTime() => (DateTime?) this.ReadStringValue(ReadType.ReadAsDateTime);

    public override string? ReadAsString() => (string) this.ReadStringValue(ReadType.ReadAsString);

    public override byte[]? ReadAsBytes()
    {
      this.EnsureBuffer();
      bool flag = false;
      switch (this._currentState)
      {
        case JsonReader.State.Start:
        case JsonReader.State.Property:
        case JsonReader.State.ArrayStart:
        case JsonReader.State.Array:
        case JsonReader.State.ConstructorStart:
        case JsonReader.State.Constructor:
          char ch;
          do
          {
            do
            {
              ch = this._chars[this._charPos];
              switch (ch)
              {
                case char.MinValue:
                  continue;
                case '\t':
                case ' ':
                  goto label_21;
                case '\n':
                  goto label_20;
                case '\r':
                  goto label_19;
                case '"':
                case '\'':
                  goto label_6;
                case ',':
                  goto label_15;
                case '/':
                  goto label_14;
                case '[':
                  goto label_12;
                case ']':
                  goto label_16;
                case 'n':
                  goto label_13;
                case '{':
                  goto label_11;
                default:
                  goto label_22;
              }
            }
            while (!this.ReadNullChar());
            this.SetToken(JsonToken.None, (object) null, false);
            return (byte[]) null;
label_6:
            this.ParseString(ch, ReadType.ReadAsBytes);
            byte[] numArray = (byte[]) this.Value;
            if (flag)
            {
              this.ReaderReadAndAssert();
              if (this.TokenType != JsonToken.EndObject)
                throw JsonReaderException.Create((JsonReader) this, "Error reading bytes. Unexpected token: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this.TokenType));
              this.SetToken(JsonToken.Bytes, (object) numArray, false);
            }
            return numArray;
label_11:
            ++this._charPos;
            this.SetToken(JsonToken.StartObject);
            this.ReadIntoWrappedTypeObject();
            flag = true;
            continue;
label_12:
            ++this._charPos;
            this.SetToken(JsonToken.StartArray);
            return this.ReadArrayIntoByteArray();
label_13:
            this.HandleNull();
            return (byte[]) null;
label_14:
            this.ParseComment(false);
            continue;
label_15:
            this.ProcessValueComma();
            continue;
label_16:
            ++this._charPos;
            if (this._currentState != JsonReader.State.Array && this._currentState != JsonReader.State.ArrayStart && this._currentState != JsonReader.State.PostValue)
              throw this.CreateUnexpectedCharacterException(ch);
            this.SetToken(JsonToken.EndArray);
            return (byte[]) null;
label_19:
            this.ProcessCarriageReturn(false);
            continue;
label_20:
            this.ProcessLineFeed();
            continue;
label_21:
            ++this._charPos;
            continue;
label_22:
            ++this._charPos;
          }
          while (char.IsWhiteSpace(ch));
          throw this.CreateUnexpectedCharacterException(ch);
        case JsonReader.State.PostValue:
          if (this.ParsePostValue(true))
            return (byte[]) null;
          goto case JsonReader.State.Start;
        case JsonReader.State.Finished:
          this.ReadFinished();
          return (byte[]) null;
        default:
          throw JsonReaderException.Create((JsonReader) this, "Unexpected state: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this.CurrentState));
      }
    }

    private object? ReadStringValue(ReadType readType)
    {
      this.EnsureBuffer();
      switch (this._currentState)
      {
        case JsonReader.State.Start:
        case JsonReader.State.Property:
        case JsonReader.State.ArrayStart:
        case JsonReader.State.Array:
        case JsonReader.State.ConstructorStart:
        case JsonReader.State.Constructor:
          char ch;
          do
          {
            do
            {
              ch = this._chars[this._charPos];
              switch (ch)
              {
                case char.MinValue:
                  continue;
                case '\t':
                case ' ':
                  goto label_28;
                case '\n':
                  goto label_27;
                case '\r':
                  goto label_26;
                case '"':
                case '\'':
                  goto label_6;
                case ',':
                  goto label_22;
                case '-':
                  goto label_7;
                case '.':
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                  goto label_10;
                case '/':
                  goto label_21;
                case 'I':
                  goto label_18;
                case 'N':
                  goto label_19;
                case ']':
                  goto label_23;
                case 'f':
                case 't':
                  goto label_13;
                case 'n':
                  goto label_20;
                default:
                  goto label_29;
              }
            }
            while (!this.ReadNullChar());
            this.SetToken(JsonToken.None, (object) null, false);
            return (object) null;
label_6:
            this.ParseString(ch, readType);
            return this.FinishReadQuotedStringValue(readType);
label_7:
            if (this.EnsureChars(1, true) && this._chars[this._charPos + 1] == 'I')
              return this.ParseNumberNegativeInfinity(readType);
            this.ParseNumber(readType);
            return this.Value;
label_10:
            if (readType != ReadType.ReadAsString)
            {
              ++this._charPos;
              throw this.CreateUnexpectedCharacterException(ch);
            }
            this.ParseNumber(ReadType.ReadAsString);
            return this.Value;
label_13:
            if (readType != ReadType.ReadAsString)
            {
              ++this._charPos;
              throw this.CreateUnexpectedCharacterException(ch);
            }
            string str = ch == 't' ? JsonConvert.True : JsonConvert.False;
            if (!this.MatchValueWithTrailingSeparator(str))
              throw this.CreateUnexpectedCharacterException(this._chars[this._charPos]);
            this.SetToken(JsonToken.String, (object) str);
            return (object) str;
label_18:
            return this.ParseNumberPositiveInfinity(readType);
label_19:
            return this.ParseNumberNaN(readType);
label_20:
            this.HandleNull();
            return (object) null;
label_21:
            this.ParseComment(false);
            continue;
label_22:
            this.ProcessValueComma();
            continue;
label_23:
            ++this._charPos;
            if (this._currentState != JsonReader.State.Array && this._currentState != JsonReader.State.ArrayStart && this._currentState != JsonReader.State.PostValue)
              throw this.CreateUnexpectedCharacterException(ch);
            this.SetToken(JsonToken.EndArray);
            return (object) null;
label_26:
            this.ProcessCarriageReturn(false);
            continue;
label_27:
            this.ProcessLineFeed();
            continue;
label_28:
            ++this._charPos;
            continue;
label_29:
            ++this._charPos;
          }
          while (char.IsWhiteSpace(ch));
          throw this.CreateUnexpectedCharacterException(ch);
        case JsonReader.State.PostValue:
          if (this.ParsePostValue(true))
            return (object) null;
          goto case JsonReader.State.Start;
        case JsonReader.State.Finished:
          this.ReadFinished();
          return (object) null;
        default:
          throw JsonReaderException.Create((JsonReader) this, "Unexpected state: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this.CurrentState));
      }
    }

    private object? FinishReadQuotedStringValue(ReadType readType)
    {
      switch (readType)
      {
        case ReadType.ReadAsBytes:
        case ReadType.ReadAsString:
          return this.Value;
        case ReadType.ReadAsDateTime:
          return this.Value is DateTime dateTime ? (object) dateTime : (object) this.ReadDateTimeString((string) this.Value);
        case ReadType.ReadAsDateTimeOffset:
          return this.Value is DateTimeOffset dateTimeOffset ? (object) dateTimeOffset : (object) this.ReadDateTimeOffsetString((string) this.Value);
        default:
          throw new ArgumentOutOfRangeException(nameof (readType));
      }
    }

    private JsonReaderException CreateUnexpectedCharacterException(char c) => JsonReaderException.Create((JsonReader) this, "Unexpected character encountered while parsing value: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) c));

    public override bool? ReadAsBoolean()
    {
      this.EnsureBuffer();
      switch (this._currentState)
      {
        case JsonReader.State.Start:
        case JsonReader.State.Property:
        case JsonReader.State.ArrayStart:
        case JsonReader.State.Array:
        case JsonReader.State.ConstructorStart:
        case JsonReader.State.Constructor:
          char ch;
          do
          {
            do
            {
              ch = this._chars[this._charPos];
              switch (ch)
              {
                case char.MinValue:
                  continue;
                case '\t':
                case ' ':
                  goto label_19;
                case '\n':
                  goto label_18;
                case '\r':
                  goto label_17;
                case '"':
                case '\'':
                  goto label_6;
                case ',':
                  goto label_13;
                case '-':
                case '.':
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                  goto label_8;
                case '/':
                  goto label_12;
                case ']':
                  goto label_14;
                case 'f':
                case 't':
                  goto label_9;
                case 'n':
                  goto label_7;
                default:
                  goto label_20;
              }
            }
            while (!this.ReadNullChar());
            this.SetToken(JsonToken.None, (object) null, false);
            return new bool?();
label_6:
            this.ParseString(ch, ReadType.Read);
            return this.ReadBooleanString(this._stringReference.ToString());
label_7:
            this.HandleNull();
            return new bool?();
label_8:
            this.ParseNumber(ReadType.Read);
            bool flag1 = !(this.Value is BigInteger bigInteger) ? Convert.ToBoolean(this.Value, (IFormatProvider) CultureInfo.InvariantCulture) : bigInteger != 0L;
            this.SetToken(JsonToken.Boolean, (object) flag1, false);
            return new bool?(flag1);
label_9:
            bool flag2 = ch == 't';
            if (!this.MatchValueWithTrailingSeparator(flag2 ? JsonConvert.True : JsonConvert.False))
              throw this.CreateUnexpectedCharacterException(this._chars[this._charPos]);
            this.SetToken(JsonToken.Boolean, (object) flag2);
            return new bool?(flag2);
label_12:
            this.ParseComment(false);
            continue;
label_13:
            this.ProcessValueComma();
            continue;
label_14:
            ++this._charPos;
            if (this._currentState != JsonReader.State.Array && this._currentState != JsonReader.State.ArrayStart && this._currentState != JsonReader.State.PostValue)
              throw this.CreateUnexpectedCharacterException(ch);
            this.SetToken(JsonToken.EndArray);
            return new bool?();
label_17:
            this.ProcessCarriageReturn(false);
            continue;
label_18:
            this.ProcessLineFeed();
            continue;
label_19:
            ++this._charPos;
            continue;
label_20:
            ++this._charPos;
          }
          while (char.IsWhiteSpace(ch));
          throw this.CreateUnexpectedCharacterException(ch);
        case JsonReader.State.PostValue:
          if (this.ParsePostValue(true))
            return new bool?();
          goto case JsonReader.State.Start;
        case JsonReader.State.Finished:
          this.ReadFinished();
          return new bool?();
        default:
          throw JsonReaderException.Create((JsonReader) this, "Unexpected state: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this.CurrentState));
      }
    }

    private void ProcessValueComma()
    {
      ++this._charPos;
      if (this._currentState != JsonReader.State.PostValue)
      {
        this.SetToken(JsonToken.Undefined);
        JsonReaderException characterException = this.CreateUnexpectedCharacterException(',');
        --this._charPos;
        throw characterException;
      }
      this.SetStateBasedOnCurrent();
    }

    private object? ReadNumberValue(ReadType readType)
    {
      this.EnsureBuffer();
      switch (this._currentState)
      {
        case JsonReader.State.Start:
        case JsonReader.State.Property:
        case JsonReader.State.ArrayStart:
        case JsonReader.State.Array:
        case JsonReader.State.ConstructorStart:
        case JsonReader.State.Constructor:
          char ch;
          do
          {
            do
            {
              ch = this._chars[this._charPos];
              switch (ch)
              {
                case char.MinValue:
                  continue;
                case '\t':
                case ' ':
                  goto label_21;
                case '\n':
                  goto label_20;
                case '\r':
                  goto label_19;
                case '"':
                case '\'':
                  goto label_6;
                case ',':
                  goto label_15;
                case '-':
                  goto label_10;
                case '.':
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                  goto label_13;
                case '/':
                  goto label_14;
                case 'I':
                  goto label_9;
                case 'N':
                  goto label_8;
                case ']':
                  goto label_16;
                case 'n':
                  goto label_7;
                default:
                  goto label_22;
              }
            }
            while (!this.ReadNullChar());
            this.SetToken(JsonToken.None, (object) null, false);
            return (object) null;
label_6:
            this.ParseString(ch, readType);
            return this.FinishReadQuotedNumber(readType);
label_7:
            this.HandleNull();
            return (object) null;
label_8:
            return this.ParseNumberNaN(readType);
label_9:
            return this.ParseNumberPositiveInfinity(readType);
label_10:
            if (this.EnsureChars(1, true) && this._chars[this._charPos + 1] == 'I')
              return this.ParseNumberNegativeInfinity(readType);
            this.ParseNumber(readType);
            return this.Value;
label_13:
            this.ParseNumber(readType);
            return this.Value;
label_14:
            this.ParseComment(false);
            continue;
label_15:
            this.ProcessValueComma();
            continue;
label_16:
            ++this._charPos;
            if (this._currentState != JsonReader.State.Array && this._currentState != JsonReader.State.ArrayStart && this._currentState != JsonReader.State.PostValue)
              throw this.CreateUnexpectedCharacterException(ch);
            this.SetToken(JsonToken.EndArray);
            return (object) null;
label_19:
            this.ProcessCarriageReturn(false);
            continue;
label_20:
            this.ProcessLineFeed();
            continue;
label_21:
            ++this._charPos;
            continue;
label_22:
            ++this._charPos;
          }
          while (char.IsWhiteSpace(ch));
          throw this.CreateUnexpectedCharacterException(ch);
        case JsonReader.State.PostValue:
          if (this.ParsePostValue(true))
            return (object) null;
          goto case JsonReader.State.Start;
        case JsonReader.State.Finished:
          this.ReadFinished();
          return (object) null;
        default:
          throw JsonReaderException.Create((JsonReader) this, "Unexpected state: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this.CurrentState));
      }
    }

    private object? FinishReadQuotedNumber(ReadType readType)
    {
      if (readType == ReadType.ReadAsInt32)
        return (object) this.ReadInt32String(this._stringReference.ToString());
      if (readType == ReadType.ReadAsDecimal)
        return (object) this.ReadDecimalString(this._stringReference.ToString());
      if (readType == ReadType.ReadAsDouble)
        return (object) this.ReadDoubleString(this._stringReference.ToString());
      throw new ArgumentOutOfRangeException(nameof (readType));
    }

    public override DateTimeOffset? ReadAsDateTimeOffset() => (DateTimeOffset?) this.ReadStringValue(ReadType.ReadAsDateTimeOffset);

    public override Decimal? ReadAsDecimal() => (Decimal?) this.ReadNumberValue(ReadType.ReadAsDecimal);

    public override double? ReadAsDouble() => (double?) this.ReadNumberValue(ReadType.ReadAsDouble);

    private void HandleNull()
    {
      if (this.EnsureChars(1, true))
      {
        if (this._chars[this._charPos + 1] == 'u')
        {
          this.ParseNull();
        }
        else
        {
          this._charPos += 2;
          throw this.CreateUnexpectedCharacterException(this._chars[this._charPos - 1]);
        }
      }
      else
      {
        this._charPos = this._charsUsed;
        throw this.CreateUnexpectedEndException();
      }
    }

    private void ReadFinished()
    {
      if (this.EnsureChars(0, false))
      {
        this.EatWhitespace();
        if (this._isEndOfFile)
          return;
        if (this._chars[this._charPos] != '/')
          throw JsonReaderException.Create((JsonReader) this, "Additional text encountered after finished reading JSON content: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this._chars[this._charPos]));
        this.ParseComment(false);
      }
      this.SetToken(JsonToken.None);
    }

    private bool ReadNullChar()
    {
      if (this._charsUsed == this._charPos)
      {
        if (this.ReadData(false) == 0)
        {
          this._isEndOfFile = true;
          return true;
        }
      }
      else
        ++this._charPos;
      return false;
    }

    private void EnsureBuffer()
    {
      if (this._chars != null)
        return;
      this._chars = BufferUtils.RentBuffer(this._arrayPool, 1024);
      this._chars[0] = char.MinValue;
    }

    private void ReadStringIntoBuffer(char quote)
    {
      int charPos1 = this._charPos;
      int charPos2 = this._charPos;
      int lastWritePosition = this._charPos;
      this._stringBuffer.Position = 0;
      do
      {
        char ch1 = this._chars[charPos1++];
        if (ch1 <= '\r')
        {
          if (ch1 != char.MinValue)
          {
            if (ch1 != '\n')
            {
              if (ch1 == '\r')
              {
                this._charPos = charPos1 - 1;
                this.ProcessCarriageReturn(true);
                charPos1 = this._charPos;
              }
            }
            else
            {
              this._charPos = charPos1 - 1;
              this.ProcessLineFeed();
              charPos1 = this._charPos;
            }
          }
          else if (this._charsUsed == charPos1 - 1)
          {
            --charPos1;
            if (this.ReadData(true) == 0)
            {
              this._charPos = charPos1;
              throw JsonReaderException.Create((JsonReader) this, "Unterminated string. Expected delimiter: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) quote));
            }
          }
        }
        else if (ch1 != '"' && ch1 != '\'')
        {
          if (ch1 == '\\')
          {
            this._charPos = charPos1;
            if (!this.EnsureChars(0, true))
              throw JsonReaderException.Create((JsonReader) this, "Unterminated string. Expected delimiter: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) quote));
            int writeToPosition = charPos1 - 1;
            char ch2 = this._chars[charPos1];
            ++charPos1;
            char ch3;
            switch (ch2)
            {
              case '"':
              case '\'':
              case '/':
                ch3 = ch2;
                break;
              case '\\':
                ch3 = '\\';
                break;
              case 'b':
                ch3 = '\b';
                break;
              case 'f':
                ch3 = '\f';
                break;
              case 'n':
                ch3 = '\n';
                break;
              case 'r':
                ch3 = '\r';
                break;
              case 't':
                ch3 = '\t';
                break;
              case 'u':
                this._charPos = charPos1;
                ch3 = this.ParseUnicode();
                if (StringUtils.IsLowSurrogate(ch3))
                  ch3 = '�';
                else if (StringUtils.IsHighSurrogate(ch3))
                {
                  bool flag;
                  do
                  {
                    flag = false;
                    if (this.EnsureChars(2, true) && this._chars[this._charPos] == '\\' && this._chars[this._charPos + 1] == 'u')
                    {
                      char writeChar = ch3;
                      this._charPos += 2;
                      ch3 = this.ParseUnicode();
                      if (!StringUtils.IsLowSurrogate(ch3))
                      {
                        if (StringUtils.IsHighSurrogate(ch3))
                        {
                          writeChar = '�';
                          flag = true;
                        }
                        else
                          writeChar = '�';
                      }
                      this.EnsureBufferNotEmpty();
                      this.WriteCharToBuffer(writeChar, lastWritePosition, writeToPosition);
                      lastWritePosition = this._charPos;
                    }
                    else
                      ch3 = '�';
                  }
                  while (flag);
                }
                charPos1 = this._charPos;
                break;
              default:
                this._charPos = charPos1;
                throw JsonReaderException.Create((JsonReader) this, "Bad JSON escape sequence: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) ("\\" + ch2.ToString())));
            }
            this.EnsureBufferNotEmpty();
            this.WriteCharToBuffer(ch3, lastWritePosition, writeToPosition);
            lastWritePosition = charPos1;
          }
        }
      }
      while ((int) this._chars[charPos1 - 1] != (int) quote);
      this.FinishReadStringIntoBuffer(charPos1 - 1, charPos2, lastWritePosition);
    }

    private void FinishReadStringIntoBuffer(
      int charPos,
      int initialPosition,
      int lastWritePosition)
    {
      if (initialPosition == lastWritePosition)
      {
        this._stringReference = new StringReference(this._chars, initialPosition, charPos - initialPosition);
      }
      else
      {
        this.EnsureBufferNotEmpty();
        if (charPos > lastWritePosition)
          this._stringBuffer.Append(this._arrayPool, this._chars, lastWritePosition, charPos - lastWritePosition);
        this._stringReference = new StringReference(this._stringBuffer.InternalBuffer, 0, this._stringBuffer.Position);
      }
      this._charPos = charPos + 1;
    }

    private void WriteCharToBuffer(char writeChar, int lastWritePosition, int writeToPosition)
    {
      if (writeToPosition > lastWritePosition)
        this._stringBuffer.Append(this._arrayPool, this._chars, lastWritePosition, writeToPosition - lastWritePosition);
      this._stringBuffer.Append(this._arrayPool, writeChar);
    }

    private char ConvertUnicode(bool enoughChars)
    {
      if (!enoughChars)
        throw JsonReaderException.Create((JsonReader) this, "Unexpected end while parsing Unicode escape sequence.");
      int num1;
      if (!ConvertUtils.TryHexTextToInt(this._chars, this._charPos, this._charPos + 4, out num1))
        throw JsonReaderException.Create((JsonReader) this, "Invalid Unicode escape sequence: \\u{0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) new string(this._chars, this._charPos, 4)));
      int num2 = (int) Convert.ToChar(num1);
      this._charPos += 4;
      return (char) num2;
    }

    private char ParseUnicode() => this.ConvertUnicode(this.EnsureChars(4, true));

    private void ReadNumberIntoBuffer()
    {
      int charPos = this._charPos;
      while (true)
      {
        char currentChar;
        do
        {
          currentChar = this._chars[charPos];
          if (currentChar == char.MinValue)
            this._charPos = charPos;
          else
            goto label_5;
        }
        while (this._charsUsed == charPos && this.ReadData(true) != 0);
        break;
label_5:
        if (!this.ReadNumberCharIntoBuffer(currentChar, charPos))
          ++charPos;
        else
          goto label_4;
      }
      return;
label_4:;
    }

    private bool ReadNumberCharIntoBuffer(char currentChar, int charPos)
    {
      switch (currentChar)
      {
        case '+':
        case '-':
        case '.':
        case '0':
        case '1':
        case '2':
        case '3':
        case '4':
        case '5':
        case '6':
        case '7':
        case '8':
        case '9':
        case 'A':
        case 'B':
        case 'C':
        case 'D':
        case 'E':
        case 'F':
        case 'X':
        case 'a':
        case 'b':
        case 'c':
        case 'd':
        case 'e':
        case 'f':
        case 'x':
          return false;
        default:
          this._charPos = charPos;
          if (char.IsWhiteSpace(currentChar) || currentChar == ',' || currentChar == '}' || currentChar == ']' || currentChar == ')' || currentChar == '/')
            return true;
          throw JsonReaderException.Create((JsonReader) this, "Unexpected character encountered while parsing number: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) currentChar));
      }
    }

    private void ClearRecentString()
    {
      this._stringBuffer.Position = 0;
      this._stringReference = new StringReference();
    }

    private bool ParsePostValue(bool ignoreComments)
    {
      char c;
      while (true)
      {
        do
        {
          do
          {
            c = this._chars[this._charPos];
            switch (c)
            {
              case char.MinValue:
                if (this._charsUsed == this._charPos)
                  continue;
                goto label_4;
              case '\t':
              case ' ':
                goto label_11;
              case '\n':
                goto label_13;
              case '\r':
                goto label_12;
              case ')':
                goto label_7;
              case ',':
                goto label_10;
              case '/':
                goto label_8;
              case ']':
                goto label_6;
              case '}':
                goto label_5;
              default:
                goto label_14;
            }
          }
          while (this.ReadData(false) != 0);
          this._currentState = JsonReader.State.Finished;
          return false;
label_4:
          ++this._charPos;
          continue;
label_5:
          ++this._charPos;
          this.SetToken(JsonToken.EndObject);
          return true;
label_6:
          ++this._charPos;
          this.SetToken(JsonToken.EndArray);
          return true;
label_7:
          ++this._charPos;
          this.SetToken(JsonToken.EndConstructor);
          return true;
label_8:
          this.ParseComment(!ignoreComments);
        }
        while (ignoreComments);
        break;
label_11:
        ++this._charPos;
        continue;
label_12:
        this.ProcessCarriageReturn(false);
        continue;
label_13:
        this.ProcessLineFeed();
        continue;
label_14:
        if (char.IsWhiteSpace(c))
          ++this._charPos;
        else
          goto label_16;
      }
      return true;
label_10:
      ++this._charPos;
      this.SetStateBasedOnCurrent();
      return false;
label_16:
      if (!this.SupportMultipleContent || this.Depth != 0)
        throw JsonReaderException.Create((JsonReader) this, "After parsing a value an unexpected character was encountered: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) c));
      this.SetStateBasedOnCurrent();
      return false;
    }

    private bool ParseObject()
    {
      while (true)
      {
        char c;
        do
        {
          c = this._chars[this._charPos];
          switch (c)
          {
            case char.MinValue:
              if (this._charsUsed == this._charPos)
                continue;
              goto label_4;
            case '\t':
            case ' ':
              goto label_9;
            case '\n':
              goto label_8;
            case '\r':
              goto label_7;
            case '/':
              goto label_6;
            case '}':
              goto label_5;
            default:
              goto label_10;
          }
        }
        while (this.ReadData(false) != 0);
        break;
label_4:
        ++this._charPos;
        continue;
label_7:
        this.ProcessCarriageReturn(false);
        continue;
label_8:
        this.ProcessLineFeed();
        continue;
label_9:
        ++this._charPos;
        continue;
label_10:
        if (char.IsWhiteSpace(c))
          ++this._charPos;
        else
          goto label_12;
      }
      return false;
label_5:
      this.SetToken(JsonToken.EndObject);
      ++this._charPos;
      return true;
label_6:
      this.ParseComment(true);
      return true;
label_12:
      return this.ParseProperty();
    }

    private bool ParseProperty()
    {
      char ch = this._chars[this._charPos];
      char quote;
      switch (ch)
      {
        case '"':
        case '\'':
          ++this._charPos;
          quote = ch;
          this.ShiftBufferIfNeeded();
          this.ReadStringIntoBuffer(quote);
          break;
        default:
          if (!this.ValidIdentifierChar(ch))
            throw JsonReaderException.Create((JsonReader) this, "Invalid property identifier character: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this._chars[this._charPos]));
          quote = char.MinValue;
          this.ShiftBufferIfNeeded();
          this.ParseUnquotedProperty();
          break;
      }
      string str = this.PropertyNameTable == null ? this._stringReference.ToString() : this.PropertyNameTable.Get(this._stringReference.Chars, this._stringReference.StartIndex, this._stringReference.Length) ?? this._stringReference.ToString();
      this.EatWhitespace();
      if (this._chars[this._charPos] != ':')
        throw JsonReaderException.Create((JsonReader) this, "Invalid character after parsing property name. Expected ':' but got: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this._chars[this._charPos]));
      ++this._charPos;
      this.SetToken(JsonToken.PropertyName, (object) str);
      this._quoteChar = quote;
      this.ClearRecentString();
      return true;
    }

    private bool ValidIdentifierChar(char value) => char.IsLetterOrDigit(value) || value == '_' || value == '$';

    private void ParseUnquotedProperty()
    {
      int charPos = this._charPos;
      char currentChar;
      do
      {
        currentChar = this._chars[this._charPos];
        if (currentChar == char.MinValue)
        {
          if (this._charsUsed == this._charPos)
          {
            if (this.ReadData(true) == 0)
              throw JsonReaderException.Create((JsonReader) this, "Unexpected end while parsing unquoted property name.");
          }
          else
          {
            this._stringReference = new StringReference(this._chars, charPos, this._charPos - charPos);
            break;
          }
        }
      }
      while (!this.ReadUnquotedPropertyReportIfDone(currentChar, charPos));
    }

    private bool ReadUnquotedPropertyReportIfDone(char currentChar, int initialPosition)
    {
      if (this.ValidIdentifierChar(currentChar))
      {
        ++this._charPos;
        return false;
      }
      if (!char.IsWhiteSpace(currentChar) && currentChar != ':')
        throw JsonReaderException.Create((JsonReader) this, "Invalid JavaScript property identifier character: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) currentChar));
      this._stringReference = new StringReference(this._chars, initialPosition, this._charPos - initialPosition);
      return true;
    }

    private bool ParseValue()
    {
      char ch;
      while (true)
      {
        do
        {
          ch = this._chars[this._charPos];
          switch (ch)
          {
            case char.MinValue:
              if (this._charsUsed == this._charPos)
                continue;
              goto label_4;
            case '\t':
            case ' ':
              goto label_30;
            case '\n':
              goto label_29;
            case '\r':
              goto label_28;
            case '"':
            case '\'':
              goto label_5;
            case ')':
              goto label_27;
            case ',':
              goto label_26;
            case '-':
              goto label_17;
            case '/':
              goto label_21;
            case 'I':
              goto label_16;
            case 'N':
              goto label_15;
            case '[':
              goto label_24;
            case ']':
              goto label_25;
            case 'f':
              goto label_7;
            case 'n':
              goto label_8;
            case 't':
              goto label_6;
            case 'u':
              goto label_22;
            case '{':
              goto label_23;
            default:
              goto label_31;
          }
        }
        while (this.ReadData(false) != 0);
        break;
label_4:
        ++this._charPos;
        continue;
label_28:
        this.ProcessCarriageReturn(false);
        continue;
label_29:
        this.ProcessLineFeed();
        continue;
label_30:
        ++this._charPos;
        continue;
label_31:
        if (char.IsWhiteSpace(ch))
          ++this._charPos;
        else
          goto label_33;
      }
      return false;
label_5:
      this.ParseString(ch, ReadType.Read);
      return true;
label_6:
      this.ParseTrue();
      return true;
label_7:
      this.ParseFalse();
      return true;
label_8:
      if (this.EnsureChars(1, true))
      {
        switch (this._chars[this._charPos + 1])
        {
          case 'e':
            this.ParseConstructor();
            break;
          case 'u':
            this.ParseNull();
            break;
          default:
            throw this.CreateUnexpectedCharacterException(this._chars[this._charPos]);
        }
        return true;
      }
      ++this._charPos;
      throw this.CreateUnexpectedEndException();
label_15:
      this.ParseNumberNaN(ReadType.Read);
      return true;
label_16:
      this.ParseNumberPositiveInfinity(ReadType.Read);
      return true;
label_17:
      if (this.EnsureChars(1, true) && this._chars[this._charPos + 1] == 'I')
        this.ParseNumberNegativeInfinity(ReadType.Read);
      else
        this.ParseNumber(ReadType.Read);
      return true;
label_21:
      this.ParseComment(true);
      return true;
label_22:
      this.ParseUndefined();
      return true;
label_23:
      ++this._charPos;
      this.SetToken(JsonToken.StartObject);
      return true;
label_24:
      ++this._charPos;
      this.SetToken(JsonToken.StartArray);
      return true;
label_25:
      ++this._charPos;
      this.SetToken(JsonToken.EndArray);
      return true;
label_26:
      this.SetToken(JsonToken.Undefined);
      return true;
label_27:
      ++this._charPos;
      this.SetToken(JsonToken.EndConstructor);
      return true;
label_33:
      if (!char.IsNumber(ch) && ch != '-' && ch != '.')
        throw this.CreateUnexpectedCharacterException(ch);
      this.ParseNumber(ReadType.Read);
      return true;
    }

    private void ProcessLineFeed()
    {
      ++this._charPos;
      this.OnNewLine(this._charPos);
    }

    private void ProcessCarriageReturn(bool append)
    {
      ++this._charPos;
      this.SetNewLine(this.EnsureChars(1, append));
    }

    private void EatWhitespace()
    {
      while (true)
      {
        char c;
        do
        {
          c = this._chars[this._charPos];
          switch (c)
          {
            case char.MinValue:
              if (this._charsUsed == this._charPos)
                continue;
              goto label_5;
            case '\n':
              goto label_7;
            case '\r':
              goto label_6;
            case ' ':
              goto label_9;
            default:
              goto label_8;
          }
        }
        while (this.ReadData(false) != 0);
        break;
label_5:
        ++this._charPos;
        continue;
label_6:
        this.ProcessCarriageReturn(false);
        continue;
label_7:
        this.ProcessLineFeed();
        continue;
label_8:
        if (!char.IsWhiteSpace(c))
          goto label_4;
label_9:
        ++this._charPos;
      }
      return;
label_4:;
    }

    private void ParseConstructor()
    {
      if (!this.MatchValueWithTrailingSeparator("new"))
        throw JsonReaderException.Create((JsonReader) this, "Unexpected content while parsing JSON.");
      this.EatWhitespace();
      int charPos1 = this._charPos;
      char c;
      while (true)
      {
        do
        {
          c = this._chars[this._charPos];
          if (c == char.MinValue)
          {
            if (this._charsUsed != this._charPos)
              goto label_6;
          }
          else
            goto label_7;
        }
        while (this.ReadData(true) != 0);
        break;
label_7:
        if (char.IsLetterOrDigit(c))
          ++this._charPos;
        else
          goto label_9;
      }
      throw JsonReaderException.Create((JsonReader) this, "Unexpected end while parsing constructor.");
label_6:
      int charPos2 = this._charPos;
      ++this._charPos;
      goto label_17;
label_9:
      switch (c)
      {
        case '\n':
          charPos2 = this._charPos;
          this.ProcessLineFeed();
          break;
        case '\r':
          charPos2 = this._charPos;
          this.ProcessCarriageReturn(true);
          break;
        default:
          if (char.IsWhiteSpace(c))
          {
            charPos2 = this._charPos;
            ++this._charPos;
            break;
          }
          if (c != '(')
            throw JsonReaderException.Create((JsonReader) this, "Unexpected character while parsing constructor: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) c));
          charPos2 = this._charPos;
          break;
      }
label_17:
      this._stringReference = new StringReference(this._chars, charPos1, charPos2 - charPos1);
      string str = this._stringReference.ToString();
      this.EatWhitespace();
      if (this._chars[this._charPos] != '(')
        throw JsonReaderException.Create((JsonReader) this, "Unexpected character while parsing constructor: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this._chars[this._charPos]));
      ++this._charPos;
      this.ClearRecentString();
      this.SetToken(JsonToken.StartConstructor, (object) str);
    }

    private void ParseNumber(ReadType readType)
    {
      this.ShiftBufferIfNeeded();
      char firstChar = this._chars[this._charPos];
      int charPos = this._charPos;
      this.ReadNumberIntoBuffer();
      this.ParseReadNumber(readType, firstChar, charPos);
    }

    private void ParseReadNumber(ReadType readType, char firstChar, int initialPosition)
    {
      this.SetPostValueState(true);
      this._stringReference = new StringReference(this._chars, initialPosition, this._charPos - initialPosition);
      bool flag1 = char.IsDigit(firstChar) && this._stringReference.Length == 1;
      bool flag2 = firstChar == '0' && this._stringReference.Length > 1 && this._stringReference.Chars[this._stringReference.StartIndex + 1] != '.' && this._stringReference.Chars[this._stringReference.StartIndex + 1] != 'e' && this._stringReference.Chars[this._stringReference.StartIndex + 1] != 'E';
      JsonToken newToken;
      object obj;
      switch (readType)
      {
        case ReadType.Read:
        case ReadType.ReadAsInt64:
          if (flag1)
          {
            obj = (object) ((long) firstChar - 48L);
            newToken = JsonToken.Integer;
            break;
          }
          if (flag2)
          {
            string str = this._stringReference.ToString();
            try
            {
              obj = (object) (str.StartsWith("0x", StringComparison.OrdinalIgnoreCase) ? Convert.ToInt64(str, 16) : Convert.ToInt64(str, 8));
            }
            catch (Exception ex)
            {
              throw this.ThrowReaderError("Input string '{0}' is not a valid number.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) str), ex);
            }
            newToken = JsonToken.Integer;
            break;
          }
          long num1;
          switch (ConvertUtils.Int64TryParse(this._stringReference.Chars, this._stringReference.StartIndex, this._stringReference.Length, out num1))
          {
            case ParseResult.Success:
              obj = (object) num1;
              newToken = JsonToken.Integer;
              break;
            case ParseResult.Overflow:
              string number = this._stringReference.ToString();
              obj = number.Length <= 380 ? JsonTextReader.BigIntegerParse(number, CultureInfo.InvariantCulture) : throw this.ThrowReaderError("JSON integer {0} is too large to parse.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this._stringReference.ToString()));
              newToken = JsonToken.Integer;
              break;
            default:
              if (this._floatParseHandling == FloatParseHandling.Decimal)
              {
                Decimal num2;
                if (ConvertUtils.DecimalTryParse(this._stringReference.Chars, this._stringReference.StartIndex, this._stringReference.Length, out num2) != ParseResult.Success)
                  throw this.ThrowReaderError("Input string '{0}' is not a valid decimal.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this._stringReference.ToString()));
                obj = (object) num2;
              }
              else
              {
                double result;
                if (!double.TryParse(this._stringReference.ToString(), NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture, out result))
                  throw this.ThrowReaderError("Input string '{0}' is not a valid number.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this._stringReference.ToString()));
                obj = (object) result;
              }
              newToken = JsonToken.Float;
              break;
          }
          break;
        case ReadType.ReadAsInt32:
          if (flag1)
            obj = (object) ((int) firstChar - 48);
          else if (flag2)
          {
            string str = this._stringReference.ToString();
            try
            {
              obj = (object) (str.StartsWith("0x", StringComparison.OrdinalIgnoreCase) ? Convert.ToInt32(str, 16) : Convert.ToInt32(str, 8));
            }
            catch (Exception ex)
            {
              throw this.ThrowReaderError("Input string '{0}' is not a valid integer.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) str), ex);
            }
          }
          else
          {
            int num3;
            switch (ConvertUtils.Int32TryParse(this._stringReference.Chars, this._stringReference.StartIndex, this._stringReference.Length, out num3))
            {
              case ParseResult.Success:
                obj = (object) num3;
                break;
              case ParseResult.Overflow:
                throw this.ThrowReaderError("JSON integer {0} is too large or small for an Int32.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this._stringReference.ToString()));
              default:
                throw this.ThrowReaderError("Input string '{0}' is not a valid integer.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this._stringReference.ToString()));
            }
          }
          newToken = JsonToken.Integer;
          break;
        case ReadType.ReadAsString:
          string s = this._stringReference.ToString();
          if (flag2)
          {
            try
            {
              if (s.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                Convert.ToInt64(s, 16);
              else
                Convert.ToInt64(s, 8);
            }
            catch (Exception ex)
            {
              throw this.ThrowReaderError("Input string '{0}' is not a valid number.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) s), ex);
            }
          }
          else if (!double.TryParse(s, NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture, out double _))
            throw this.ThrowReaderError("Input string '{0}' is not a valid number.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this._stringReference.ToString()));
          newToken = JsonToken.String;
          obj = (object) s;
          break;
        case ReadType.ReadAsDecimal:
          if (flag1)
            obj = (object) ((Decimal) firstChar - 48M);
          else if (flag2)
          {
            string str = this._stringReference.ToString();
            try
            {
              obj = (object) Convert.ToDecimal(str.StartsWith("0x", StringComparison.OrdinalIgnoreCase) ? Convert.ToInt64(str, 16) : Convert.ToInt64(str, 8));
            }
            catch (Exception ex)
            {
              throw this.ThrowReaderError("Input string '{0}' is not a valid decimal.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) str), ex);
            }
          }
          else
          {
            Decimal num4;
            if (ConvertUtils.DecimalTryParse(this._stringReference.Chars, this._stringReference.StartIndex, this._stringReference.Length, out num4) != ParseResult.Success)
              throw this.ThrowReaderError("Input string '{0}' is not a valid decimal.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this._stringReference.ToString()));
            obj = (object) num4;
          }
          newToken = JsonToken.Float;
          break;
        case ReadType.ReadAsDouble:
          if (flag1)
            obj = (object) ((double) firstChar - 48.0);
          else if (flag2)
          {
            string str = this._stringReference.ToString();
            try
            {
              obj = (object) Convert.ToDouble(str.StartsWith("0x", StringComparison.OrdinalIgnoreCase) ? Convert.ToInt64(str, 16) : Convert.ToInt64(str, 8));
            }
            catch (Exception ex)
            {
              throw this.ThrowReaderError("Input string '{0}' is not a valid double.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) str), ex);
            }
          }
          else
          {
            double result;
            if (!double.TryParse(this._stringReference.ToString(), NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture, out result))
              throw this.ThrowReaderError("Input string '{0}' is not a valid double.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this._stringReference.ToString()));
            obj = (object) result;
          }
          newToken = JsonToken.Float;
          break;
        default:
          throw JsonReaderException.Create((JsonReader) this, "Cannot read number value as type.");
      }
      this.ClearRecentString();
      this.SetToken(newToken, obj, false);
    }

    private JsonReaderException ThrowReaderError(string message, Exception? ex = null)
    {
      this.SetToken(JsonToken.Undefined, (object) null, false);
      return JsonReaderException.Create((JsonReader) this, message, ex);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static object BigIntegerParse(string number, CultureInfo culture) => (object) BigInteger.Parse(number, (IFormatProvider) culture);

    private void ParseComment(bool setToken)
    {
      ++this._charPos;
      if (!this.EnsureChars(1, false))
        throw JsonReaderException.Create((JsonReader) this, "Unexpected end while parsing comment.");
      bool flag;
      if (this._chars[this._charPos] == '*')
      {
        flag = false;
      }
      else
      {
        if (this._chars[this._charPos] != '/')
          throw JsonReaderException.Create((JsonReader) this, "Error parsing comment. Expected: *, got {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this._chars[this._charPos]));
        flag = true;
      }
      ++this._charPos;
      int charPos = this._charPos;
      while (true)
      {
        do
        {
          do
          {
            switch (this._chars[this._charPos])
            {
              case char.MinValue:
                if (this._charsUsed == this._charPos)
                  continue;
                goto label_14;
              case '\n':
                goto label_20;
              case '\r':
                goto label_17;
              case '*':
                goto label_15;
              default:
                goto label_23;
            }
          }
          while (this.ReadData(true) != 0);
          if (!flag)
            throw JsonReaderException.Create((JsonReader) this, "Unexpected end while parsing comment.");
          this.EndComment(setToken, charPos, this._charPos);
          return;
label_14:
          ++this._charPos;
          continue;
label_15:
          ++this._charPos;
        }
        while (flag || !this.EnsureChars(0, true) || this._chars[this._charPos] != '/');
        break;
label_17:
        if (!flag)
        {
          this.ProcessCarriageReturn(true);
          continue;
        }
        goto label_18;
label_20:
        if (!flag)
        {
          this.ProcessLineFeed();
          continue;
        }
        goto label_21;
label_23:
        ++this._charPos;
      }
      this.EndComment(setToken, charPos, this._charPos - 1);
      ++this._charPos;
      return;
label_18:
      this.EndComment(setToken, charPos, this._charPos);
      return;
label_21:
      this.EndComment(setToken, charPos, this._charPos);
    }

    private void EndComment(bool setToken, int initialPosition, int endPosition)
    {
      if (!setToken)
        return;
      this.SetToken(JsonToken.Comment, (object) new string(this._chars, initialPosition, endPosition - initialPosition));
    }

    private bool MatchValue(string value) => this.MatchValue(this.EnsureChars(value.Length - 1, true), value);

    private bool MatchValue(bool enoughChars, string value)
    {
      if (!enoughChars)
      {
        this._charPos = this._charsUsed;
        throw this.CreateUnexpectedEndException();
      }
      for (int index = 0; index < value.Length; ++index)
      {
        if ((int) this._chars[this._charPos + index] != (int) value[index])
        {
          this._charPos += index;
          return false;
        }
      }
      this._charPos += value.Length;
      return true;
    }

    private bool MatchValueWithTrailingSeparator(string value)
    {
      if (!this.MatchValue(value))
        return false;
      return !this.EnsureChars(0, false) || this.IsSeparator(this._chars[this._charPos]) || this._chars[this._charPos] == char.MinValue;
    }

    private bool IsSeparator(char c)
    {
      switch (c)
      {
        case '\t':
        case '\n':
        case '\r':
        case ' ':
          return true;
        case ')':
          if (this.CurrentState == JsonReader.State.Constructor || this.CurrentState == JsonReader.State.ConstructorStart)
            return true;
          break;
        case ',':
        case ']':
        case '}':
          return true;
        case '/':
          if (!this.EnsureChars(1, false))
            return false;
          char ch = this._chars[this._charPos + 1];
          return ch == '*' || ch == '/';
        default:
          if (char.IsWhiteSpace(c))
            return true;
          break;
      }
      return false;
    }

    private void ParseTrue()
    {
      if (!this.MatchValueWithTrailingSeparator(JsonConvert.True))
        throw JsonReaderException.Create((JsonReader) this, "Error parsing boolean value.");
      this.SetToken(JsonToken.Boolean, (object) true);
    }

    private void ParseNull()
    {
      if (!this.MatchValueWithTrailingSeparator(JsonConvert.Null))
        throw JsonReaderException.Create((JsonReader) this, "Error parsing null value.");
      this.SetToken(JsonToken.Null);
    }

    private void ParseUndefined()
    {
      if (!this.MatchValueWithTrailingSeparator(JsonConvert.Undefined))
        throw JsonReaderException.Create((JsonReader) this, "Error parsing undefined value.");
      this.SetToken(JsonToken.Undefined);
    }

    private void ParseFalse()
    {
      if (!this.MatchValueWithTrailingSeparator(JsonConvert.False))
        throw JsonReaderException.Create((JsonReader) this, "Error parsing boolean value.");
      this.SetToken(JsonToken.Boolean, (object) false);
    }

    private object ParseNumberNegativeInfinity(ReadType readType) => this.ParseNumberNegativeInfinity(readType, this.MatchValueWithTrailingSeparator(JsonConvert.NegativeInfinity));

    private object ParseNumberNegativeInfinity(ReadType readType, bool matched)
    {
      if (matched)
      {
        if (readType != ReadType.Read)
        {
          if (readType != ReadType.ReadAsString)
          {
            if (readType != ReadType.ReadAsDouble)
              goto label_7;
          }
          else
          {
            this.SetToken(JsonToken.String, (object) JsonConvert.NegativeInfinity);
            return (object) JsonConvert.NegativeInfinity;
          }
        }
        if (this._floatParseHandling == FloatParseHandling.Double)
        {
          this.SetToken(JsonToken.Float, (object) double.NegativeInfinity);
          return (object) double.NegativeInfinity;
        }
label_7:
        throw JsonReaderException.Create((JsonReader) this, "Cannot read -Infinity value.");
      }
      throw JsonReaderException.Create((JsonReader) this, "Error parsing -Infinity value.");
    }

    private object ParseNumberPositiveInfinity(ReadType readType) => this.ParseNumberPositiveInfinity(readType, this.MatchValueWithTrailingSeparator(JsonConvert.PositiveInfinity));

    private object ParseNumberPositiveInfinity(ReadType readType, bool matched)
    {
      if (matched)
      {
        if (readType != ReadType.Read)
        {
          if (readType != ReadType.ReadAsString)
          {
            if (readType != ReadType.ReadAsDouble)
              goto label_7;
          }
          else
          {
            this.SetToken(JsonToken.String, (object) JsonConvert.PositiveInfinity);
            return (object) JsonConvert.PositiveInfinity;
          }
        }
        if (this._floatParseHandling == FloatParseHandling.Double)
        {
          this.SetToken(JsonToken.Float, (object) double.PositiveInfinity);
          return (object) double.PositiveInfinity;
        }
label_7:
        throw JsonReaderException.Create((JsonReader) this, "Cannot read Infinity value.");
      }
      throw JsonReaderException.Create((JsonReader) this, "Error parsing Infinity value.");
    }

    private object ParseNumberNaN(ReadType readType) => this.ParseNumberNaN(readType, this.MatchValueWithTrailingSeparator(JsonConvert.NaN));

    private object ParseNumberNaN(ReadType readType, bool matched)
    {
      if (matched)
      {
        if (readType != ReadType.Read)
        {
          if (readType != ReadType.ReadAsString)
          {
            if (readType != ReadType.ReadAsDouble)
              goto label_7;
          }
          else
          {
            this.SetToken(JsonToken.String, (object) JsonConvert.NaN);
            return (object) JsonConvert.NaN;
          }
        }
        if (this._floatParseHandling == FloatParseHandling.Double)
        {
          this.SetToken(JsonToken.Float, (object) double.NaN);
          return (object) double.NaN;
        }
label_7:
        throw JsonReaderException.Create((JsonReader) this, "Cannot read NaN value.");
      }
      throw JsonReaderException.Create((JsonReader) this, "Error parsing NaN value.");
    }

    public override void Close()
    {
      base.Close();
      if (this._chars != null)
      {
        BufferUtils.ReturnBuffer(this._arrayPool, this._chars);
        this._chars = (char[]) null;
      }
      if (this.CloseInput)
        this._reader?.Close();
      this._stringBuffer.Clear(this._arrayPool);
    }

    public bool HasLineInfo() => true;

    public int LineNumber => this.CurrentState == JsonReader.State.Start && this.LinePosition == 0 && this.TokenType != JsonToken.Comment ? 0 : this._lineNumber;

    public int LinePosition => this._charPos - this._lineStartPos;
  }
}
