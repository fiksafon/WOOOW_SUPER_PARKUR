#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor.Animations;

public class NPCGeneratorWindow : EditorWindow
{
    private GameObject basePrefab;
    private GameObject previewInstance;

    // Папки, откуда автоматически подтягиваются спрайт-листы
    private DefaultAsset bodyFolder;
    private DefaultAsset hairFolder;
    private DefaultAsset outfitFolder;
    private DefaultAsset legsFolder;

    // Списки названий для выпадающих меню
    private string[] bodyOptions = new string[] { "None" };
    private int selectedBodyIndex = 0;

    private string[] hairOptions = new string[] { "None" };
    private int selectedHairIndex = 0;

    private string[] outfitOptions = new string[] { "None" };
    private int selectedOutfitIndex = 0;

    private string[] legsOptions = new string[] { "None" };
    private int selectedLegsIndex = 0;

    // Словари для хранения связи: [Имя файла] -> [Нулевой спрайт]
    private Dictionary<string, Sprite> bodySprites = new Dictionary<string, Sprite>();
    private Dictionary<string, Sprite> hairSprites = new Dictionary<string, Sprite>();
    private Dictionary<string, Sprite> outfitSprites = new Dictionary<string, Sprite>();
    private Dictionary<string, Sprite> legsSprites = new Dictionary<string, Sprite>();

    [MenuItem("Tools/NPC Generator Studio (Auto-Scan)")]
    public static void ShowWindow()
    {
        GetWindow<NPCGeneratorWindow>("NPC Generator Studio");
    }

    private void OnGUI()
    {
        GUILayout.Label("Умный генератор NPC (Авто-сканирование папок)", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        basePrefab = (GameObject)EditorGUILayout.ObjectField("Базовый префаб NPC", basePrefab, typeof(GameObject), false);

        EditorGUILayout.Space(10);
        GUILayout.Label("Папки с частями персонажа (скрипт сам найдет спрайт-листы и их 0-е кадры):", EditorStyles.helpBox);

        // Поле выбора папок
        EditorGUI.BeginChangeCheck();
        bodyFolder = (DefaultAsset)EditorGUILayout.ObjectField("Папка Тела (Body)", bodyFolder, typeof(DefaultAsset), false);
        hairFolder = (DefaultAsset)EditorGUILayout.ObjectField("Папка Волос (Hair)", hairFolder, typeof(DefaultAsset), false);
        outfitFolder = (DefaultAsset)EditorGUILayout.ObjectField("Папка Одежды (Outfit)", outfitFolder, typeof(DefaultAsset), false);
        legsFolder = (DefaultAsset)EditorGUILayout.ObjectField("Папка Ног (Legs)", legsFolder, typeof(DefaultAsset), false);

        // Если папки изменились — автоматически пересканируем их содержимое
        if (EditorGUI.EndChangeCheck())
        {
            RefreshAllLists();
        }

        EditorGUILayout.Space(10);
        GUILayout.Label("Выпадающие списки выбора:", EditorStyles.boldLabel);

        selectedBodyIndex = EditorGUILayout.Popup("Тело", selectedBodyIndex, bodyOptions);
        selectedHairIndex = EditorGUILayout.Popup("Волосы", selectedHairIndex, hairOptions);
        selectedOutfitIndex = EditorGUILayout.Popup("Одежда", selectedOutfitIndex, outfitOptions);
        selectedLegsIndex = EditorGUILayout.Popup("Ноги", selectedLegsIndex, legsOptions);

        EditorGUILayout.Space(20);

        if (GUILayout.Button("Обновить превью", GUILayout.Height(35)))
        {
            UpdatePreview();
        }

        EditorGUILayout.Space(10);

        GUI.backgroundColor = Color.green;
        if (GUILayout.Button("Экспортировать в Prefab", GUILayout.Height(30)))
        {
            ExportPrefab();
        }
        GUI.backgroundColor = Color.white;
    }

    private void RefreshAllLists()
    {
        bodyOptions = LoadFolderToDictionary(bodyFolder, out bodySprites);
        hairOptions = LoadFolderToDictionary(hairFolder, out hairSprites);
        outfitOptions = LoadFolderToDictionary(outfitFolder, out outfitSprites);
        legsOptions = LoadFolderToDictionary(legsFolder, out legsSprites);

        selectedBodyIndex = 0;
        selectedHairIndex = 0;
        selectedOutfitIndex = 0;
        selectedLegsIndex = 0;
    }

    private string[] LoadFolderToDictionary(DefaultAsset folder, out Dictionary<string, Sprite> dict)
    {
        dict = new Dictionary<string, Sprite>();
        List<string> options = new List<string> { "None" };

        if (folder == null) return options.ToArray();

        string path = AssetDatabase.GetAssetPath(folder);
        if (!Directory.Exists(path)) return options.ToArray();

        string[] guids = AssetDatabase.FindAssets("t:Texture2D", new[] { path });

        foreach (string guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath);
            if (texture == null) continue;

            Object[] subAssets = AssetDatabase.LoadAllAssetsAtPath(assetPath);
            List<Sprite> sheetSprites = new List<Sprite>();

            foreach (Object sub in subAssets)
            {
                if (sub is Sprite sprite)
                {
                    sheetSprites.Add(sprite);
                }
            }

            if (sheetSprites.Count > 0)
            {
                sheetSprites.Sort((a, b) => {
                    int res = b.rect.y.CompareTo(a.rect.y);
                    if (res == 0)
                        res = a.rect.x.CompareTo(b.rect.x);
                    return res;
                });

                Sprite trueFirstSprite = sheetSprites[0];
                string sheetName = texture.name;

                if (!dict.ContainsKey(sheetName))
                {
                    dict.Add(sheetName, trueFirstSprite);
                    options.Add(sheetName);
                }
            }
        }

        return options.ToArray();
    }

