// Decompiled with JetBrains decompiler
// Type: Microsoft.Deployment.Compression.DuplicateStream
// Assembly: Microsoft.Deployment.Compression, Version=3.0.0.0, Culture=neutral, PublicKeyToken=ce35f76fcda82bad
// MVID: 851118E5-1A5D-4354-8CEB-CAC13A1060E5
// Assembly location: C:\Users\Administrator.THEFLIGHTSIMS\Downloads\Release.v1.4.2203.19\Microsoft.Deployment.Compression.dll

using System;
using System.IO;

namespace Microsoft.Deployment.Compression
{
  public class DuplicateStream : Stream
  {
    private Stream source;
    private long position;

    public DuplicateStream(Stream source) => this.source = source != null ? DuplicateStream.OriginalStream(source) : throw new ArgumentNullException(nameof (source));

    public Stream Source => this.source;

    public override bool CanRead => this.source.CanRead;

    public override bool CanWrite => this.source.CanWrite;

    public override bool CanSeek => this.source.CanSeek;

    public override long Length => this.source.Length;

    public override long Position
    {
      get => this.position;
      set => this.position = value;
    }

    public static Stream OriginalStream(Stream stream) => !(stream is DuplicateStream duplicateStream) ? stream : duplicateStream.Source;

    public override void Flush() => this.source.Flush();

    public override void SetLength(long value) => this.source.SetLength(value);

    public override void Close() => this.source.Close();

    public override int Read(byte[] buffer, int offset, int count)
    {
      long position = this.source.Position;
      this.source.Position = this.position;
      int num = this.source.Read(buffer, offset, count);
      this.position = this.source.Position;
      this.source.Position = position;
      return num;
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
      long position = this.source.Position;
      this.source.Position = this.position;
      this.source.Write(buffer, offset, count);
      this.position = this.source.Position;
      this.source.Position = position;
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
      long num = 0;
      switch (origin)
      {
        case SeekOrigin.Current:
          num = this.position;
          break;
        case SeekOrigin.End:
          num = this.Length;
          break;
      }
      this.position = checked (num + offset);
      return this.position;
    }
  }
}
