using System.Collections.Generic;

namespace WinMan.Registry.Models
{
    public class KeyModel
    {
        public string Name { get; set; }
        public List<KeyModel> Keys { get; set; }
        public List<ValueModel> Values { get; set; }
    }
}
