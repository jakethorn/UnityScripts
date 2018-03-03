using UnityEditor;
using UnityEngine;
using System;

namespace Jake
{
	public class LogWindow : MonoBehaviour
	{
		[MenuItem("Jake/Open Log Folder", priority = 200)]
		static void OpenLogFolder()
		{
			throw new NotImplementedException();

			//var path = @"C:\Users\Jake\AppData\Local\Unity\Editor\Editor.log";
			//System.Diagnostics.Process.Start("explorer.exe", "/select," + path);
		}
	}
}
