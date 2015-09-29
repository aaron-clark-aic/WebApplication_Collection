using System;
using System.IO;
namespace SQLHelper
{
	public class SystemError
	{
		private static string _fileName;
		public static string FileName
		{
			get
			{
				return SystemError._fileName;
			}
			set
			{
				if (!String.IsNullOrEmpty(value))
				{
					SystemError._fileName = value;
					EnableLog = true;
				}
			}
		}
		public static bool EnableLog { get; set; }

		static SystemError()
		{
			FileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, String.Format("SqlHelper_Err_{0}.log", DateTime.Today.ToString("yyyyMMdd")));
			EnableLog = false;
		}

		public static void SystemLog(string message)
		{
			if (EnableLog)
			{
				string fileName = _fileName;
				lock (fileName)
				{
					StreamWriter sr = null;
					try
					{
						if (File.Exists(fileName))
						{
							///�����־�ļ��Ѿ����ڣ���ֱ��д����־�ļ�
							sr = File.AppendText(fileName);
						}
						else
						{
							///������־�ļ�
							sr = File.CreateText(fileName);
						}
						sr.WriteLine(DateTime.Now.ToString() + ": " + message);
						sr.WriteLine();
					}
					catch (Exception)
					{
						EnableLog = false;
					}
					finally
					{
						if (sr != null)
						{
							sr.Dispose();
						}
					}
				}
			}
		}
	}
}
