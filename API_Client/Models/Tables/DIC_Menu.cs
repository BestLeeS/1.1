using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Models
{
	public class DIC_Menu
	{
		[SugarColumn(IsPrimaryKey = true, ColumnName = "ID")]
		public Guid id { get; set; }
		[SugarColumn(ColumnName = "MenuIndex")]
		public string index { get; set; }
		[SugarColumn(ColumnName = "MenuName")]
		public string name { get; set; }
		[SugarColumn(ColumnName = "Path")]
		public string path { get; set; }
		[SugarColumn(ColumnName = "Componentpath")]
		public string componentpath { get; set; }
		[SugarColumn(IsIgnore = true)]
		public string component { get; set; }
		[SugarColumn(ColumnName = "Redirect")]
		public string redirect { get; set; }
		[SugarColumn(ColumnName = "ParentID")]
		public Guid parentID { get; set; }
		[SugarColumn(IsIgnore = true)]
		public string parentName { get; set; }
		[SugarColumn(ColumnName = "Title")]
		public string title { get; set; }
		[SugarColumn(ColumnName = "Icon")]
		public string icon { get; set; }
		[SugarColumn(IsIgnore = true)]
		public meta meta { get; set; } = new meta();
		[SugarColumn(ColumnName = "OrderNum")]
		public int orderNum { get; set; }
		
		public bool Enable { get; set; }
		[SugarColumn(IsIgnore = true)]
		public Guid GroupID { get; set; }
		[SugarColumn(IsIgnore = true)]
		public List<DIC_Menu> children { get; set; }
	}

	public class meta
	{
		public string title { get; set; }
		public string icon { get; set; }
	}
}