    private void UpdatePreview()
    {
        if (basePrefab == null)
        {
            EditorUtility.DisplayDialog("Ошибка", "Выберите базовый префаб NPC!", "ОК");
            return;
        }

        if (previewInstance != null)
        {
            DestroyImmediate(previewInstance);
        }

        previewInstance = (GameObject)PrefabUtility.InstantiatePrefab(basePrefab);
        previewInstance.name = "NPC_Preview_Active";

        CleanChildAnimators(previewInstance);

        ApplySpriteFromDict(previewInstance, "Body", bodyOptions[selectedBodyIndex], bodySprites);
        ApplySpriteFromDict(previewInstance, "Hair", hairOptions[selectedHairIndex], hairSprites);
        ApplySpriteFromDict(previewInstance, "Outfit", outfitOptions[selectedOutfitIndex], outfitSprites);
        ApplySpriteFromDict(previewInstance, "Legs", legsOptions[selectedLegsIndex], legsSprites);

        Selection.activeGameObject = previewInstance;
        if (SceneView.lastActiveSceneView != null)
        {
            SceneView.lastActiveSceneView.FrameSelected();
        }
    }

    private void CleanChildAnimators(GameObject root)
    {
        foreach (Transform child in root.transform)
        {
            Animator anim = child.GetComponent<Animator>();
            if (anim != null)
            {
                DestroyImmediate(anim);
            }
        }
    }

