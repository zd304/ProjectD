using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

public static class ExcelLoader
{
    public static readonly char[] Separators = new char[] { '\t' };
    public static readonly string ExcelRootDirectory = /*Application.dataPath*/"Assets/Resources/Excels/";
    public static Encoding Encoding = null;
    private static bool initialized = false;

    public static void Initialize()
    {

    }

    public static void Unintialize()
    {

    }

    // 加载所有表格;
    public static bool LoadAll()
    {
        if (Encoding == null)
        {
            Encoding = Encoding.Unicode; // default encoding
        }

        if (initialized)
        {
            return false;
        }

        ExcelFactory.InitializeFactory();

        string filePath = ExcelRootDirectory + "index.txt";

        TextAsset textAsset = Resources.Load<TextAsset>(filePath);
        if (textAsset == null)
        {
            Debug.LogError("index.txt加载失败");
            return false;
        }
        string indexContent = textAsset.text;

        if (string.IsNullOrEmpty(indexContent))
        {
            Debug.LogError("Index.txt表为空");
            return false;
        }

        ParseIndex(indexContent);

        ExcelFactory.UninitializeFactory();

        initialized = true;

        return true;
    }

    // 读取表index.txt，加载表里面所有C#相关的表格;
    private static void ParseIndex(string indexContent)
    {
        using (StringReader reader = new StringReader(indexContent))
        {
            // 表头;
            string line = reader.ReadLine();

            while (!string.IsNullOrEmpty(line))
            {
                line = reader.ReadLine();
                if (!string.IsNullOrEmpty(line))
                {
                    string[] strDatas = line.Split(Separators, StringSplitOptions.None); // 不移除空格子

                    if (strDatas.Length < 3)
                    {
                        continue;
                    }

                    // 不是C#需要加载的表格就不用加载;
                    string pot = strDatas[2].ToLower();
                    if (!pot.Contains("c"))
                    {
                        continue;
                    }

                    string className = strDatas[1];
                    className = Path.GetFileNameWithoutExtension(className);

                    ParseConfig(className);
                }
            }
        }
    }

    // 加载类名相关的所有表格;
    public static bool ParseConfig(string className)
    {
        string[] files = ExcelFactory.GetFiles(className);

        ExcelFactory.ClearExcelFunc clearFunc = null;
        if (ExcelFactory.ClearFunctions.TryGetValue(className, out clearFunc))
        {
            clearFunc();
        }

        for (int i = 0; i < files.Length; ++i)
        {
            string fileName = ExcelRootDirectory + files[i] + ".txt";

            LoadExcel(className, fileName);
        }

        return true;
    }

    // 加载指定路径的表格;
    public static void LoadExcel(string className, string fileName)
    {
#if UNITY_EDITOR
        TextAsset textAsset = UnityEditor.AssetDatabase.LoadAssetAtPath<TextAsset>(fileName);
#else
        TextAsset textAsset = Resources.Load<TextAsset>(fileName);
#endif
        if (textAsset == null)
        {
            Debug.LogError(fileName + "加载失败");
            return;
        }

        string excelContent = textAsset.text;
        if (string.IsNullOrEmpty(excelContent))
        {
            //Debug.Log("loadfile=" + fileName);
            using (StringReader reader = new StringReader(excelContent))
            {
                // 表头;
                string lineData = reader.ReadLine();

                while (!string.IsNullOrEmpty(lineData))
                {
                    lineData = reader.ReadLine();
                    if (!string.IsNullOrEmpty(lineData))
                    {
                        string[] datas = lineData.Split(Separators, StringSplitOptions.None); // 不移除空格子

                        var line = ExcelFactory.InitExcelLine(className, datas);
                    }
                }
            }
        }
    }
}