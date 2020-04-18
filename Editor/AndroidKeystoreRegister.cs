using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace UniAndroidKeystoreRegister
{
	internal static class AndroidKeystoreRegister
	{
		private sealed class Data
		{
			[field: SerializeField] public string KeyaliasPass { get; set; } = null;
			[field: SerializeField] public string KeystorePass { get; set; } = null;
		}

		private const string FILENAME = "AndroidKeystoreRegister.json";

		private static Data m_data;

		private static string Directory => Application.persistentDataPath;
		private static string Path      => $"{Directory}/{FILENAME}";

		[InitializeOnLoadMethod]
		private static void Run()
		{
			Load();

			PlayerSettings.Android.keyaliasPass = m_data.KeyaliasPass;
			PlayerSettings.Android.keystorePass = m_data.KeystorePass;
		}

		[SettingsProvider]
		public static SettingsProvider CreateSettingsProvider()
		{
			var path = "Project/UniAndroidKeystoreRegister";
			var provider = new SettingsProvider( path, SettingsScope.Project )
			{
				label               = "UniAndroidKeystoreRegister",
				activateHandler     = OnActive,
				deactivateHandler   = OnDeactive,
				titleBarGuiHandler  = null,
				guiHandler          = OnGUI,
				footerBarGuiHandler = null,
				keywords            = new[] { "keyaliasPass", "keystorePass" },
			};

			return provider;
		}

		private static void OnActive( string searchContext, VisualElement rootElement )
		{
			Load();
		}

		private static void OnGUI( string searchContext )
		{
			using ( new EditorGUILayout.HorizontalScope() )
			{
				EditorGUILayout.PrefixLabel( "keyaliasPass" );
				m_data.KeyaliasPass = EditorGUILayout.TextField( m_data.KeyaliasPass );
			}

			using ( new EditorGUILayout.HorizontalScope() )
			{
				EditorGUILayout.PrefixLabel( "keystorePass" );
				m_data.KeystorePass = EditorGUILayout.TextField( m_data.KeystorePass );
			}

			using ( new EditorGUILayout.HorizontalScope() )
			{
				if ( GUILayout.Button( "Open Settings Folder" ) )
				{
					Process.Start( Directory );
				}

				if ( GUILayout.Button( "Open Settings File" ) )
				{
					Process.Start( Path );
				}

				if ( GUILayout.Button( "Save" ) )
				{
					Save();
				}
			}
		}

		private static void OnDeactive()
		{
			Save();
		}

		private static void Load()
		{
			if ( !File.Exists( Path ) )
			{
				m_data = new Data();
				return;
			}

			var reader = new StreamReader( Path );
			var json   = reader.ReadToEnd();

			m_data = JsonUtility.FromJson<Data>( json );

			reader.Close();
		}

		private static void Save()
		{
			var writer = new StreamWriter( Path );
			var json   = JsonUtility.ToJson( m_data );

			writer.Write( json );

			writer.Close();
		}
	}
}