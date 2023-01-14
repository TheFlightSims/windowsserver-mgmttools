namespace UserRights.Application;

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security.Principal;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.Security.Authentication.Identity;
using Windows.Win32.System.WindowsProgramming;

/// <summary>
/// Represents a managed wrapper around the local security authority user right functions.
/// </summary>
public class LsaUserRights : ILsaUserRights, IDisposable
{
    private bool disposed;
    private LsaCloseSafeHandle? handle;

    /// <inheritdoc />
    public void Connect(string? systemName = default)
    {
        if (this.disposed)
        {
            throw new ObjectDisposedException(this.GetType().FullName);
        }

        if (this.handle != null)
        {
            throw new InvalidOperationException("A connection to the policy database already exists.");
        }

        OBJECT_ATTRIBUTES objectAttributes = default;

        const uint desiredAccess = PInvoke.POLICY_CREATE_ACCOUNT |
            PInvoke.POLICY_LOOKUP_NAMES |
            PInvoke.POLICY_VIEW_LOCAL_INFORMATION;

        this.handle = this.LsaOpenPolicy(ref objectAttributes, desiredAccess, systemName);
    }

    /// <inheritdoc />
    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <inheritdoc />
    public unsafe void LsaAddAccountRights(SecurityIdentifier accountSid, params string[] userRights)
    {
        if (this.disposed)
        {
            throw new ObjectDisposedException(this.GetType().FullName);
        }

        if (this.handle is null)
        {
            throw new InvalidOperationException("A connection to the policy database is required.");
        }

        ArgumentNullException.ThrowIfNull(accountSid);
        ArgumentNullException.ThrowIfNull(userRights);

        if (userRights.Length == 0)
        {
            throw new ArgumentOutOfRangeException(nameof(userRights), "Value cannot be an empty collection.");
        }

        var bytes = new byte[accountSid.BinaryLength];
        accountSid.GetBinaryForm(bytes, 0);
        PSID psid;
        fixed (byte* b = bytes)
        {
            psid = new PSID(b);
        }

        Span<UNICODE_STRING> rights = stackalloc UNICODE_STRING[userRights.Length];
        for (var i = 0; i < userRights.Length; i++)
        {
            var privilege = userRights[i];

            fixed (char* p = privilege)
            {
                var length = checked((ushort)(privilege.Length * sizeof(char)));

                rights[i] = new UNICODE_STRING
                {
                    Length = length,
                    MaximumLength = length,
                    Buffer = p
                };
            }
        }

        var status = PInvoke.LsaAddAccountRights(this.handle, psid, rights);
        var error = PInvoke.LsaNtStatusToWinError(status);

        if ((WIN32_ERROR)error != WIN32_ERROR.ERROR_SUCCESS)
        {
            throw new Win32Exception((int)error);
        }
    }

    /// <inheritdoc />
    public unsafe string[] LsaEnumerateAccountRights(SecurityIdentifier accountSid)
    {
        if (this.disposed)
        {
            throw new ObjectDisposedException(this.GetType().FullName);
        }

        if (this.handle is null)
        {
            throw new InvalidOperationException("A connection to the policy database is required.");
        }

        ArgumentNullException.ThrowIfNull(accountSid);

        var bytes = new byte[accountSid.BinaryLength];
        accountSid.GetBinaryForm(bytes, 0);
        PSID psid;
        fixed (byte* b = bytes)
        {
            psid = new PSID(b);
        }

        UNICODE_STRING* userRights = default;
        try
        {
            var status = PInvoke.LsaEnumerateAccountRights(this.handle, psid, out userRights, out var count);
            var error = (WIN32_ERROR)PInvoke.LsaNtStatusToWinError(status);

            if (error != WIN32_ERROR.ERROR_SUCCESS)
            {
                throw new Win32Exception((int)error);
            }

            var results = new string[count];

            for (var i = 0; i < count; i++)
            {
                var offset = Marshal.SizeOf(typeof(UNICODE_STRING)) * i;
                var ptr = nint.Add((nint)userRights, offset);
                var result = Marshal.PtrToStructure(ptr, typeof(UNICODE_STRING)) ?? throw new InvalidOperationException();
                var unicodeString = (UNICODE_STRING)result;

                results[i] = new string(unicodeString.Buffer.Value);
            }

            return results;
        }
        finally
        {
            if (userRights is not null)
            {
                PInvoke.LsaFreeMemory(userRights);
            }
        }
    }

