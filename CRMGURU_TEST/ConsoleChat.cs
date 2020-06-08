using System;
using System.Collections.Generic;
using System.Text;

namespace CRMGURU_TEST
{
	public static class ConsoleChat
	{
		public static char MenuForUser(string msg)
		{
			Console.WriteLine('\n' + msg + '\n');
			var key = Console.ReadKey().KeyChar;
			Console.WriteLine();
			return key;
		}
		public static string ChatWithUser(string msg)
		{
			Console.WriteLine('\n' + msg + '\n');
			var str = Console.ReadLine();
			Console.WriteLine();
			return str;
		}

		public static void WriteLine(params object[] str)
		{
			Console.WriteLine(str);
		}
	}
}
