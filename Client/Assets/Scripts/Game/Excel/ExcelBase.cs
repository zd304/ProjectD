using System.Collections.Generic;
using UnityEngine;

public interface IExcelLine<U>
{
    U GetPrimaryKey();
}

public class ExcelBase<T, U> where T : IExcelLine<U>
{
    public static void Add(T excel)
    {
        if (excelView == null)
        {
            excelView = new Dictionary<U, T>();
        }
#if UNITY_EDITOR
        U id = excel.GetPrimaryKey();
        if (excelView.ContainsKey(id))
        {
            Debug.LogError("表格" + typeof(T).Name + "ID重复，重复ID为：" + id + "，请策划大大检查一下");
        }
#endif
        excelView.Add(excel.GetPrimaryKey(), excel);
    }

    public static void Remove(U id)
    {
        if (excelView == null)
        {
            return;
        }
        excelView.Remove(id);
    }

    public static T Find(U id)
    {
        if (excelView == null)
        {
            return default(T);
        }
        T excel;
        if (!excelView.TryGetValue(id, out excel))
        {
            return default(T);
        }
        return excel;
    }

    public static Dictionary<U, T>.Enumerator GetEnumerator()
    {
        if (excelView == null)
        {
            Debug.LogError("excelView为空");
            return default(Dictionary<U, T>.Enumerator);
        }
        return excelView.GetEnumerator();
    }

    public static void Clear()
    {
        excelView = null;
    }

    public static bool IsEmpty
    {
        get
        {
            return excelView == null;
        }
    }

    public static Dictionary<U, T> excelView = null;
}