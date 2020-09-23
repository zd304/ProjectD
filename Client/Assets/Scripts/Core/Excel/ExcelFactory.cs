using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

// 工厂为了去掉反射
public static partial class ExcelFactory
{
    public static readonly char[] ArraySeperators = new char[] { '*', ',', '\"' };
    private static Dictionary<string, int> enumIDDict = new Dictionary<string, int>();

    public delegate void InitExcelLineFunc(string[] datas);
    public static Dictionary<string, InitExcelLineFunc> Initializers = null;
    public delegate void ClearExcelFunc();
    public static Dictionary<string, ClearExcelFunc> ClearFunctions = null;

    public static Dictionary<string, string[]> filesNames = null;

    public static void InitializeFactory()
    {
        Initializers = new Dictionary<string, InitExcelLineFunc>();
        filesNames = new Dictionary<string, string[]>();
        ClearFunctions = new Dictionary<string, ClearExcelFunc>();
        InitClearFunc();
        InitFiles();
        Initialize();
    }

    public static void UninitializeFactory()
    {
        ClearFunctions.Clear();
        ClearFunctions = null;

        Initializers.Clear();
        Initializers = null;

        filesNames.Clear();
        filesNames = null;
    }

    public static void ReloadExcel(string excelName, string[] files = null)
    {
        InitializeFactory();

        if (files != null)
        {
            filesNames[excelName] = files;
        }
        ExcelLoader.ParseConfig(excelName);

        UninitializeFactory();
    }

    public static object InitExcelLine(string className, string[] datas)
    {
        InitExcelLineFunc func = null;
        if (Initializers.TryGetValue(className, out func))
        {
            func(datas);
        }

        return null;
    }

    public static string[] GetFiles(string className)
    {
        string[] files = null;
        if (filesNames.TryGetValue(className, out files))
        {
            return files;
        }
        return null;
    }

    public static string GetHeaderFieldNames(Type type)
    {
        string rst = string.Empty;

        FieldInfo[] fieldInfos = type.GetFields();
        for (int i = 0; i < fieldInfos.Length; ++i)
        {
            FieldInfo fieldInfo = fieldInfos[i];
            if (i > 0)
            {
                rst += "\t";
            }
            rst += fieldInfo.Name;
        }
        return rst;
    }

    #region Parse

    static void parse_int(string data, out int rst)
    {
        if (!int.TryParse(data, out rst))
        {
            enumIDDict.TryGetValue(data, out rst);
        }
    }

    static void parse_float(string data, out float rst)
    {
        float.TryParse(data, out rst);
    }

    static void parse_int_array(string data, out int[] rst)
    {
        string[] datas = data.Split(ArraySeperators, StringSplitOptions.RemoveEmptyEntries);
        rst = new int[datas.Length];
        for (int i = 0; i < rst.Length; ++i)
        {
            int.TryParse(datas[i], out rst[i]);
        }
    }

    static void parse_float_array(string data, out float[] rst)
    {
        string[] datas = data.Split(ArraySeperators, StringSplitOptions.RemoveEmptyEntries);
        rst = new float[datas.Length];
        for (int i = 0; i < rst.Length; ++i)
        {
            float.TryParse(datas[i], out rst[i]);
        }
    }

    static void parse_string(string data, out string rst)
    {
        if (data.Length > 0)
        {
            if (data.Substring(0, 1) == "\"")
            {
                rst = data.Substring(1, data.Length - 2);
                rst.Replace("\"\"", "\"");
            }
            else
            {

                rst = data;
            }
        }
        else
        {
            rst = data;
        }


    }

    static void parse_string_array(string data, out string[] rst)
    {
        rst = data.Split(ArraySeperators, StringSplitOptions.RemoveEmptyEntries);
    }

    static void parse_idenum(string data, out int rst)
    {
        enumIDDict.TryGetValue(data, out rst);
    }
    #endregion

    // 以下函数一般是编辑器下使用，不太关心性能
    #region ToString
    public static string tostring(int v)
    {
        return string.Format("{0}", v);
    }

    static string tostring(float v)
    {
        return string.Format("{0}", v);
    }

    static string tostring(string v)
    {
        return v;
    }

    static string tostring(int[] v, char seperator = '*')
    {
        string rst = string.Empty;
        if (v == null)
        {
            return rst;
        }
        for (int i = 0; i < v.Length; ++i)
        {
            rst += tostring(v[i]);
            if (i < v.Length - 1)
            {
                rst += seperator;
            }
        }
        return rst;
    }

    static string tostring(float[] v, char seperator = ',')
    {
        string rst = string.Empty;
        if (v == null)
        {
            return rst;
        }
        for (int i = 0; i < v.Length; ++i)
        {
            rst += tostring(v[i]);
            if (i < v.Length - 1)
            {
                rst += seperator;
            }
        }
        return rst;
    }

    static string tostring(string[] v, char seperator = '*')
    {
        string rst = string.Empty;
        if (v == null)
        {
            return rst;
        }
        for (int i = 0; i < v.Length; ++i)
        {
            rst += tostring(v[i]);
            if (i < v.Length - 1)
            {
                rst += seperator;
            }
        }
        return rst;
    }
    #endregion
}