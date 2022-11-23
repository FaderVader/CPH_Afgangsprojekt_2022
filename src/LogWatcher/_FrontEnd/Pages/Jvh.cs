using Domain.Database;
using Domain.Models;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace FrontEnd.Pages
{
    public partial class Jvh : ComponentBase
    {
        [Inject]
        public SqlConnect GetData { get; set; }

        public string OutputValue { get; set; }


        protected override async Task OnInitializedAsync()
        {
            var result = await GetData.GetLogFileById(2);
            OutputValue = result.FileName;

            await UpdateDB();
        }

        private async Task UpdateDB()
        {
            var logFile = new LogFile { SourceSystemID = 4, FileName = "log4.log", FileHash = "ddd" };
            await GetData.CreateLogFile(logFile);
        }
    }
}
