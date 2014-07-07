using Blog.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Globalization;

namespace Blog.Controllers
{
    public class PostsController : Controller
    {
        private BlogModel model = new BlogModel();
        public bool IsAdmin { get { return Session["IsAdmin"] != null && (bool)Session["IsAdmin"]; } }
        private const int PostsPerPage = 2;
        private const int PostPerFeed = 25;
        
        public ActionResult Index(int? id)
        {
            int pageNumber = id ?? 0;
            IEnumerable<Post> posts =
                (from post in model.Posts
                where post.DateTime < DateTime.Now
                orderby post.DateTime descending
                select post).Skip(pageNumber * PostsPerPage).Take(PostsPerPage + 1);
            ViewBag.IsPreviousLinkVisible = pageNumber > 0;
            ViewBag.IsNextLinkVisible = posts.Count() > PostsPerPage;
            ViewBag.PageNumber = pageNumber;
            ViewBag.IsAdmin = IsAdmin;
            return View(posts.Take(PostsPerPage));
        }

        public ActionResult PostList()
        {
            if (IsAdmin)
            {
                ViewBag.IsAdmin = IsAdmin;
                IEnumerable<Post> posts = (from p in model.Posts
                                           orderby p.DateTime descending
                                           select p).ToList();
                return View(posts);
            }
            else
            {
                return View("NotAdmin");
            }

        }

        public ActionResult RSS()
        {
            IEnumerable<SyndicationItem> posts =
                (from post in model.Posts
                 where post.DateTime < DateTime.Now
                 orderby post.DateTime descending
                 select post).Take(PostPerFeed).ToList().Select(x => GetSyndicationItem(x));

            SyndicationFeed feed = new SyndicationFeed("Babak Hosseini", "Babak Hosseini's Blog", new Uri("http://www.techlist.ir"), posts);
            Rss20FeedFormatter formattedFeed = new Rss20FeedFormatter(feed);
            return new FeedResult(formattedFeed);
        }

        private SyndicationItem GetSyndicationItem(Post post)
        {
            return new SyndicationItem(post.Title, post.Body, new Uri("http://www.techlist.ir/Posts/Details/" + post.ID));
        }

        public ActionResult Details(int? id)
        {
            Post post = GetPost(id);
            ViewBag.IsAdmin = IsAdmin;
            return View(post);
        }

        [HttpPost]
        public ActionResult CommentModal(int id, string name, string email, string body )
        {
            if (!id.Equals(null) 
                || !string.IsNullOrWhiteSpace(name)
                || !string.IsNullOrWhiteSpace(email)
                || !string.IsNullOrWhiteSpace(body)
               )
            {
                Comment comment = new Comment()
                {
                    PostID = id,
                    DateTime = DateTime.Now,
                    Name = name,
                    Email = email,
                    Body = body
                };
                try
                {
                    model.Comments.Add(comment);
                    model.SaveChanges();
                    ViewBag.Message = "نظر شما با موفقیت ثبت شد و پس از بررسی در وبسایت قرار خواهد گرفت:";
                    ViewBag.backLink = "Details/" + comment.PostID;
                    
                    return View("Success");
                }
                catch
                {
                    ViewBag.Message = "متاسفانه مشکلی در انجام عملیات پیش آمده است. لطفا مجددا تلاش کنید.";
                    return View("Error");
                }

            }
            else
            {
                return View("Error");
            }

        }

