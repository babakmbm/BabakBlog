using Blog.Models.FileExplorer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blog.Controllers
{
    public class ExplorerController : Controller
    {
        public bool IsAdmin { get { return Session["IsAdmin"] != null && (bool)Session["IsAdmin"]; } }

        public ActionResult Index(string path)
        {
            if (IsAdmin)
            {
                ViewBag.IsAdmin = IsAdmin;
                string realPath;
                realPath = Server.MapPath("~/Content/" + path);
                ViewBag.real = realPath;
                if (System.IO.File.Exists(realPath))
                {
                    return base.File(realPath, "application/octet-stream"); // application/octet-stream For UnKnown File Types (هر نوع فایلی) 
                }
                else if (System.IO.Directory.Exists(realPath))
                {
                    Uri url = Request.Url;
                    if (url.ToString().Last() != '/')
                    {
                        Response.Redirect("/Explorer/" + path + "/");
                    }

                    //For Directory Listing
                    List<DirectoryModel> directoryListModel = new List<DirectoryModel>();
                    IEnumerable<string> directoryList = Directory.EnumerateDirectories(realPath);

                    foreach (string dir in directoryList)
                    {
                        DirectoryInfo d = new DirectoryInfo(dir);
                        DirectoryModel dirModel = new DirectoryModel();

                        dirModel.DirectoryName = Path.GetFileName(dir);
                        dirModel.directoyAccessed = d.LastAccessTime;

                        directoryListModel.Add(dirModel);
                    }

                    //For File Listing
                    List<FileModel> fileListModel = new List<FileModel>();
                    IEnumerable<string> fileList = Directory.EnumerateFiles(realPath);

                    foreach (string file in fileList)
                    {
                        FileInfo f = new FileInfo(file);
                        FileModel fileModel = new FileModel();

                        if (f.Extension.ToLower() != "php" && f.Extension.ToLower() != "aspx"
                            && f.Extension.ToLower() != "asp")
                        {
                            fileModel.FileName = Path.GetFileName(file);
                            fileModel.FileExtension = f.Extension.ToLower();
                            fileModel.modalFlag = Path.GetFileName(file).Replace(".","1").Replace(" ","1");
                            fileModel.FileAccessed = f.LastAccessTime;
                            fileModel.FileSize = (f.Length < 1024) ? f.Length.ToString() + " B" : f.Length / 1024 + " KB";
                            fileListModel.Add(fileModel);
                        }
                    }

                    ExplorerModel explorerModel = new ExplorerModel(directoryListModel, fileListModel);

                    return View(explorerModel);
                }
                else
                {
                    return Content(path + " وجود ندارد. "); //For Invalid Directories (چنین فایل یا فولدری وجود ندارد)
                }

            }
            else
            {
                return View("NotAdmin");
            }
        }
    }
}
