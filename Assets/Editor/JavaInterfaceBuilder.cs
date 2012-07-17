using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

using Debug = UnityEngine.Debug;


public class JavaInterfaceBuilder : AssetPostprocessor
{
	const string
		kBuildPath = "Assets/Plugins/Android/src/",
		kPerlPath = "/usr/bin/perl",
		kBuildScriptName = "build.pl";


	static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromPath)
	{
		List<string> changedPathes = new List<string> ();

		changedPathes.AddRange (importedAssets);
		changedPathes.AddRange (deletedAssets);
		changedPathes.AddRange (movedAssets);
		changedPathes.AddRange (movedFromPath);

		foreach (string path in changedPathes)
		{
			if (path.IndexOf (kBuildPath) == 0 && path.IndexOf (".java") == path.Length - ".java".Length)
			{
				Build ();
				return;
			}
		}
	}


	[MenuItem ("Assets/Build Java interface")]
	public static void Build ()
	{
		string workingDirectory = Application.dataPath;
		workingDirectory = workingDirectory.Substring (0, workingDirectory.Length - "Assets/".Length) + "/" + kBuildPath;

		ProcessStartInfo startInfo = new ProcessStartInfo ()
		{
			WorkingDirectory = workingDirectory.Replace ('/', Path.DirectorySeparatorChar),
			FileName = kPerlPath,
			Arguments = kBuildScriptName,
			UseShellExecute = false,
			RedirectStandardError = true,

		};
		Process process = Process.Start (startInfo);
		process.WaitForExit ();

		if (!process.StandardError.EndOfStream)
		{
			Debug.LogError (process.StandardError.ReadToEnd ());
		}

		AssetDatabase.Refresh ();
	}
}
