using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Blog.Models;
using System.Text;
using System.ServiceModel.Syndication;
using System.Data;

namespace Blog.Controllers
{
    public class PostsController : Controller
    {
        private BlogModel model = new BlogModel();
        private const int PostsPerPage = 4;
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

        [HttpGet]
        public ActionResult CreateComment(int id)
        {
            Comment comment = new Comment();
            comment.PostID = id;
            comment.DateTime = DateTime.Now;
            return View(comment);
        }
        
        
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CreateComment(Comment comment)
        {
            if (ModelState.IsValid)
            {
                model.Comments.Add(comment);
                model.SaveChanges();
                return RedirectToAction("Details", new { id = comment.PostID });
            }

            return View(comment);
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
            if (IsAdmin)
            {
                Post post = new Post();
                post.DateTime = DateTime.Now;
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
                        post.Tags.Add(GetTag(tagName));
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
            if(IsAdmin){
            Post post = model.Posts.Find(id);
            ViewBag.IsAdmin = IsAdmin;
            post.DateModified = DateTime.Now;
            StringBuilder tagList = new StringBuilder();
            foreach (Tag tag in post.Tags)
            {
                tagList.AppendFormat("{0} ", tag.Name);
            }

            ViewBag.TagsList = tagList.ToString();

            return View(post);
            }
            else
            {
                return RedirectToAction("NotAdmin");
            }
        }

        [HttpPost]
        public ActionResult EditPost(Post post, string t) /*TODO: FIX THIS SHIT->TAGS UPDATE*/
        {

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
                        post.Tags.Add(GetTag(tagName));
                    }
                    if (ModelState.IsValid)
                    {
                        model.Entry(post).State = EntityState.Modified;
                        model.SaveChanges();
                        return RedirectToAction("Details/" + post.ID);
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
        
        
        /*
        [ValidateInput(false)]
        public ActionResult Update(int? id, string title, string body, DateTime dateTime, string tags)
        {
            if (!IsAdmin)
            {
                return RedirectToAction("Index");
            }
            Post post = GetPost(id);
            post.Title = title;
            post.Body = body;
            post.DateTime = dateTime;
            post.Tags.Clear();

            tags = tags ?? string.Empty;
            string[] tagNames = tags.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string tagName in tagNames)
            {
                post.Tags.Add(GetTag(tagName));
            }

            if (!id.HasValue)
            {
                model.Posts.Add(post);
            }
            model.SaveChanges();
            return RedirectToAction("Details", new { id = post.ID });

        }

        [ValidateInput(false)]
        public ActionResult Edit(int? id)
        {
            ViewBag.IsAdmin = IsAdmin;
            Post post = GetPost(id);
            StringBuilder tagList = new StringBuilder();
            foreach( Tag tag in post.Tags)
            {
                tagList.AppendFormat("{0} ", tag.Name);
            }

            ViewBag.Tags = tagList.ToString();
            return View(post);
        }
        */

        public ActionResult NotAdmin()
        {
            return View();
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

        public bool IsAdmin { get {return Session["IsAdmin"] != null && (bool)Session["IsAdmin"]; } }
    }
}
