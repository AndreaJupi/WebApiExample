using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiExample.Helper
{
    public static class Helper
    {
        public static List<DataList> CreateExcelList(DataTable table)
        {
            List<DataList> list = new List<DataList>();

            foreach (var item in table.Columns)
            {
                list.Add(new DataList() { Name = item.ToString() });
            }

            return list;
        }
    }


    public class DataList
    {
        public string Name { get; set; }
    }
}
