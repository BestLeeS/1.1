using System;
using System.Collections.Generic;
using System.Text;

namespace Models.SysFunc
{
    public class ReqDICMenu
    {
        public Guid ID { get; set; }
        public string MenuName { get; set; }
        public string Title { get; set; }
        public string Path { get; set; }
        public string ComponentPath { get; set; }
        public string Redirect { get; set; }
        public string Icon { get; set; }
        public Guid? ParentID { get; set; }
        public int OrderNum { get; set; }
        public bool Enable { get; set; }
    }
}
