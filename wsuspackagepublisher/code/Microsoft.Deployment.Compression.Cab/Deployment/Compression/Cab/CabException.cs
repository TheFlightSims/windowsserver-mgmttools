// Decompiled with JetBrains decompiler
// Type: Microsoft.Deployment.Compression.Cab.CabException
// Assembly: Microsoft.Deployment.Compression.Cab, Version=3.0.0.0, Culture=neutral, PublicKeyToken=ce35f76fcda82bad
// MVID: FD30E6CD-702E-43F8-86E3-5A8B35FB59B3
// Assembly location: C:\Users\Administrator.THEFLIGHTSIMS\Downloads\Release.v1.4.2203.19\Microsoft.Deployment.Compression.Cab.dll

using System;
using System.Globalization;
using System.Resources;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Microsoft.Deployment.Compression.Cab
{
  [Serializable]
  public class CabException : ArchiveException
  {
    private static ResourceManager errorResources;
    private int error;
    private int errorCode;

    public CabException(string message, Exception innerException)
      : this(0, 0, message, innerException)
    {
    }

    public CabException(string message)
      : this(0, 0, message, (Exception) null)
    {
    }

    public CabException()
      : this(0, 0, (string) null, (Exception) null)
    {
    }

    internal CabException(int error, int errorCode, string message, Exception innerException)
      : base(message, innerException)
    {
      this.error = error;
      this.errorCode = errorCode;
    }

    internal CabException(int error, int errorCode, string message)
      : this(error, errorCode, message, (Exception) null)
    {
    }

    protected CabException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
      this.error = info != null ? info.GetInt32("cabError") : throw new ArgumentNullException(nameof (info));
      this.errorCode = info.GetInt32("cabErrorCode");
    }

    public int Error => this.error;

    public int ErrorCode => this.errorCode;

    internal static ResourceManager ErrorResources
    {
      get
      {
        if (CabException.errorResources == null)
          CabException.errorResources = new ResourceManager(typeof (CabException).Namespace + ".Errors", typeof (CabException).Assembly);
        return CabException.errorResources;
      }
    }

    [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      if (info == null)
        throw new ArgumentNullException(nameof (info));
      info.AddValue("cabError", this.error);
      info.AddValue("cabErrorCode", this.errorCode);
      base.GetObjectData(info, context);
    }

    internal static string GetErrorMessage(int error, int errorCode, bool extracting)
    {
      int num = extracting ? 2000 : 1000;
      string errorMessage = CabException.ErrorResources.GetString(checked (num + error).ToString((IFormatProvider) CultureInfo.InvariantCulture.NumberFormat), CultureInfo.CurrentCulture) ?? CabException.ErrorResources.GetString(num.ToString((IFormatProvider) CultureInfo.InvariantCulture.NumberFormat), CultureInfo.CurrentCulture);
      if (errorCode != 0)
        errorMessage = string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0} " + CabException.ErrorResources.GetString("1", CultureInfo.CurrentCulture), (object) errorMessage, (object) errorCode);
      return errorMessage;
    }
  }
}
