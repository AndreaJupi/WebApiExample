using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiExample.Helper;
using WebApiExample.Repositories.Interfaces;

namespace WebApiExample.TemplateMethod
{
    public class StudentA : DataReader
    {
        private static Context _context;
        public StudentA(Context context)
        {
            _context = context;
        }
        IFileReaderRepository fileReader = new FileReaderRepository(_context);

        public override void SelectStudents(string filePath)
        {
            List<Student> students = new List<Student>();
            students = fileReader.ReadCSVFile(filePath).ToList();
            var studentsByFirstName = students.Select(x => x.FirstMidName).ToList();
        }
    }
}
