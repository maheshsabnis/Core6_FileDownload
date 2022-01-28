namespace Core6_FileDownload.Models
{
    public class FileModel
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
    }

    public class FilesModels
    {
        public List<FileModel> files { get; set; } = new List<FileModel>();
    }
}