        public ActionResult Delete(int id)
        {
            if (IsAdmin)
            {
                Post post = GetPost(id);
                model.Posts.Remove(post);
                model.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public ActionResult DeleteComment(int id)
        {
            if (IsAdmin)
            {
                Comment comment = model.Comments.Where(x => x.ID == id).First();
                var postID = comment.Post.ID;
                model.Comments.Remove(comment);
                model.SaveChanges();
                return RedirectToAction("Details", new { id = postID });
            }
            else
            {
                return RedirectToAction("NotAdmin");
            }
            
        }

        public ActionResult Tags(string id)
        {
            Tag tag = GetTag(id);
            ViewBag.IsAdmin = IsAdmin;
            ViewBag.tagname = id;
            return View("Tags", tag.Posts);
        }

        [HttpGet]
        public ActionResult CreatePost()
        {
            ViewBag.IsAdmin = IsAdmin;
            if (IsAdmin)
            {
                Post post = new Post();
                post.DateTime = DateTime.Now;
                post.DateModified = DateTime.Now;
                return View(post);
            }
            else
            {
                return RedirectToAction("NotAdmin");
            }
        }

        [HttpPost]
        public ActionResult CreatePost(Post post, string t)
        {
            ViewBag.IsAdmin = IsAdmin;
            if (IsAdmin)
            {
                if (!string.IsNullOrEmpty(t))
                {
                    ViewBag.tagsFlag = true;
                    ViewBag.TTags = t;
                    var tags = t ?? string.Empty;
                    string[] tagNames = tags.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string tagName in tagNames)
                    {
                        post.Tags.Add(new Tag {Name=tagName});
                    }
                    if (ModelState.IsValid)
                    {
                        model.Posts.Add(post);
                        model.SaveChanges();
                        return RedirectToAction("Index");
                    }

                    return View(post);
                }
                else
                {
                    ViewBag.tagsFlag = false;
                    ViewBag.NoTags = "حداقل یک تگ وارد کنید";
                    return View(post);
                }
            }
            else
            {
                return RedirectToAction("NotAdmin");
            }
        }


        [HttpGet]
        public ActionResult EditPost(int id)
        {
            if (IsAdmin)
            {
                
                Post post = model.Posts.Find(id);
                ViewBag.d1 = post.Tags.Count();
                ViewBag.IsAdmin = IsAdmin;
                post.DateModified = DateTime.Now;
                return View(post);
            }
            else
            {
                return RedirectToAction("NotAdmin");
            }
        }

        [HttpPost]
        public ActionResult EditPost(Post post, string t, int id)
        {
            if (IsAdmin)
            {
                var v = model.Posts.Find(id); //Fixed IT
                if (!string.IsNullOrEmpty(t))
                {
                    post.Tags.Clear();
                    var tags = t ?? string.Empty;
                    string[] tagNames = tags.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string tagName in tagNames)
                    {
                        if (!v.Tags.Contains(new Tag {Name = tagName}))
                        {
                            v.Tags.Add(new Tag { Name = tagName });
                        }
                        else
                        {
                            ViewBag.err1 = string.Format("{0} alerdy exist", tagName);
                        }
                    }
                    ViewBag.d = v.Tags.Count();
                }
                if (ModelState.IsValid)
                {
                    model.Entry(v).CurrentValues.SetValues(post); //Fixed IT
                    model.SaveChanges();
                    return RedirectToAction("Details/" + post.ID);
                }

                return View(post);

            }
            else
            {
                return RedirectToAction("NotAdmin");
            }

        }

        public ActionResult NotAdmin()
        {
            return View();
        }

        public ActionResult Search(string sq)
        {
            ViewBag.IsAdmin = IsAdmin;
            ViewBag.sq = sq;
            var nist = "NoResult";
            var posts = model.Posts
                             .Where(a => a.Title.Contains(sq))
                             .Take(10);
            if (posts.Any())
            {
                return View(posts);
            }
            else return View(nist);
        }

        public ActionResult Comments()
        {
            ViewBag.IsAdmin = IsAdmin;
            if (IsAdmin)
            {
                IEnumerable<Comment> comments = (from c in model.Comments
                                                 orderby c.PostID descending
                                                 select c).ToList();
                return View(comments);
            }
            else
            {
                return RedirectToAction("NotAdmin");
            }
        }

        private Tag GetTag(string tagName)
        {
            return model.Tags.Where(x => x.Name == tagName).FirstOrDefault() ?? new Tag() { Name = tagName };

        }

        private Post GetPost(int? id)
        {
            if (id.HasValue)
            {
                return model.Posts.Where(x => x.ID == id).First();
            }
            else
            {
                return new Post() {ID = -1 };
            }
        }

        //[HttpGet]
        //public ActionResult CreateComment(int id)
        //{
        //    Comment comment = new Comment();
        //    ViewBag.IsAdmin = IsAdmin;
        //    comment.PostID = id;
        //    comment.DateTime = DateTime.Now;
        //    return View(comment);
        //}


        //[HttpPost]
        //[ValidateInput(false)]
        //public ActionResult CreateComment(Comment comment)
        //{
        //    ViewBag.IsAdmin = IsAdmin;
        //    if (ModelState.IsValid)
        //    {
        //        model.Comments.Add(comment);
        //        model.SaveChanges();
        //        return RedirectToAction("Details", new { id = comment.PostID });
        //    }

        //    return View(comment);
        //}

    }
}
