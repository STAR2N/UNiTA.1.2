using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using System.IO;

namespace FrostweepGames.VoicePro
{
    [InitializeOnLoad]
    public class WelcomeDialog : EditorWindow
    {
        private static bool _punNetworkDefineEnabled;

        private static bool _mirrorNetworkDefineEnabled;

        private string _punDefine = "PUN2_NETWORK_PROVIDER";
        private string _mirrorDefine = "MIRROR_NETWORK_PROVIDER";

        private static bool _showOnStartup = true;

        private const string _PathToConfig = "/../ProjectSettings/voice_chat_pro_config.txt";

        private static bool _Inited = false;

        private static string[] _params = new string[] { "true", "false", "false" };

        static WelcomeDialog()
        {
            EditorApplication.update += Startup;
        }

        private static void Startup()
        {
            EditorApplication.update -= Startup;

            if (!File.Exists(Application.dataPath + _PathToConfig))
            {
                File.WriteAllLines(Application.dataPath + _PathToConfig, _params);
            }

            _params = File.ReadAllLines(Application.dataPath + _PathToConfig);

            if(_params.Length < 3)
			{
                _params = new string[] { "true", "false", "false" };
            }

            _showOnStartup = _params[0].Trim().ToLower() == "true";
            _punNetworkDefineEnabled = _params[1].Trim().ToLower() == "true";
            _mirrorNetworkDefineEnabled = _params[2].Trim().ToLower() == "true";

            if (_showOnStartup)
            {
                Init();
            }
        }

        [MenuItem("Window/Frostweep Games/Voice Pro")]
        private static void Init()
        {
            if (_Inited)
                return;

            WelcomeDialog window = (WelcomeDialog)GetWindow(typeof(WelcomeDialog), false, "Voice Pro", true);
            window.minSize = new Vector2(500, 400);
            window.maxSize = new Vector2(500, 400);
            window.Show();

            _Inited = true;
        }

		private void OnDestroy()
		{
            _Inited = false;
        }

		private void OnGUI()
        {
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Welcome to Frostweep Games - Voice Pro!", EditorStyles.boldLabel);

            EditorGUILayout.Space();
            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();
            if (GUILayout.Button("Asset Store Page"))
            {
                Application.OpenURL("https://assetstore.unity.com/publishers/14839");
            }
            if (GUILayout.Button("Official Discord Server"))
            {
                Application.OpenURL("https://discord.gg/TZdhnWy");
            }
            if (GUILayout.Button("Contact Us"))
            {
                Application.OpenURL("mailto: assets@frostweepgames.com");
            }
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Tools");

            if (GUILayout.Button("Add Voice Pro Prefab to Scene"))
            {
                Undo.RegisterCreatedObjectUndo(PrefabUtility.InstantiatePrefab(Resources.Load<GameObject>("VoicePro")), "VoicePro");
                EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            }

            EditorGUILayout.Space();
            bool showOnStartup = GUILayout.Toggle(_showOnStartup, "Show on startup");

            if (showOnStartup != _showOnStartup)
            {
                _showOnStartup = showOnStartup;

                _params[0] = _showOnStartup.ToString();

                File.WriteAllLines(Application.dataPath + _PathToConfig, _params);
            }

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Networks");

            DrawNetworkingToggle(ref _punNetworkDefineEnabled, ref _params[1], "PUN 2 Network Provider Define");
            DrawNetworkingToggle(ref _mirrorNetworkDefineEnabled, ref _params[2], "Mirror Network Provider Define");
        }

        private void DrawNetworkingToggle(ref bool status, ref string savingParam, string name)
		{
            EditorGUILayout.Space();

            bool currentStatus = GUILayout.Toggle(status, name);

            if (currentStatus != status)
            {
                _punNetworkDefineEnabled = false;
                _mirrorNetworkDefineEnabled = false;
                _params[1] = "false";
                _params[2] = "false";

                status = currentStatus;

                savingParam = status.ToString();

                File.WriteAllLines(Application.dataPath + _PathToConfig, _params);

                HandleDefineSymbols();
            }

        }

        private void HandleDefineSymbols()
		{
            BuildTargetGroup[] group = new BuildTargetGroup[]
            {
                BuildTargetGroup.Standalone,
                BuildTargetGroup.iOS,
                BuildTargetGroup.Android,
                BuildTargetGroup.WebGL,
                BuildTargetGroup.Switch,
                BuildTargetGroup.XboxOne,
                BuildTargetGroup.PS4
            };

            foreach(var item in group)
			{
                string defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(item);

                defines = CheckForDefine(defines, _punNetworkDefineEnabled, _punDefine);
                defines = CheckForDefine(defines, _mirrorNetworkDefineEnabled, _mirrorDefine);

                PlayerSettings.SetScriptingDefineSymbolsForGroup(item, defines);
            }
        }

        private string CheckForDefine(string defines, bool status, string define)
		{
            if (status)
            {
                if (!defines.Contains(define))
                {
                    if (!defines.EndsWith(";"))
                        defines += ";";
                    defines += define + ";";
                }
            }
            else
            {
                if (defines.Contains(define))
                {
                    if (defines.EndsWith(define))
                        defines = defines.Replace(define, string.Empty);
                    else
                        defines = defines.Replace(define + ";", string.Empty);
                }
            }

            return defines;
        }
    }
}