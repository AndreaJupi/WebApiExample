using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiExample.TemplateMethod
{
    public abstract class DataReader
    {
        public abstract void SelectStudents(string filePath);

        public void Run(string filePath)
        {
            SelectStudents(filePath);
        }
    }
}
