using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiExample.Repositories.Interfaces
{
    public class FileReaderRepository : IFileReaderRepository
    {
        private readonly Context _context;
        public FileReaderRepository(Context context)
        {
            _context = context;
        }
        public List<Student> ReadCSVFile(string filePath)
        {
            List<Student> students = new List<Student>();
            FileInfo file = new FileInfo(filePath);
            var fullName = file.FullName;
            if (file != null && file.Length > 0)
            {
                var filepath = fullName;
                if (file.Extension == ".xls" || file.Extension == ".xlsx")
                {
                    // Auto-detect format, supports:
                    //  - Binary Excel files (2.0-2003 format; *.xls)
                    //  - OpenXml Excel files (2007 format; *.xlsx)

                    System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                    //open file and returns as Stream
                    using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
                    {
                        using (var reader = ExcelReaderFactory.CreateReader(stream))
                        {
                            var t = reader.AsDataSet(new ExcelDataSetConfiguration()
                            {

                                // Gets or sets a value indicating whether to set the DataColumn.DataType
                                // property in a second pass.
                                UseColumnDataType = true,

                                // Gets or sets a callback to obtain configuration options for a DataTable.
                                ConfigureDataTable = (tableReader) => new ExcelDataTableConfiguration()
                                {

                                    // Gets or sets a value indicating the prefix of generated column names.
                                    EmptyColumnNamePrefix = "Column",

                                    // Gets or sets a value indicating whether to use a row from the
                                    // data as column names.
                                    UseHeaderRow = true,

                                    // Gets or sets a callback to determine which row is the header row.
                                    // Only called when UseHeaderRow = true.
                                    ReadHeaderRow = (rowReader) =>
                                    {
                                        // F.ex skip the first row and use the 2nd row as column headers:
                                        //rowReader.Read();
                                    },

                                    // Gets or sets a callback to determine whether to include the
                                    // current row in the DataTable.
                                    FilterRow = (rowReader) =>
                                    {
                                        return true;
                                    },

                                    // Gets or sets a callback to determine whether to include the specific
                                    // column in the DataTable. Called once per column after reading the
                                    // headers.
                                    FilterColumn = (rowReader, columnIndex) =>
                                    {
                                        return true;
                                    }
                                }
                            });

                            foreach (var col in t.Tables[0].Columns)
                            {

                                var c = col as DataColumn;

                                c.ColumnName = c.ColumnName
                                                        .Replace(" ", "")
                                                        .Replace("(", "")
                                                        .Replace(")", "")
                                                        .Replace("-", "")
                                                        .Replace("/", "");

                            }
                            students = t.Tables[0].ToList<Student>();
                        }
                    }
                }
            }
            
            return students;
        }
        public void ImportStudents(List<Student> students)
        {
            _context.AddRange(students);
            _context.SaveChanges();
        }
    }
}
