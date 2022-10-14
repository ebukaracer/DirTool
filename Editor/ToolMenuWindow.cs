using UnityEditor;
using UnityEngine;

namespace Racer.DirTool
{
    internal class ToolMenuWindow : EditorWindow
    {
        private const string RootDirKey = "TMW_rd";
        private const string SubDirKey = "TMW_sd";
        private const string DefaultRootDir = "_Project";
        private const string DefaultSubDir = "_Scripts, Materials, Models, Animations, Prefabs";
        private const int Width = 800;
        private const int Divisor = 4;

        private static bool _shouldRetain;
        private static string _projectRootDir;
        private static string _subDirNames;



        [MenuItem("Dir_Tool/Setup Window")]
        private static void DisplayWindow()
        {
            _projectRootDir = EditorPrefs.HasKey(RootDirKey) ? EditorPrefs.GetString(RootDirKey) : DefaultRootDir;
            _subDirNames = EditorPrefs.HasKey(SubDirKey) ? EditorPrefs.GetString(SubDirKey) : DefaultSubDir;

            var window = GetWindow<ToolMenuWindow>();
            window.titleContent = new GUIContent("Tool Menu Window");

            // Limit size of the window, non re-sizable
            window.minSize = new Vector2(Width, Width / Divisor);
            window.maxSize = new Vector2(Width, Width / Divisor);
        }

        private void OnGUI()
        {
#if UNITY_2019_1_OR_NEWER
            EditorGUILayout.Space(10);
#else
            EditorGUILayout.Space();
#endif
            EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(Width));
            EditorGUILayout.HelpBox("Input multiple sub-directories(names) by separating with comma(,)", MessageType.Info);
            EditorGUILayout.EndHorizontal();

            GUILayout.Label("Directory Settings", EditorStyles.boldLabel);

            _projectRootDir = EditorGUILayout.TextField("Root Directory", _projectRootDir);

            EditorGUILayout.BeginHorizontal(GUILayout.Width(Width - (Divisor - 1)));
            _subDirNames = EditorGUILayout.TextField("Sub-Directories", _subDirNames);
            if (GUILayout.Button(new GUIContent("Create", "Creates a directory from the [Root Directory] specified."),
                GUILayout.MaxWidth(150)))
            {
                DirectoryManager.CreateDirectory(rootDir: _projectRootDir, subDir: _subDirNames);

                AssetDatabase.Refresh();
            }

            if (GUILayout.Button(new GUIContent("Delete", "Deletes a directory from the [Root Directory] specified."),
                GUILayout.MaxWidth(150)))
            {
                DirectoryManager.DeleteDirectory(rootDir: _projectRootDir, subDir: _subDirNames);

                AssetDatabase.Refresh();
            }
            EditorGUILayout.EndHorizontal();

#if UNITY_2019_1_OR_NEWER
            EditorGUILayout.Space(10);
#else
            EditorGUILayout.Space();
#endif
            EditorGUILayout.HelpBox("Whether or not to preserve the Inputted Values every time this Window is opened, default values are used if unchecked.", MessageType.Info);
            _shouldRetain = EditorGUILayout.Toggle("Retain Input", _shouldRetain, GUILayout.MaxWidth(Width / Divisor));
        }

        public static string GetRootDir() => _projectRootDir;
        public static string GetSubDir() => _subDirNames;

        private void OnDestroy()
        {
            if (_shouldRetain)
            {
                EditorPrefs.SetString(RootDirKey, _projectRootDir);
                EditorPrefs.SetString(SubDirKey, _subDirNames);
            }
            else
            {
                if (!string.IsNullOrEmpty(_projectRootDir))
                    EditorPrefs.DeleteKey(RootDirKey);

                if (!string.IsNullOrEmpty(_subDirNames))
                    EditorPrefs.DeleteKey(SubDirKey);
            }
        }
    }
}