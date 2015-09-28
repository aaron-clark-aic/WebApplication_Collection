using System;
namespace SQLHelper
{
	public class SystemHTML
	{
		private static string HTMLEncode(string fString)
		{
			if (fString != string.Empty)
			{
				fString.Replace("<", "&lt;");
				fString.Replace(">", "&rt;");
				fString.Replace('"'.ToString(), "&quot;");
				fString.Replace('\''.ToString(), "&#39;");
				fString.Replace('\r'.ToString(), "");
				fString.Replace('\n'.ToString(), "<BR> ");
			}
			return fString;
		}
	}
}
