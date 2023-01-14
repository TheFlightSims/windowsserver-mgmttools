using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WSUSOnlineDescriptions
{
	public class WSUSLanguage
	{
		string shortLanguage;
		string longLanguage;

		public string ShortLanguage
		{
			get
			{
				return this.shortLanguage;
			}
		}

		public string LongLanguage
		{
			get
			{
				return this.longLanguage;
			}
		}

		public WSUSLanguage(string shortLanguage, string longLanguage)
		{
			this.shortLanguage = shortLanguage;
			this.longLanguage = longLanguage;
		}

		public override string ToString()
		{
			return String.Format("{0} ({1})", this.longLanguage, this.shortLanguage);
		}
	}
}