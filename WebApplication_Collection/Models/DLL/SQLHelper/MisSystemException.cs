using System;
namespace SQLHelper
{
	public class MisSystemException : Exception
	{
		public MisSystemException(string source, string message, Exception inner) : base(message, inner)
		{
			Source = source;
		}
		public MisSystemException(string source, string message) : base(message)
		{
			Source = source;
		}
	}
}
