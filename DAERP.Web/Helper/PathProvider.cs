using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace DAERP.Web.Helper
{
    public class PathProvider : IPathProvider
    {
        private IWebHostEnvironment _hostEnvironment;
        public PathProvider(IWebHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
        }
        public string MapPath(string path)
        {
            string filePath = Path.Combine(_hostEnvironment.WebRootPath, path);
            return filePath;
        }
    }
}
