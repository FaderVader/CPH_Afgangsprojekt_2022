using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Domain.FileSystem
{
    public static class OpenLogFile
    {
        public static async Task OpenWithDefault(string filename)
        {
            var fileExists = await Task.Run(() => { return File.Exists(filename); });
            if (!fileExists) return;

            var startInfo = new ProcessStartInfo(filename) { UseShellExecute = true};
            using (var p = new Process())
            {
                p.StartInfo = startInfo;
                p.Start();
            }
        }
    }
}