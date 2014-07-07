using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blog.Models.FileExplorer
{
    public class ExplorerModel
    {
        public List<DirectoryModel> DirectoryModelList { get; set; }
        public List<FileModel> FileModelList { get; set; }

        public ExplorerModel(List<DirectoryModel> _directoryModelList ,List<FileModel> _fileModelList)
        {
            DirectoryModelList = _directoryModelList;
            FileModelList = _fileModelList;
        }
    }
}