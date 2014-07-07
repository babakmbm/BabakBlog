using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blog.Models.FileExplorer
{
    public class FileModel
    {
        public string FileName { get; set; }
        public string FileSize { get; set; }
        public string FileExtension { get; set; }
        public string FilePath { get; set; }
        public string modalFlag { get; set; }
        public DateTime FileAccessed { get; set; }
    }
}