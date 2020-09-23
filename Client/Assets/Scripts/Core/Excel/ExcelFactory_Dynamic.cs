using System;

public static partial class ExcelFactory
{
	// cha_list
	static void init_cha_list(string[] datas)
	{
		excel_cha_list excel = new excel_cha_list();

		parse_int(datas[0], out excel.id);
		parse_string(datas[1], out excel.name);
		parse_int(datas[2], out excel.type);
		parse_int(datas[3], out excel.race);
		parse_string(datas[4], out excel.path);
		parse_float(datas[5], out excel.halfSize);
		parse_string(datas[6], out excel.portrait);

		excel_cha_list.Add(excel);
	}

	public static void clear_cha_list()
	{
		excel_cha_list.Clear();
	}

	static void InitClearFunc()
	{
		ClearFunctions["cha_list"] = clear_cha_list;
	}

	static void InitFiles()
	{
		filesNames["cha_list"] = new string[]
		{
			"cha/cha_list",
		};
	}

	public static void Initialize()
	{
		Initializers["cha_list"] = init_cha_list;
	}


#if UNITY_EDITOR
	public static string ToLineString(this excel_cha_list excel)
	{
		string rst = string.Empty;

		rst += tostring(excel.id);
		rst += '\t';

		rst += tostring(excel.name);
		rst += '\t';

		rst += tostring(excel.type);
		rst += '\t';

		rst += tostring(excel.race);
		rst += '\t';

		rst += tostring(excel.path);
		rst += '\t';

		rst += tostring(excel.halfSize);
		rst += '\t';

		rst += tostring(excel.portrait);
		return rst;
	}



#endif
}