using UnityEditor;
using UnityEngine;
using LitJson;
using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class ExcelLoaderEditor
{
    public static readonly string DynamicFactoryCodePath = Application.dataPath + "/Scripts/Core/Excel/ExcelFactory_Dynamic.cs";
    public static readonly string ExcelDefineCodePath = Application.dataPath + "/Scripts/Core/Excel/ExcelDefine.cs";

    [MenuItem("Tools/Excel/生成表格代码")]
    public static void GenExcelCode()
    {
        if (ExcelLoader.Encoding == null)
        {
            ExcelLoader.Encoding = Encoding.Unicode; // default encoding
        }
        string filePath = ExcelLoader.ExcelRootDirectory + "index.txt";
        using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        {
            StreamReader oReader = new StreamReader(fileStream, ExcelLoader.Encoding);
            string indexContent = oReader.ReadToEnd();

            LoadExcelConfig(indexContent);
        }
    }

    static void LoadExcelConfig(string indexContent)
    {
        List<string> initTexts = new List<string>();
        List<string> filesTexts = new List<string>();
        List<string> regTexts = new List<string>();
        List<string> defTexts = new List<string>();
        List<string> toStrTexts = new List<string>();
        List<string> clearTexts = new List<string>();
        List<string> regClearTexts = new List<string>();

        using (StringReader reader = new StringReader(indexContent))
        {
            // 表头;
            string line = reader.ReadLine();

            while (!string.IsNullOrEmpty(line))
            {
                line = reader.ReadLine();
                if (!string.IsNullOrEmpty(line))
                {
                    string[] strDatas = line.Split(ExcelLoader.Separators, StringSplitOptions.None); // 不移除空格子

                    // 不是C#需要加载的表格就不用生成了;
                    string pot = strDatas[2].ToLower();
                    if (!pot.Contains("c"))
                    {
                        continue;
                    }

                    string configPath = strDatas[1];

                    string initTxt;
                    string filesTxt;
                    string regTxt;
                    string defTxt;
                    string toStrTxt;
                    string clearTxt;
                    string regClearTxt;
                    ParseConfig(configPath, out initTxt, out filesTxt, out regTxt, out defTxt, out toStrTxt, out clearTxt, out regClearTxt);

                    initTexts.Add(initTxt);
                    filesTexts.Add(filesTxt);
                    regTexts.Add(regTxt);
                    defTexts.Add(defTxt);
                    toStrTexts.Add(toStrTxt);
                    clearTexts.Add(clearTxt);
                    regClearTexts.Add(regClearTxt);
                }
            }
        }

        string txt = PackageDefineCode(defTexts);
        SaveDefineCode(txt);

        txt = PackageDynamicCode(initTexts, filesTexts, regTexts, toStrTexts, clearTexts, regClearTexts);
        SaveDynamicCode(txt);
    }

    private static void ParseConfig(string configPath, out string initTxt, out string filesTxt, out string regTxt, out string defTxt, out string toStrTxt, out string clearTxt, out string regClearTxt)
    {
        string path = ExcelLoader.ExcelRootDirectory + "config/" + configPath + ".json";

        using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        {
            StreamReader oReader = new StreamReader(fileStream, Encoding.UTF8);
            string configTxt = oReader.ReadToEnd();

            JsonData data = JsonMapper.ToObject(configTxt);

            JsonData filesData = data["files"];
            JsonData fieldData = data["fields"];
            JsonData primaryKey = null;

            string primaryKeyType = "int";
            if (data.ContainsKey("primaryKey"))
            {
                primaryKey = data["primaryKey"];
                for (int i = 0; i < fieldData.Count; ++i)
                {
                    JsonData fieldDef = fieldData[i];
                    string fieldName = fieldDef["name"].ToString();
                    if (fieldName == primaryKey.ToString())
                    {
                        primaryKeyType = fieldDef["type"].ToString();
                        break;
                    }
                }
            }

            string className = Path.GetFileNameWithoutExtension(configPath);

            initTxt = "\t// " + className + "\r\n\tstatic void init_" + className + "(string[] datas)\r\n";
            initTxt += "\t{\r\n";
            initTxt += "\t\texcel_" + className + " excel = new excel_" + className + "();\r\n";
            initTxt += "\r\n";

            toStrTxt = "\tpublic static string ToLineString(this excel_" + className + " excel)\r\n";
            toStrTxt += "\t{\r\n";
            toStrTxt += "\t\tstring rst = string.Empty;\r\n\r\n";

            clearTxt = "\tpublic static void clear_" + className + "()\r\n";
            clearTxt += "\t{\r\n";
            clearTxt += "\t\texcel_" + className + ".Clear();\r\n";
            clearTxt += "\t}\r\n";
            clearTxt += "\r\n";

            defTxt = "public class excel_" + className + " : ExcelBase<excel_" + className + ", " + primaryKeyType + ">, IExcelLine<" + primaryKeyType + ">\r\n";
            defTxt += "{\r\n";

            for (int i = 0; i < fieldData.Count; ++i)
            {
                JsonData fieldDef = fieldData[i];
                string fieldName = fieldDef["name"].ToString();
                string fieldType = fieldDef["type"].ToString();
                if (fieldType == "idenum")
                {
                    fieldType = "int";
                }

                if (fieldDef.ContainsKey("access"))
                {
                    string access = fieldDef["access"].ToString();
                    access = access.ToLower();
                    if (!access.Contains("c"))
                    {
                        continue;
                    }
                }

                defTxt += "\tpublic " + fieldType + " " + fieldName + ";\r\n";

                if (fieldType.Contains("[]"))
                {
                    fieldType = fieldType.Replace("[]", "_array");
                }

                initTxt += "\t\tparse_" + fieldType + "(datas[" + i + "], out excel." + fieldName + ");\r\n";

                toStrTxt += "\t\trst += tostring(excel." + fieldName + ");\r\n";
                if (i < fieldData.Count - 1)
                {
                    toStrTxt += "\t\trst += \'\\t\';\r\n\r\n";
                }
            }

            if (primaryKey != null)
            {
                defTxt += "\r\n\tpublic " + primaryKeyType + " GetPrimaryKey()\r\n";
                defTxt += "\t{\r\n";
                defTxt += "\t\treturn " + primaryKey.ToString() + ";\r\n";
                defTxt += "\t}\r\n";
            }

            defTxt += "}\r\n";

            initTxt += "\r\n";
            initTxt += "\t\texcel_" + className + ".Add(excel);\r\n";
            initTxt += "\t}\r\n";

            toStrTxt += "\t\treturn rst;\r\n";
            toStrTxt += "\t}\r\n";
            toStrTxt += "\r\n";

            filesTxt = "\t\tfilesNames[\"" + className + "\"] = new string[]\r\n";
            filesTxt += "\t\t{\r\n";

            for (int i = 0; i < filesData.Count; ++i)
            {
                JsonData fileData = filesData[i];
                string file = fileData.ToString();

                filesTxt += "\t\t\t\"" + file + "\",\r\n";
            }
            filesTxt += "\t\t};";

            regTxt = "\t\tInitializers[\"" + className + "\"] = init_" + className + ";";
            regClearTxt = "\t\tClearFunctions[\"" + className + "\"] = clear_" + className + ";";
        }
    }

    private static string GetClassCode(string className, JsonData fieldDef)
    {
        string txt = string.Empty;

        return txt;
    }

    private static string PackageDynamicCode(List<string> initTexts, List<string> filesTexts, List<string> regTexts, List<string> toStrTxt, List<string> clearTexts, List<string> regClearTexts)
    {
        string txt = string.Empty;
        txt = "using System;\r\n";
        txt += "\r\n";
        txt += "public static partial class ExcelFactory\r\n";
        txt += "{\r\n";

        for (int i = 0; i < initTexts.Count; ++i)
        {
            string initTxt = initTexts[i];
            txt += initTxt;
            txt += "\r\n";
        }

        for (int i = 0; i < clearTexts.Count; ++i)
        {
            string clearTxt = clearTexts[i];
            txt += clearTxt;
        }

        txt += "\tstatic void InitClearFunc()\r\n";
        txt += "\t{\r\n";
        for (int i = 0; i < regClearTexts.Count; ++i)
        {
            string clearsTxt = string.Empty;
            if (i > 0)
            {
                clearsTxt = "\r\n";
            }
            clearsTxt += regClearTexts[i];
            txt += clearsTxt;
            txt += "\r\n";
        }
        txt += "\t}\r\n";
        txt += "\r\n";

        txt += "\tstatic void InitFiles()\r\n";
        txt += "\t{\r\n";
        for (int i = 0; i < filesTexts.Count; ++i)
        {
            string filesTxt = string.Empty;
            if (i > 0)
            {
                filesTxt = "\r\n";
            }
            filesTxt += filesTexts[i];
            txt += filesTxt;
            txt += "\r\n";
        }
        txt += "\t}\r\n";
        txt += "\r\n";

        txt += "\tpublic static void Initialize()\r\n";
        txt += "\t{\r\n";
        for (int i = 0; i < regTexts.Count; ++i)
        {
            string regTxt = string.Empty;
            if (i > 0)
            {
                regTxt = "\r\n";
            }
            regTxt += regTexts[i];
            txt += regTxt;
            txt += "\r\n";
        }
        txt += "\t}\r\n";

        txt += "\r\n\r\n#if UNITY_EDITOR\r\n";

        for (int i = 0; i < toStrTxt.Count; ++i)
        {
            string t = toStrTxt[i];
            txt += t;
            txt += "\r\n";
        }

        txt += "\r\n#endif\r\n";

        txt += "}";

        return txt;
    }

    private static string PackageDefineCode(List<string> classTexts)
    {
        string txt = "using System;\r\n\r\n";

        for (int i = 0; i < classTexts.Count; ++i)
        {
            txt += classTexts[i];
        }
        return txt;
    }

    private static void SaveDynamicCode(string txt)
    {
        if (File.Exists(DynamicFactoryCodePath))
        {
            File.Delete(DynamicFactoryCodePath);
        }
        using (FileStream fileStream = new FileStream(DynamicFactoryCodePath, FileMode.CreateNew, FileAccess.Write, FileShare.ReadWrite))
        {
            StreamWriter writer = new StreamWriter(fileStream, Encoding.UTF8);

            writer.Write(txt);

            writer.Flush();
            writer.Close();
        }
    }

    private static void SaveDefineCode(string txt)
    {
        if (File.Exists(ExcelDefineCodePath))
        {
            File.Delete(ExcelDefineCodePath);
        }
        using (FileStream fileStream = new FileStream(ExcelDefineCodePath, FileMode.CreateNew, FileAccess.Write, FileShare.ReadWrite))
        {
            StreamWriter writer = new StreamWriter(fileStream, Encoding.UTF8);

            writer.Write(txt);

            writer.Flush();
            writer.Close();
        }
    }
}