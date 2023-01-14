// Decompiled with JetBrains decompiler
// Type: Microsoft.Deployment.Compression.OffsetStream
// Assembly: Microsoft.Deployment.Compression, Version=3.0.0.0, Culture=neutral, PublicKeyToken=ce35f76fcda82bad
// MVID: 851118E5-1A5D-4354-8CEB-CAC13A1060E5
// Assembly location: C:\Users\Administrator.THEFLIGHTSIMS\Downloads\Release.v1.4.2203.19\Microsoft.Deployment.Compression.dll

using System;
using System.IO;

namespace Microsoft.Deployment.Compression
{
  public class OffsetStream : Stream
  {
    private Stream source;
    private long sourceOffset;

    public OffsetStream(Stream source, long offset)
    {
      this.source = source != null ? source : throw new ArgumentNullException(nameof (source));
      this.sourceOffset = offset;
      this.source.Seek(this.sourceOffset, SeekOrigin.Current);
    }

    public Stream Source => this.source;

    public long Offset => this.sourceOffset;

    public override bool CanRead => this.source.CanRead;

    public override bool CanWrite => this.source.CanWrite;

    public override bool CanSeek => this.source.CanSeek;

    public override long Length => checked (this.source.Length - this.sourceOffset);

    public override long Position
    {
      get => checked (this.source.Position - this.sourceOffset);
      set => this.source.Position = checked (value + this.sourceOffset);
    }

    public override int Read(byte[] buffer, int offset, int count) => this.source.Read(buffer, offset, count);

    public override void Write(byte[] buffer, int offset, int count) => this.source.Write(buffer, offset, count);

    public override int ReadByte() => this.source.ReadByte();

    public override void WriteByte(byte value) => this.source.WriteByte(value);

    public override void Flush() => this.source.Flush();

    public override long Seek(long offset, SeekOrigin origin) => checked (this.source.Seek(offset + (unchecked (origin == SeekOrigin.Begin) ? this.sourceOffset : 0L), origin) - this.sourceOffset);

    public override void SetLength(long value) => this.source.SetLength(checked (value + this.sourceOffset));

    public override void Close() => this.source.Close();
  }
}
