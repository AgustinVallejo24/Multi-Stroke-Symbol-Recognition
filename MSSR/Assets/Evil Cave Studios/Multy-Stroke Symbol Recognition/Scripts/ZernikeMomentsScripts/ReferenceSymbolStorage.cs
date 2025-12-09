using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEditor;


namespace SymbolRecognizer
{


public static class ReferenceSymbolStorage
{
    [Serializable]
    private class ReferenceSymbolWrapper
    {
        public List<ReferenceSymbolGroup> symbols;
    }
    private static readonly string DEFAULT_JSON_CONTENT = JsonUtility.ToJson(new ReferenceSymbolWrapper { symbols = new List<ReferenceSymbolGroup>() }, true);
    
    private static readonly string FILENAME = "symbols.json";
    private const string FILENAME_NO_EXTENSION = "symbols";

    [DllImport("__Internal")]
    private static extern void SyncFilesystem();

    private static string GetFilePath()
    {
        string directoryPath = Application.dataPath + "/Resources";
        try { Directory.CreateDirectory(directoryPath); }
        catch (IOException) { /* Ignore if already exists */ }

        return Path.Combine(directoryPath, FILENAME);
    }


    public static void SaveSymbols(List<ReferenceSymbolGroup> symbols)
    {
        string filePath = GetFilePath();

        ReferenceSymbolWrapper wrapper = new ReferenceSymbolWrapper { symbols = symbols };
        string json = JsonUtility.ToJson(wrapper, true);

        File.WriteAllText(filePath, json);


#if UNITY_EDITOR
        AssetDatabase.Refresh();
#endif

    }


    public static List<ReferenceSymbolGroup> LoadSymbols()
    {
        TextAsset textAsset = Resources.Load<TextAsset>(FILENAME_NO_EXTENSION);

        if (textAsset == null)
        {
            Debug.LogError($"No se encontró el archivo {FILENAME} en Resources.");
            return new List<ReferenceSymbolGroup>();
        }

        string json = textAsset.text;

        if (string.IsNullOrEmpty(json))
        {
            Debug.LogError("El archivo JSON está vacío.");
            return new List<ReferenceSymbolGroup>();
        }

        try
        {
            ReferenceSymbolWrapper wrapper = JsonUtility.FromJson<ReferenceSymbolWrapper>(json);
            return wrapper?.symbols ?? new List<ReferenceSymbolGroup>();
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error al deserializar JSON: {ex.Message}");
            return new List<ReferenceSymbolGroup>();
        }
    }


    public static void AppendSymbol(ReferenceSymbol newSymbol)
    {

        List<ReferenceSymbolGroup> current = LoadSymbols();

        ReferenceSymbolGroup existingGroup = current.FirstOrDefault(x => string.Equals(x.symbolName, newSymbol.symbolName, StringComparison.OrdinalIgnoreCase));

        if (existingGroup.symbolName != null)
        {
            existingGroup.symbols.Add(newSymbol);
        }
        else
        {
            ReferenceSymbolGroup newGroup = new ReferenceSymbolGroup
            {
                symbolName = newSymbol.symbolName,
                symbols = new List<ReferenceSymbol> { newSymbol }
            };
            current.Add(newGroup);
        }

        SaveSymbols(current);
        Debug.Log("Appended symbol: " + newSymbol.symbolName);
    }

    public static bool JsonExistsInResources()
    {
        TextAsset asset = Resources.Load<TextAsset>(FILENAME_NO_EXTENSION);
        return asset != null;
    }

}
}