    private void ApplySpriteFromDict(GameObject parent, string childName, string key, Dictionary<string, Sprite> dict)
    {
        Transform childTransform = parent.transform.Find(childName);
        if (childTransform == null) return;

        SpriteRenderer sr = childTransform.GetComponent<SpriteRenderer>();
        if (sr == null) return;

        switch (childName)
        {
            case "Body": sr.sortingOrder = 0; break;
            case "Legs": sr.sortingOrder = 1; break;
            case "Outfit": sr.sortingOrder = 2; break;
            case "Hair": sr.sortingOrder = 3; break;
        }

        if (key == "None")
        {
            sr.sprite = null;
            return;
        }

        if (dict != null && dict.ContainsKey(key))
        {
            sr.sprite = dict[key];
        }
        else
        {
            string[] guids = AssetDatabase.FindAssets($"{key} t:Sprite");
            if (guids.Length > 0)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                sr.sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
            }
        }
    }

   // Структура для описания каждой анимации
    private struct AnimConfig
    {
        public string name;
        public int frameCount;

        public AnimConfig(string name, int frameCount)
        {
            this.name = name;
            this.frameCount = frameCount;
        }
    }

    private void ExportPrefab()
    {
        if (previewInstance == null)
        {
            EditorUtility.DisplayDialog("Ошибка", "Сначала создайте превью!", "ОК");
            return;
        }

        string folderPath = EditorUtility.SaveFolderPanel("Выберите папку для сохранения NPC", "Assets", "");
        if (string.IsNullOrEmpty(folderPath)) return;

        string relativePath = "";
        if (folderPath.StartsWith(Application.dataPath))
        {
            relativePath = "Assets" + folderPath.Substring(Application.dataPath.Length);
        }
        else
        {
            EditorUtility.DisplayDialog("Ошибка", "Пожалуйста, выберите папку внутри папки Assets вашего проекта!", "ОК");
            return;
        }

        string npcName = previewInstance.name;

        try
        {
            // Укажи здесь точное название и количество кадров для КАЖДОЙ из своих 7 анимаций (по порядку сверху вниз в спрайт-листе)
            AnimConfig[] animations = new AnimConfig[]
            {
                new AnimConfig("Idle", 5),       
                new AnimConfig("Walk", 8),       
                new AnimConfig("Run", 8),        
                new AnimConfig("Jump", 5),     
                new AnimConfig("Fall", 3),       
                new AnimConfig("Attack", 6),     
                new AnimConfig("Death", 10)        
            };

            int frameRate = 12;
            AnimatorController animController = null;
            Dictionary<string, AnimationClip> createdClips = new Dictionary<string, AnimationClip>();

            // Собираем спрайты с персонажа
           // Собираем спрайты с персонажа
            foreach (Transform child in previewInstance.transform)
            {
                SpriteRenderer sr = child.GetComponent<SpriteRenderer>();
                if (sr == null || sr.sprite == null) continue;

                string spritePath = AssetDatabase.GetAssetPath(sr.sprite);
                Object[] assets = AssetDatabase.LoadAllAssetsAtPath(spritePath);
                List<Sprite> sheetSprites = assets.OfType<Sprite>().ToList();

                // === ДОБАВЛЯЕМ ЭТУ СТРОКУ: Игнорируем пустые ячейки-разделители ===
                sheetSprites = sheetSprites.Where(s => s != null && !s.name.Contains("_EMPTY_")).ToList();

                // Сортируем спрайты по сетке: сверху вниз (Y), слева направо (X)
                sheetSprites.Sort((a, b) => {
                    int res = b.rect.y.CompareTo(a.rect.y);
                    if (res == 0) res = a.rect.x.CompareTo(b.rect.x);
                    return res;
                });

                if (sheetSprites.Count == 0) continue;

                int currentIndex = 0;

                // Нарезаем спрайты поочередно для каждой анимации согласно её количеству кадров
                foreach (var animConfig in animations)
                {
                    if (currentIndex >= sheetSprites.Count) break;

                    int countToTake = Mathf.Min(animConfig.frameCount, sheetSprites.Count - currentIndex);
                    List<Sprite> rowSprites = sheetSprites.GetRange(currentIndex, countToTake);
                    currentIndex += countToTake; // Сдвигаем указатель на следующий ряд/анимацию

                    string clipName = animConfig.name;

                    if (!createdClips.ContainsKey(clipName))
                    {
                        AnimationClip clip = new AnimationClip();
                        clip.frameRate = frameRate;

                        // Включаем зацикливание для Idle, Walk, Run
                        if (clipName == "Idle" || clipName == "Walk" || clipName == "Run")
                        {
                            var settings = AnimationUtility.GetAnimationClipSettings(clip);
                            settings.loopTime = true;
                            AnimationUtility.SetAnimationClipSettings(clip, settings);
                        }

                        createdClips.Add(clipName, clip);
                    }

                    AnimationClip currentClip = createdClips[clipName];

                    EditorCurveBinding binding = new EditorCurveBinding
                    {
                        type = typeof(SpriteRenderer),
                        path = child.name,
                        propertyName = "m_Sprite"
                    };

                    ObjectReferenceKeyframe[] keyframes = new ObjectReferenceKeyframe[rowSprites.Count];
                    float timePerFrame = 1f / frameRate;

                    for (int i = 0; i < rowSprites.Count; i++)
                    {
                        keyframes[i] = new ObjectReferenceKeyframe
                        {
                            time = i * timePerFrame,
                            value = rowSprites[i]
                        };
                    }

                    AnimationUtility.SetObjectReferenceCurve(currentClip, binding, keyframes);
                }
                sheetSprites.Sort((a, b) => {
                            // Округляем Y до ближайших 16 или 32 пикселей (высота твоего спрайта), 
                        // чтобы спрайты одной строки считались находящимися на одной высоте
                           int rowA = Mathf.RoundToInt(-a.rect.y / 32f); // Замени 32 на высоту твоего спрайта в пикселях
                           int rowB = Mathf.RoundToInt(-b.rect.y / 32f);

                           if (rowA != rowB)
                            {
                                  return rowA.CompareTo(rowB); // Сначала сверху вниз по рядам
                            }
                             return a.rect.x.CompareTo(b.rect.x); // Внутри ряда — строго слева направо
                });
                
            }

            // Создаем Animator Controller и сохраняем клипы
            string controllerPath = Path.Combine(relativePath, $"{npcName}_Controller.controller").Replace("\\", "/");
            animController = AnimatorController.CreateAnimatorControllerAtPath(controllerPath);
            var rootStateMachine = animController.layers[0].stateMachine;

            bool isFirst = true;
            foreach (var pair in createdClips)
            {
                string clipName = pair.Key;
                AnimationClip clip = pair.Value;

                string clipPath = Path.Combine(relativePath, $"{npcName}_{clipName}.anim").Replace("\\", "/");
                AssetDatabase.CreateAsset(clip, clipPath);

                var state = rootStateMachine.AddState(clipName);
                state.motion = clip;

                if (isFirst)
                {
                    rootStateMachine.defaultState = state;
                    isFirst = false;
                }
                
            }

            // Привязываем контроллер к превью
            Animator mainAnimator = previewInstance.GetComponent<Animator>();
            if (mainAnimator == null)
            {
                mainAnimator = previewInstance.AddComponent<Animator>();
            }
            mainAnimator.runtimeAnimatorController = animController;

            // Сохраняем префаб
            string prefabPath = Path.Combine(relativePath, $"{npcName}.prefab").Replace("\\", "/");
            PrefabUtility.SaveAsPrefabAsset(previewInstance, prefabPath);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            EditorUtility.DisplayDialog("Успех!", $"Успешно создано клипов: {createdClips.Count}\nСохранено в:\n{relativePath}", "OK");
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Ошибка при экспорте префаба: " + ex.Message);
        }
    }

    private void OnDisable()
    {
        if (previewInstance != null)
        {
            DestroyImmediate(previewInstance);
        }
    }
}
#endif