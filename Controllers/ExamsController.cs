using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HtmlAgilityPack;
using Microsoft.CodeAnalysis;
using NetCoreExamProject.Models;
using Microsoft.EntityFrameworkCore;

namespace NetCoreExamProject.Controllers
{
    public class ExamsController : Controller
    {
        MyDBContext DB;
        public ExamsController()
        {
            DB = new MyDBContext();
        }
        public IActionResult Index()
        {
            var posts = GetLast5Post();
            return View(posts);
        }
        public IActionResult ExamList()
        {
            var exams = DB.Exams.Select(s => new Exam
            {
                Id = s.Id,
                Post = DB.Posts.Where(w => w.Link == s.PostLink).FirstOrDefault(),
                CreateDate = s.CreateDate,
            }).ToList();
            return View(exams);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public string CreateExam([FromBody] Exam exam)
        {
            try
            {
                exam.CreateDate = DateTime.Now.ToString();
                exam.PostLink = exam.Post.Link;
                var post = DB.Posts.Find(exam.Post.Link);
                if (post == null)
                {
                    exam.Post.Content = GetContentOfPost(exam.Post.Link);
                    DB.Posts.Add(exam.Post);
                }
                else
                {
                    DB.Entry<Post>(post).State = EntityState.Detached;
                }
                DB.Exams.Add(exam);
                DB.SaveChanges();
                return "Operation successfull";
            }
            catch (Exception e)
            {
                return "An error occured";
            }
        }
        [HttpPost]
        public string deleteExam([FromBody] int id)
        {
            try
            {
                DB.Questions.RemoveRange(DB.Questions.Where(x => x.ExamId == id));
                DB.Exams.Remove(new Exam { Id = id });
                DB.SaveChanges();
                return "Operation successfull";
            }
            catch (Exception e)
            {
                return "An error occured";
            }
        }
        public IActionResult SolveExam(int id)
        {
            var exam = DB.Exams.Find(id);
            exam.Post = DB.Posts.Where(w => w.Link == exam.PostLink).FirstOrDefault();
            exam.Questions = DB.Questions.Where(w => w.ExamId == id).ToList();
            return View(exam);
        }
        public JsonResult getExamAnswers([FromBody] int id)
        {
            var Answers = DB.Questions.Where(w => w.ExamId == id).Select(s => s.Answer).ToList();
            return Json(Answers);
        }

        public List<Post> GetLast5Post()
        {
            var posts = new List<Post>(5);
            var document = GetHtmlDocument("https://www.wired.com");
            HtmlNodeCollection cardComponents = document.DocumentNode.SelectNodes("//div[contains(@class, 'card-component ')]");
            for (int i = 0; i < 5; i++)
            {
                var title = document.DocumentNode.SelectSingleNode(cardComponents[i].XPath + "/div[1]/ul[1]/li[2]/a[2]/h2").InnerText;
                var link = document.DocumentNode.SelectSingleNode(cardComponents[i].XPath + "/div[1]/ul[1]/li[2]/a[2]").Attributes["href"].Value;
                posts.Add(new Post { Title = title, Link = link });
            }

            return posts;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public string GetContentOfPost([FromBody] string link)
        {
            var document = GetHtmlDocument("https://www.wired.com" + link);
            var xPath = "//*[@id='main-content']/article/div[2]/div/div[1]/div[1]/div[1]";
            HtmlNodeCollection paragraphs = document.DocumentNode.SelectNodes(xPath);
            return paragraphs[0].InnerHtml;
        }

        public HtmlDocument GetHtmlDocument(string url)
        {
            Uri uri = new Uri(url);
            var client = new WebClient();
            client.Encoding = Encoding.UTF8;
            var html = client.DownloadString(uri);
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(html);
            return document;

            //var htmlWeb = new HtmlWeb();
            //htmlWeb.OverrideEncoding = Encoding.UTF8;
            //var doc = htmlWeb.Load(url);
            //return doc;
        }
    }
}
