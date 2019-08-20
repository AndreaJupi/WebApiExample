using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebApiExample.Helper;
using WebApiExample.Repositories.Interfaces;
using WebApiExample.TemplateMethod;

namespace WebApiExample.Controllers
{
    [Route("api/[controller]")]
    public class SampleDataController : Controller
    {
        private readonly Context _context;
        private readonly IFileReaderRepository _fileReader;
        private string _option1;

        public SampleDataController(IOptions<MyOptions> optionsAccessor, Context context, IFileReaderRepository fileReader)
        {
            this._option1 = optionsAccessor.Value.Option1;
           //options.Option1 = optionsAccessor.CurrentValue.Option1;
            _context = context;
            _fileReader = fileReader;
        }

        private static string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        [HttpGet("[action]")]
        public async Task<IActionResult> ImportCSVFile()
        {
            //template method pattern example
            DataReader daoStudentsA = new StudentA(_context);
            daoStudentsA.Run(this._option1);

            DataReader daoStudentsB = new StudentB(_context);
            daoStudentsB.Run(this._option1);

            //repository pattern example
            List<Student> students = new List<Student>();
            students = _fileReader.ReadCSVFile(this._option1);

            if (students.Count() == 0)
            {
                return BadRequest("Data are empty.");
            }
             _fileReader.ImportStudents(students);

            return Ok();
        }

        [HttpGet("[action]")]
        public IEnumerable<WeatherForecast> WeatherForecasts()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                DateFormatted = DateTime.Now.AddDays(index).ToString("d"),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            });
        }

        public class WeatherForecast
        {
            public string DateFormatted { get; set; }
            public int TemperatureC { get; set; }
            public string Summary { get; set; }

            public int TemperatureF
            {
                get
                {
                    return 32 + (int)(TemperatureC / 0.5556);
                }
            }
        }
    }
}
