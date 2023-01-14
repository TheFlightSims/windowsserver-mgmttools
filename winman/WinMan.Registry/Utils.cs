using System.Collections.Generic;
using WinMan.Registry.Models;

namespace WinMan.Registry
{
    public class Util
    {
        public static KeyModel GetRegistry(string name)
        {
            if (name=="root")
            {
                return BuildRoot();
            }
            
            return null;
        }

        private static KeyModel BuildRoot()
        {
            var root = new KeyModel() {
                Name = "Server"
            };
            
            root.Keys = new List<KeyModel>();
            root.Keys.Add(new KeyModel() { Name = Microsoft.Win32.Registry.ClassesRoot.Name });
            root.Keys.Add(new KeyModel() { Name = Microsoft.Win32.Registry.CurrentUser.Name });
            root.Keys.Add(new KeyModel() { Name = Microsoft.Win32.Registry.LocalMachine.Name });
            root.Keys.Add(new KeyModel() { Name = Microsoft.Win32.Registry.Users.Name });
            root.Keys.Add(new KeyModel() { Name = Microsoft.Win32.Registry.CurrentConfig.Name });
            return root;
        }
    }
}
