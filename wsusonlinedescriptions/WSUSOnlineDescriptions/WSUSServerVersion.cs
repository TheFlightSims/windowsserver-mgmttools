using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WSUSOnlineDescriptions
{
	public class WSUSServerVersion
	{
		public string Name { get; set; }

		public string DatabaseName { get; set; }

		public WSUSServerVersion(string name, string databaseName)
		{
			this.Name = name;
			this.DatabaseName = databaseName;
		}

		public override string ToString()
		{
			return this.Name;
		}
	}
}