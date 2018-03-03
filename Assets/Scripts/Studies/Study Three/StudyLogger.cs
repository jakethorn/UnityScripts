using UnityEngine;
using System.IO;

namespace Jake.Studies.Four
{
	public class StudyLogger : MonoBehaviour
	{
		public int id;
		public string dir;

		private static string log = "";

		void Start()
		{
			LogLine("Participant ID: " + id.ToString());
		}

		void OnApplicationQuit()
		{
			if (dir.Length > 0 && dir[dir.Length - 1] != '\\')
			{
				dir += "\\";
			}

			if (dir != null && dir != "")
			{
				File.WriteAllText(dir + id.ToString() + ".txt", log);
			}
		}

		public static void Log(string message)
		{
			log += message;
		}

		public static void LogLine(string message)
		{
			if (message.Length > 0 && message[message.Length - 1] != '\n')
			{
				message += "\n";
			}

			if (log.Length > 0 && log[log.Length - 1] != '\n')
			{
				log += "\n";
			}

			log += message;
		}
	}
}