    /// <inheritdoc />
    public unsafe SecurityIdentifier[] LsaEnumerateAccountsWithUserRight(string? userRight = default)
    {
        if (this.disposed)
        {
            throw new ObjectDisposedException(this.GetType().FullName);
        }

        if (this.handle is null)
        {
            throw new InvalidOperationException("A connection to the policy database is required.");
        }

        UNICODE_STRING userRightUnicode = default;
        if (userRight is not null)
        {
            fixed (char* c = userRight)
            {
                var length = checked((ushort)(userRight.Length * sizeof(char)));

                userRightUnicode.Length = length;
                userRightUnicode.MaximumLength = length;
                userRightUnicode.Buffer = c;
            }
        }

        void* buffer = default;
        try
        {
            var status = PInvoke.LsaEnumerateAccountsWithUserRight(this.handle, userRightUnicode, out buffer, out var count);
            var error = (WIN32_ERROR)PInvoke.LsaNtStatusToWinError(status);

            if (error == WIN32_ERROR.ERROR_NO_MORE_ITEMS)
            {
                return Array.Empty<SecurityIdentifier>();
            }

            if (error != WIN32_ERROR.ERROR_SUCCESS)
            {
                throw new Win32Exception((int)error);
            }

            var results = new SecurityIdentifier[count];

            for (var i = 0; i < count; i++)
            {
                var offset = Marshal.SizeOf(typeof(LSA_ENUMERATION_INFORMATION)) * i;
                var result = Marshal.PtrToStructure(nint.Add((nint)buffer, offset), typeof(LSA_ENUMERATION_INFORMATION)) ?? throw new InvalidOperationException();
                var sid = ((LSA_ENUMERATION_INFORMATION)result).Sid;

                results[i] = new SecurityIdentifier((nint)sid.Value);
            }

            return results;
        }
        finally
        {
            if (buffer is not null)
            {
                PInvoke.LsaFreeMemory(buffer);
            }
        }
    }

    /// <inheritdoc />
    public unsafe void LsaRemoveAccountRights(SecurityIdentifier accountSid, params string[] userRights)
    {
        if (this.disposed)
        {
            throw new ObjectDisposedException(this.GetType().FullName);
        }

        if (this.handle is null)
        {
            throw new InvalidOperationException("A connection to the policy database is required.");
        }

        ArgumentNullException.ThrowIfNull(accountSid);
        ArgumentNullException.ThrowIfNull(userRights);

        if (userRights.Length == 0)
        {
            throw new ArgumentOutOfRangeException(nameof(userRights), "Value cannot be an empty collection.");
        }

        var bytes = new byte[accountSid.BinaryLength];
        accountSid.GetBinaryForm(bytes, 0);
        PSID psid;
        fixed (byte* b = bytes)
        {
            psid = new PSID(b);
        }

        Span<UNICODE_STRING> rights = stackalloc UNICODE_STRING[userRights.Length];
        for (var i = 0; i < userRights.Length; i++)
        {
            var privilege = userRights[i];

            fixed (char* p = privilege)
            {
                var length = checked((ushort)(privilege.Length * sizeof(char)));

                rights[i] = new UNICODE_STRING
                {
                    Length = length,
                    MaximumLength = length,
                    Buffer = p
                };
            }
        }

        var status = PInvoke.LsaRemoveAccountRights(this.handle, psid, false, rights);
        var error = PInvoke.LsaNtStatusToWinError(status);

        if ((WIN32_ERROR)error != WIN32_ERROR.ERROR_SUCCESS)
        {
            throw new Win32Exception((int)error);
        }
    }

    /// <summary>
    /// Releases resources when they are no longer required.
    /// </summary>
    /// <param name="disposing">A value indicating whether the method call comes from a dispose method (its value is <c>true</c>) or from a finalizer (its value is <c>false</c>).</param>
    protected virtual void Dispose(bool disposing)
    {
        if (this.disposed)
        {
            return;
        }

        if (disposing)
        {
            this.handle?.Dispose();
            this.disposed = true;
        }
    }

    /// <summary>
    /// Opens a handle to the Policy object on a local or remote system.
    /// </summary>
    /// <param name="objectAttributes">The connection attributes.</param>
    /// <param name="desiredAccess">The requested access rights.</param>
    /// <param name="systemName">The name of the target system.</param>
    /// <returns>A handle to the Policy object.</returns>
    private unsafe LsaCloseSafeHandle LsaOpenPolicy(ref OBJECT_ATTRIBUTES objectAttributes, uint desiredAccess, string? systemName = default)
    {
        if (this.disposed)
        {
            throw new ObjectDisposedException(this.GetType().FullName);
        }

        if (this.handle is not null)
        {
            throw new InvalidOperationException("A connection to the policy database already exists.");
        }

        UNICODE_STRING systemNameUnicode = default;
        if (systemName is not null)
        {
            fixed (char* c = systemName)
            {
                var length = checked((ushort)(systemName.Length * sizeof(char)));

                systemNameUnicode.Length = length;
                systemNameUnicode.MaximumLength = length;
                systemNameUnicode.Buffer = c;
            }
        }

        var status = PInvoke.LsaOpenPolicy(systemNameUnicode, objectAttributes, desiredAccess, out var policyHandle);
        var error = PInvoke.LsaNtStatusToWinError(status);

        if ((WIN32_ERROR)error != WIN32_ERROR.ERROR_SUCCESS)
        {
            throw new Win32Exception((int)error);
        }

        return policyHandle;
    }
}