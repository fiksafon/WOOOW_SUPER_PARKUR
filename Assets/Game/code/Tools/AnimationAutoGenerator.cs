#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class AnimationAutoGenerator : EditorWindow
{
    private Texture2D spriteSheet;
    private string targetChildName = "Outfit"; // Куда привязываем: Body, Hair, Outfit или Legs
    private string animationName = "Idle";
    private int frameRate = 12;

    [MenuItem("Tools/Auto Animation Generator")]
    public static void ShowWindow()
    {
        GetWindow<AnimationAutoGenerator>("Auto Animator");
    }

    private void OnGUI()
    {
        GUILayout.Label("Авто-генератор клипов анимации", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        spriteSheet = (Texture2D)EditorGUILayout.ObjectField("Спрайт-лист (Texture2D)", spriteSheet, typeof(Texture2D), false);
        targetChildName = EditorGUILayout.TextField("Имя слоя (Body/Hair/Outfit/Legs)", targetChildName);
        animationName = EditorGUILayout.TextField("Название анимации (Idle/Walk)", animationName);
        frameRate = EditorGUILayout.IntField("Frame Rate (кадров/сек)", frameRate);

        EditorGUILayout.Space(15);

        if (GUILayout.Button("Сгенерировать .anim клип", GUILayout.Height(35)))
        {
            GenerateAnimationClip();
        }
    }

    private void GenerateAnimationClip()
    {
        if (spriteSheet == null)
        {
            EditorUtility.DisplayDialog("Ошибка", "Перетащите спрайт-лист!", "ОК");
            return;
        }

        string path = AssetDatabase.GetAssetPath(spriteSheet);
        Object[] assets = AssetDatabase.LoadAllAssetsAtPath(path);
        
        // Получаем все спрайты из файла и сортируем их строго по порядку нарезки
        List<Sprite> sprites = assets.OfType<Sprite>().ToList();
        sprites.Sort((a, b) => {
            int res = b.rect.y.CompareTo(a.rect.y);
            if (res == 0) res = a.rect.x.CompareTo(b.rect.x);
            return res;
        });

        if (sprites.Count == 0)
        {
            EditorUtility.DisplayDialog("Ошибка", "В этом файле нет нарезанных спрайтов! Нарежьте его через Sprite Editor.", "ОК");
            return;
        }

        // Создаем новый анимационный клип
        AnimationClip clip = new AnimationClip();
        clip.frameRate = frameRate;

        // Создаем привязку (Binding) к SpriteRenderer дочернего объекта
        EditorCurveBinding binding = new EditorCurveBinding();
        binding.type = typeof(SpriteRenderer);
        binding.path = targetChildName; // Путь к объекту (например, "Outfit")
        binding.propertyName = "m_Sprite";

        // Создаем ключевые кадры для клипа
        ObjectReferenceKeyframe[] keyframes = new ObjectReferenceKeyframe[sprites.Count];
        float timePerFrame = 1f / frameRate;

        for (int i = 0; i < sprites.Count; i++)
        {
            keyframes[i] = new ObjectReferenceKeyframe
            {
                time = i * timePerFrame,
                value = sprites[i]
            };
        }

        // Записываем ключи в анимационный клип
        AnimationUtility.SetObjectReferenceCurve(clip, binding, keyframes);

        // Сохраняем клип в проект
        string savePath = EditorUtility.SaveFilePanelInProject("Сохранить клип", $"{spriteSheet.name}_{animationName}", "anim", "Выберите папку для сохранения");
        if (!string.IsNullOrEmpty(savePath))
        {
            AssetDatabase.CreateAsset(clip, savePath);
            AssetDatabase.SaveAssets();
            EditorUtility.DisplayDialog("Успех!", $"Клип анимации создан:\n{savePath}", "ОК");
        }
    }
}
#endif