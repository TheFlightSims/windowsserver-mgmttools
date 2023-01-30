using DeploymentToolkit.RegistryWrapper;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DeploymentToolkit.Actions.Utils
{
    public static class User
    {
        private const string DefaultUserProfile = @"C:\Users\Default";
        private const string PublicUserProfile = @"C:\Users\Public";

        private const string CompatibilityAllUserProfile = @"C:\Users\All Users";
        private const string CompatibilityDefaultUserProfile = @"C:\Users\Default User";

        private static List<string> _userFolders;
        public static List<string> GetUserFolders(bool includeDefaultProfile, bool includePublicProfile)
        {
            if(_userFolders == null)
            {
                var userDirectories = Directory.GetDirectories(@"C:\Users").ToList();

                if(userDirectories.Contains(DefaultUserProfile))
                {
                    userDirectories.Remove(DefaultUserProfile);
                }

                if(userDirectories.Contains(PublicUserProfile))
                {
                    userDirectories.Remove(PublicUserProfile);
                }

                if(userDirectories.Contains(CompatibilityAllUserProfile))
                {
                    userDirectories.Remove(CompatibilityAllUserProfile);
                }

                if(userDirectories.Contains(CompatibilityDefaultUserProfile))
                {
                    userDirectories.Remove(CompatibilityDefaultUserProfile);
                }

                _userFolders = userDirectories;
            }

            if(!includeDefaultProfile && !includePublicProfile)
            {
                return _userFolders;
            }

            var copy = _userFolders.ToList();
            if(includeDefaultProfile)
            {
                copy.Add(DefaultUserProfile);
            }

            if(includePublicProfile)
            {
                copy.Add(PublicUserProfile);
            }

            return copy;
        }

        private static List<string> _userRegisty;
        private static List<string> _specialUserRegistry;
        public static List<string> GetUserRegistry(bool includeDefaultProfile, bool includeSpecialProfiles)
        {
            if(_userRegisty == null)
            {
                var registry = new Win64Registry();

                _userRegisty = registry
                    .GetSubKeys("HKEY_USERS")
                    .Where((p) => p.Length > 8 && !p.EndsWith("_Classes")) // Exclude special profiles like S-1-5-20 and .DEFAULT | Exclude classes
                    .Select((p) => $@"HKEY_USERS\{p}")
                    .ToList();

                _specialUserRegistry = registry
                    .GetSubKeys("HKEY_USERS")
                    .Where((p) => p.Length == 8 && p.StartsWith("S-"))
                    .Select((p) => $@"HKEY_USERS\{p}")
                    .ToList();
            }

            if(!includeDefaultProfile && !includeSpecialProfiles)
            {
                return _userRegisty;
            }

            var copy = _userRegisty.ToList();
            if(includeDefaultProfile)
            {
                copy.Add(".DEFAULT");
            }

            if(includeSpecialProfiles)
            {
                copy.AddRange(_specialUserRegistry);
            }

            return copy;
        }
    }
}
