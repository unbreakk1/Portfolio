using UnityEditor;
using UnityEngine;


//TODO editor can start terrain generator
//TODO Button disable if BiomeGenerator is null, helpBox (userfriendly AF)✓

namespace VoxelWorldGen.Editor
{
    public class BiomeDesignerWindow : EditorWindow
    {
        private bool isFolded;
        private bool noiseFoldout;
        private static BiomeDesignerWindow window;
        private readonly Color headerSectionColor = Color.gray;

        private Texture2D desertSectionTexture;
        private Texture2D headerSectionTexture;

        private Rect desertRect;
        private Rect frostyRegionRect;
        private Rect headerRect;
        private Rect jungleRect;
        private Rect windowRect;

        private GUISkin skin;

        private World worldScript;
        private BiomeGenerator biomeGenerator;
        private NoiseSettings noiseSettings;
        
        private static NoiseSettings NoiseSettings { get;  set; }

        private void OnEnable()
        {
            noiseFoldout = isFolded;
            InitTextures();
            InitData();
            skin = Resources.Load<GUISkin>("GuiStyles/BiomeDesignerSkin");
            biomeGenerator = FindObjectOfType<BiomeGenerator>();
            worldScript = FindObjectOfType<World>();
        }

        //gets called on interaction (mouseOver) 
        private void OnGUI()
        {
            DrawLayouts();
            DrawHeader();

        }

        [MenuItem("Window/Biome Designer")]
        private static void OpenWindow()
        {
            window = (BiomeDesignerWindow)GetWindow(typeof(BiomeDesignerWindow));
            window.minSize = new Vector2(800, 600);
            window.Show();
        }

        private static void InitData()
        {
            NoiseSettings = CreateInstance<NoiseSettings>();
        }

        private void InitTextures()
        {
            headerSectionTexture = new Texture2D(1, 1);
            headerSectionTexture.SetPixel(0, 0, headerSectionColor);
            headerSectionTexture.Apply();

            desertSectionTexture = Resources.Load<Texture2D>("Icons/DesertTexture");
        }

        [ExecuteAlways]
        private void DrawLayouts()
        {
            if (!Application.isPlaying) windowRect = window.position;

            DrawLayout(ref headerRect, 0, 0, (int)windowRect.width, 50, headerSectionTexture);
            DrawLayout(ref desertRect, 0, (int)headerRect.y + (int)headerRect.height, (int)windowRect.width,
                (int)windowRect.height - (int)headerRect.height, desertSectionTexture);
            
            DrawSettings(NoiseSettings);
        }

        private void DrawLayout(ref Rect rect, int x, int y, int width, int height, Texture2D texture)
        {
            rect.x = x;
            rect.y = y;
            rect.width = width;
            rect.height = height;

            GUI.DrawTexture(rect, texture);
        }

        private void DrawHeader()
        {
            GUILayout.BeginArea(headerRect);

            GUILayout.Label("Ultimate Biome Designer FREE VERSION v0.69", skin.GetStyle("Header1"));

            GUILayout.EndArea();
        }

        private void DrawSettings(NoiseSettings editorData)
        {
            GUILayout.BeginArea(desertRect);
            
            EditorGUILayout.Space(10);
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("File Name");
            editorData.name = EditorGUILayout.TextField(editorData.name);
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.Space(5);
            EditorGUILayout.BeginVertical(); 
            DrawNoiseSettings(NoiseSettings);
            EditorGUILayout.EndVertical();
            GUILayout.EndArea();
        }

        private void DrawNoiseSettings(NoiseSettings editorData)
        {
            EditorGUILayout.BeginVertical();

            EditorGUILayout.BeginHorizontal();
            
            editorData.amplitude = EditorGUILayout.Slider(new GUIContent("Amplitude",""), editorData.amplitude, 1,16);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(5);
            EditorGUILayout.BeginHorizontal();
            editorData.frequency = EditorGUILayout.Slider(new GUIContent("Frequency", "higher number the smaller the space between Waves"),
                editorData.frequency, 1, 16);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(5);
            EditorGUILayout.BeginHorizontal();
            editorData.NoiseZoom = EditorGUILayout.Slider(new GUIContent("Noise Scale",""), editorData.NoiseZoom, 0.0001f,1f);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(5);
            EditorGUILayout.BeginHorizontal();
            editorData.Exponent = EditorGUILayout.Slider(new GUIContent("Exponent",""), editorData.Exponent, 1,16);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(5);
            EditorGUILayout.BeginHorizontal();
            editorData.Persistance = EditorGUILayout.Slider(new GUIContent("Persistance",""), editorData.Persistance, 1.0f,2f);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(5);
            EditorGUILayout.BeginHorizontal();
            editorData.Octaves = (int)EditorGUILayout.Slider(new GUIContent("Octaves",""), editorData.Octaves, 1,16);
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.Space(5);
            EditorGUILayout.BeginHorizontal();
            editorData.RedistributionModifier = EditorGUILayout.Slider(new GUIContent("Redistribution Modifier",""), editorData.RedistributionModifier, 1.0f,2.0f);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(5);
            EditorGUILayout.BeginHorizontal();
            editorData.Offset = EditorGUILayout.Vector2IntField("Noise Offset",editorData.Offset);
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.Space(5);
            EditorGUILayout.BeginHorizontal();
            editorData.WorldOffset = EditorGUILayout.Vector2IntField("World Seed/Offset",editorData.WorldOffset);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        #region BUTTONS    
            EditorGUILayout.Space(5);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Enter PlayMode")) EditorApplication.EnterPlaymode();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            if (biomeGenerator != null)
            {
                if (GUILayout.Button("OVERRIDE EXISTING Noise Settings"))
                {
                    NoiseSettings tempSettings =
                        (NoiseSettings)AssetDatabase.LoadAssetAtPath("Assets/VoxelWorldGen/Scripts/NoiseSO/DesertBiomeNoise.asset",
                            typeof(ScriptableObject));

                    NoiseSettings.amplitude = tempSettings.amplitude;
                    NoiseSettings.frequency = tempSettings.frequency;
                    NoiseSettings.RedistributionModifier = tempSettings.RedistributionModifier;
                    NoiseSettings.Exponent = tempSettings.Exponent;
                    NoiseSettings.NoiseZoom = tempSettings.NoiseZoom;
                    NoiseSettings.Offset = tempSettings.Offset;
                    NoiseSettings.WorldOffset = tempSettings.WorldOffset;
                    NoiseSettings.Octaves = tempSettings.Octaves;
                    NoiseSettings.Persistance = tempSettings.Persistance;
                }
            }
            else
            {
                EditorGUILayout.HelpBox("Something went wrong. Please open the window again.", MessageType.Error);
            }
            
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Save As Scriptable Object")) SaveData(NoiseSettings);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            if (Application.isPlaying)
            {
                if (GUILayout.Button("Start Generation")) worldScript.GenerateWorld();
            }
            else
                EditorGUILayout.HelpBox("Please Enter Playmode", MessageType.Warning);
            EditorGUILayout.EndHorizontal();
#endregion
            EditorGUILayout.EndFoldoutHeaderGroup();
        }

        private void SaveData(NoiseSettings editorData)
        {
            
            string path = AssetDatabase.GenerateUniqueAssetPath("Assets/VoxelWorldGen/Scripts/NoiseSO/SavedSO/" + editorData.name + ".asset");
            AssetDatabase.CreateAsset(NoiseSettings, path);
            Debug.Log("saved to: " + path);
        }
    }
}