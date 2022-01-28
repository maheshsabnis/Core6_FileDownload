using Core6_FileDownload.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;

namespace Core6_FileDownload.Controllers
{
    public class DownloadController : Controller
    {
        private readonly IFileProvider fileProvider;

        public DownloadController(IFileProvider provider)
        {
            fileProvider = provider;
        }

        public IActionResult Index()
        {
            var fileModels = new FilesModels();
            foreach (var item in fileProvider.GetDirectoryContents(""))
            {
                fileModels.files.Add(new FileModel() 
                {
                    FileName = item.Name, 
                    FilePath = item.PhysicalPath 
                });
            }
            return View(fileModels.files);
        }


        public async Task<IActionResult> DownloadFile(string fileName)
        {
            if (string.IsNullOrEmpty(fileName) || fileName == null)
            {
                return Content("File Name is Empty...");              
            }

            // get the filePath

            var filePath = Path.Combine(Directory.GetCurrentDirectory(),
                "ServerFiles", fileName);

            // create a memorystream
            var memoryStream = new MemoryStream();

            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memoryStream);            
            }
            // set the position to return the file from
            memoryStream.Position = 0;

            // Get the MIMEType for the File
            var mimeType = (string file) =>
            {
                var mimeTypes = MimeTypes.GetMimeTypes();
                var extension = Path.GetExtension(file).ToLowerInvariant();
                return mimeTypes[extension];
            };

            return File(memoryStream, mimeType(filePath), Path.GetFileName(filePath));
        }
    }
}
