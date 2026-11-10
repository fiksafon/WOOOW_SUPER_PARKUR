#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class SpriteAutoSlicer : EditorWindow
{
    private int cellWidth = 32;
    private int cellHeight = 32;

    private DefaultAsset targetFolder;

    [MenuItem("Tools/Auto Sprite Slicer (Massive)")]
    public static void ShowWindow()
    {
        GetWindow<SpriteAutoSlicer>("Massive Sprite Slicer");
    }

    private void OnGUI()
    {
        GUILayout.Label("Массовый авто-нарезчик спрайтов по сетке", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        targetFolder = (DefaultAsset)EditorGUILayout.ObjectField("Папка с текстурами", targetFolder, typeof(DefaultAsset), false);

        EditorGUILayout.Space(10);
        GUILayout.Label("Размер одного спрайта (в пикселях):", EditorStyles.helpBox);
        cellWidth = EditorGUILayout.IntField("Ширина (Width)", cellWidth);
        cellHeight = EditorGUILayout.IntField("Высота (Height)", cellHeight);

        EditorGUILayout.Space(20);

        if (GUILayout.Button("Нарезать ВСЕ спрайт-листы в папке", GUILayout.Height(35)))
        {
            SliceAllTexturesInFolder();
        }
    }

   private void SliceAllTexturesInFolder()
    {
        if (targetFolder == null)
        {
            EditorUtility.DisplayDialog("Ошибка", "Сначала выберите папку с текстурами!", "ОК");
            return;
        }

        string folderPath = AssetDatabase.GetAssetPath(targetFolder);
        
        if (!Directory.Exists(folderPath))
        {
            EditorUtility.DisplayDialog("Ошибка", "Выбранный объект не является папкой.", "ОК");
            return;
        }

        string[] guids = AssetDatabase.FindAssets("t:Texture2D", new[] { folderPath });

        if (guids.Length == 0)
        {
            EditorUtility.DisplayDialog("Внимание", "В выбранной папке не найдено текстур!", "ОК");
            return;
        }

        int processedCount = 0;

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(path);

            if (texture == null) continue;

            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
            if (importer == null) continue;

            // Обязательно разрешаем чтение пикселей
            importer.isReadable = true;
            importer.textureType = TextureImporterType.Sprite;
            importer.spriteImportMode = SpriteImportMode.Multiple;
            importer.spritePixelsPerUnit = cellWidth;
            importer.filterMode = FilterMode.Point;

            // Сбрасываем старые метаданные
            importer.spritesheet = new SpriteMetaData[0];
            EditorUtility.SetDirty(importer);
            importer.SaveAndReimport();

            int textureWidth = texture.width;
            int textureHeight = texture.height;

            List<SpriteMetaData> metas = new List<SpriteMetaData>();
            int frameIndex = 0;

            // Идем сверху вниз, слева направо
            for (int y = textureHeight - cellHeight; y >= 0; y -= cellHeight)
            {
                for (int x = 0; x < textureWidth; x += cellWidth)
                {
                    SpriteMetaData meta = new SpriteMetaData();

                    // Проверяем ячейку на пустоту (прозрачность)
                    if (IsCellEmpty(texture, x, y, cellWidth, cellHeight))
                    {
                        // Пустая серая зона справа получает метку _EMPTY_ и будет проигнорирована генератором
                        // meta.name = $"{texture.name}_EMPTY_{frameIndex}";
                        continue;
                    }
                    else
                    {
                        meta.name = $"{texture.name}_{frameIndex}";
                    }

                    meta.rect = new Rect(x, y, cellWidth, cellHeight);
                    meta.alignment = (int)SpriteAlignment.Center;

                    metas.Add(meta);
                    frameIndex++;
                }
            }

            importer.spritesheet = metas.ToArray();
            
            EditorUtility.SetDirty(importer);
            importer.SaveAndReimport();

            processedCount++;
            Debug.Log($"Нарезан файл: {texture.name} (Кадров: {metas.Count})");
        }

        EditorUtility.DisplayDialog("Успех!", $"Массовая нарезка завершена!\nОбработано файлов: {processedCount}", "ОК");
    }

    private bool IsCellEmpty(Texture2D texture, int startX, int startY, int width, int height)
    {
        try
        {
            Color[] pixels = texture.GetPixels(startX, startY, width, height);
            foreach (Color col in pixels)
            {
                // Если найден хоть один непрозрачный пиксель — ячейка с персонажем
                if (col.a > 0.01f) return false;
            }
        }
        catch
        {
            return false;
        }
        // Все пиксели прозрачные — это пустой хвост справа
        return true;
    }
}
#endif