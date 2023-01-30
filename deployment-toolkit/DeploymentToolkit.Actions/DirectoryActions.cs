using DeploymentToolkit.Actions.Extensions;
using DeploymentToolkit.Actions.Utils;
using DeploymentToolkit.DeploymentEnvironment;
using NLog;
using System;
using System.IO;

namespace DeploymentToolkit.Actions
{
    public static class DirectoryActions
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public static bool DirectoryExists(string path)
        {
            _logger.Trace($"Exists({path})");
            if(string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            if(!Path.IsPathRooted(path))
            {
                path = Path.Combine(DeploymentEnvironmentVariables.FilesDirectory, path);
                _logger.Trace($"Path was a non absolute path. Changed path to '{path}'");
            }

            return Directory.Exists(path);
        }

        public static bool MoveDirectory(string source, string target, bool overwrite = false, bool recursive = true)
        {
            _logger.Trace($"MoveDirectory({source}, {target}, {overwrite}, {recursive})");
            if(string.IsNullOrEmpty(source))
            {
                throw new ArgumentNullException(nameof(source));
            }

            if(string.IsNullOrEmpty(target))
            {
                throw new ArgumentNullException(nameof(target));
            }

            if(!Path.IsPathRooted(source))
            {
                source = Path.Combine(DeploymentEnvironmentVariables.FilesDirectory, source);
                _logger.Trace($"Source path was a non absolute path. Changed path to '{source}'");
            }
            if(!Path.IsPathRooted(target))
            {
                target = Path.GetFullPath(target);
                _logger.Trace($"Target path was a non absolute path. Changed path to '{target}'");
            }

            if(!Directory.Exists(source))
            {
                _logger.Warn($"Source directory was not found in '{source}' or is not a directory");
                return false;
            }

            if(!overwrite && Directory.Exists(target))
            {
                _logger.Info($"Overwrite not specified but target directory exists. Not moveing '{source}' to '{target}'");
                return false;
            }
            else if(overwrite && Directory.Exists(target))
            {
                _logger.Info($"Target directory exists. Deleting '{target}'");
                try
                {
                    Directory.Delete(target, recursive);
                }
                catch(IOException ex)
                {
                    // If recursive is set to false and the directory has subfolders, then this exception is thrown. Therefor exit ...
                    _logger.Warn(ex, $"Target directory exists and has subfolder or files. Recursive parameter was not specified. Aborting ...");
                    return false;
                }
            }

            Directory.Move(source, target);
            return true;
        }

        public static bool CopyDirectory(string source, string target, bool overwrite = false, bool recursive = true)
        {
            _logger.Trace($"CopyDirectory({source}, {target}, {overwrite}, {recursive}");
            if(string.IsNullOrEmpty(source))
            {
                throw new ArgumentNullException(nameof(source));
            }

            if(string.IsNullOrEmpty(target))
            {
                throw new ArgumentNullException(nameof(target));
            }

            if(!Path.IsPathRooted(source))
            {
                source = Path.Combine(DeploymentEnvironmentVariables.FilesDirectory, source);
                _logger.Trace($"Source path was a non absolute path. Changed path to '{source}'");
            }
            if(!Path.IsPathRooted(target))
            {
                target = Path.GetFullPath(target);
                _logger.Trace($"Target path was a non absolute path. Changed path to '{target}'");
            }

            if(!Directory.Exists(source))
            {
                _logger.Warn($"Source directory was not found in '{source}' or is not a directory");
                return false;
            }

            if(!overwrite && Directory.Exists(target))
            {
                _logger.Info($"Overwrite not specified but target directory exists. Not copying '{source}' to '{target}'");
                return false;
            }
            else if(overwrite && Directory.Exists(target))
            {
                _logger.Info($"Target directory exists. Deleting '{target}'");
                try
                {
                    Directory.Delete(target, recursive);
                }
                catch(IOException ex)
                {
                    // If recursive is set to false and the directory has subfolders, then this exception is thrown. Therefor exit ...
                    _logger.Warn(ex, $"Target directory exists and has subfolder or files. Recursive parameter was not specified. Aborting ...");
                    return false;
                }
            }

            DirectoryExtensions.Copy(source, target);
            return true;
        }

        public static bool CopyDirectoryForAllUsers(string source, string target, bool overwrite = false, bool recursive = true, bool includeDefaultProfile = false, bool includePublicProfile = false)
        {
            _logger.Trace($"CopyDirectoryForAllUsers({source}, {target}, {overwrite}, {recursive}, {includeDefaultProfile}, {includePublicProfile})");

            if(Path.IsPathRooted(target))
            {
                _logger.Error(@"Targetpath cannot be a full path! Specify relative path! (Example: Documents\Test");
                return false;
            }

            foreach(var user in User.GetUserFolders(includeDefaultProfile, includePublicProfile))
            {
                try
                {
                    _logger.Trace($"Processing '{user}'");
                    var userPath = Path.Combine(user, target);
                    CopyDirectory(source, userPath, overwrite, recursive);
                }
                catch(Exception ex)
                {
                    _logger.Error(ex, $"Failed to process '{user}'");
                    return false;
                }
            }

            return true;
        }

        public static bool DeleteDirectory(string target, bool recursive = true)
        {
            _logger.Trace($"DeleteDirectory({target}, {recursive})");
            if(string.IsNullOrEmpty(target))
            {
                throw new ArgumentNullException(nameof(target));
            }

            if(!Path.IsPathRooted(target))
            {
                target = Path.GetFullPath(target);
                _logger.Trace($"Target path was a non absolute path. Changed path to '{target}'");
            }

            if(!Directory.Exists(target))
            {
                _logger.Info($"No directory to delete");
                return true;
            }

            Directory.Delete(target, recursive);
            return true;
        }

        public static bool DeleteDirectoryForAllUsers(string target, bool recursive = true, bool includeDefaultProfile = false, bool includePublicProfile = false)
        {
            _logger.Trace($"DeleteDirectoryForAllUsers({target}, {recursive}, {includeDefaultProfile}, {includePublicProfile})");

            if(Path.IsPathRooted(target))
            {
                _logger.Error(@"Targetpath cannot be a full path! Specify relative path! (Example: Documents\Test");
                return false;
            }

            foreach(var user in User.GetUserFolders(includeDefaultProfile, includePublicProfile))
            {
                try
                {
                    _logger.Trace($"Processing '{user}'");
                    var userPath = Path.Combine(user, target);
                    DeleteDirectory(userPath, recursive);
                }
                catch(Exception ex)
                {
                    _logger.Error(ex, $"Failed to process '{user}'");
                    return false;
                }
            }

            return true;
        }

        public static bool CreateDirectory(string target)
        {
            _logger.Trace($"CreateDirectory({target})");
            if(string.IsNullOrEmpty(target))
            {
                throw new ArgumentNullException(nameof(target));
            }

            if(!Path.IsPathRooted(target))
            {
                target = Path.GetFullPath(target);
                _logger.Trace($"Target path was a non absolute path. Changed path to '{target}'");
            }

            if(Directory.Exists(target))
            {
                _logger.Info($"Directory alread exists. Nothing to create");
                return true;
            }

            Directory.CreateDirectory(target);
            return true;
        }

        public static bool CreateDirectoryForAllUsers(string target, bool includeDefaultProfile = false, bool includePublicProfile = false)
        {
            _logger.Trace($"CreateDirectoryForAllUsers({target}, {includeDefaultProfile}, {includePublicProfile})");

            if(Path.IsPathRooted(target))
            {
                _logger.Error(@"Targetpath cannot be a full path! Specify relative path! (Example: Documents\Test");
                return false;
            }

            foreach(var user in User.GetUserFolders(includeDefaultProfile, includePublicProfile))
            {
                try
                {
                    _logger.Trace($"Processing '{user}'");
                    var userPath = Path.Combine(user, target);
                    DeleteDirectory(userPath);
                }
                catch(Exception ex)
                {
                    _logger.Error(ex, $"Failed to process '{user}'");
                    return false;
                }
            }

            return true;
        }
    }
}
