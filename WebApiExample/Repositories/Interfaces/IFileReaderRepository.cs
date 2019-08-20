using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiExample.Repositories.Interfaces
{
    public interface IFileReaderRepository
    {
        List<Student> ReadCSVFile(string filePath);
        void ImportStudents(List<Student> students);
    }
}
