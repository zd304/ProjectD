using System;

public class excel_cha_list : ExcelBase<excel_cha_list, int>, IExcelLine<int>
{
	public int id;
	public string name;
	public int type;
	public int race;
	public string path;
	public float halfSize;
	public string portrait;

	public int GetPrimaryKey()
	{
		return id;
	}
}
