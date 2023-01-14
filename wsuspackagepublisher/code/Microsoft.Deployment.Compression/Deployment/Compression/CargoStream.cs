// Decompiled with JetBrains decompiler
// Type: Microsoft.Deployment.Compression.CargoStream
// Assembly: Microsoft.Deployment.Compression, Version=3.0.0.0, Culture=neutral, PublicKeyToken=ce35f76fcda82bad
// MVID: 851118E5-1A5D-4354-8CEB-CAC13A1060E5
// Assembly location: C:\Users\Administrator.THEFLIGHTSIMS\Downloads\Release.v1.4.2203.19\Microsoft.Deployment.Compression.dll

using System;
using System.Collections.Generic;
using System.IO;

namespace Microsoft.Deployment.Compression
{
  public class CargoStream : Stream
  {
    private Stream source;
    private List<IDisposable> cargo;

    public CargoStream(Stream source, params IDisposable[] cargo)
    {
      this.source = source != null ? source : throw new ArgumentNullException(nameof (source));
      this.cargo = new List<IDisposable>((IEnumerable<IDisposable>) cargo);
    }

    public Stream Source => this.source;

    public IList<IDisposable> Cargo => (IList<IDisposable>) this.cargo;

    public override bool CanRead => this.source.CanRead;

    public override bool CanWrite => this.source.CanWrite;

    public override bool CanSeek => this.source.CanSeek;

    public override long Length => this.source.Length;

    public override long Position
    {
      get => this.source.Position;
      set => this.source.Position = value;
    }

    public override void Flush() => this.source.Flush();

    public override void SetLength(long value) => this.source.SetLength(value);

    public override void Close()
    {
      this.source.Close();
      foreach (IDisposable disposable in this.cargo)
        disposable.Dispose();
    }

    public override int Read(byte[] buffer, int offset, int count) => this.source.Read(buffer, offset, count);

    public override void Write(byte[] buffer, int offset, int count) => this.source.Write(buffer, offset, count);

    public override long Seek(long offset, SeekOrigin origin) => this.source.Seek(offset, origin);
  }
}
