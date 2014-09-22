using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ASUControl
{
	class SaveSettings
	{
		public static void Main2()
		{
			string[] lines;
			lines = System.IO.File.ReadAllLines(@"C:\Users\N1amr\Desktop\New Text Document.txt");
			String[] temp = new String[lines.Length+1];
			for (int i = 0; i < lines.Length; i++)
			{
				temp[i] = lines[i];
			}
			temp[lines.Length] = "hiiii";
			System.IO.File.WriteAllLines(@"C:\Users\N1amr\Desktop\New Text Document.txt", temp);

			//System.IO.StreamWriter file = new StreamWriter(@"C:\Users\N1amr\Desktop\New Text Document.txt", true);
			//System.IO.StreamReader file2 = new StreamReader(@"C:\Users\N1amr\Desktop\New Text Document.txt", true);
			//file2.

			//file.WriteLine("Hello");
			//file.Close();
		}
	}
}