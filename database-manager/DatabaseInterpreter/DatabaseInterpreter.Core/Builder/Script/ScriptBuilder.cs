using DatabaseInterpreter.Model;
using DatabaseInterpreter.Utility;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DatabaseInterpreter.Core
{
    public class ScriptBuilder
    {
        private List<Script> scripts = new List<Script>();
        public bool FormatScript { get; set; } = true;
        public List<Script> Scripts => this.scripts;

        public void Append(Script script)
        {
            this.scripts.Add(script);
        }

        public void AppendLine(Script script)
        {
            this.Append(script);
            this.AppendLine();
        }

        public void AppendLine()
        {
            this.Append(new NewLineSript());
        }

        public void AppendRange(IEnumerable<Script> scripts)
        {
            this.scripts.AddRange(scripts);
        }

        public override string ToString()
        {
            string script = string.Join("", this.scripts.Select(item => item.Content)).Trim();

            return this.FormatScript ? this.Format(script) : script;
        }

        private string Format(string script)
        {
            Regex regex = new Regex(@"([;]+[\s]*[;]+)|(\r\n[\s]*[;])");

            return StringHelper.ToSingleEmptyLine(regex.Replace(script, ";"));
        }
    }
}
