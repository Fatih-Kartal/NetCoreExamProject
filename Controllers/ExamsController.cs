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

namespace NetCoreExamProject.Controllers
{
    public class ExamsController : Controller
    {
        public IActionResult Index()
        {
            var posts = GetLast5Post();
            return View(posts);
        }
        [HttpPost]
        public string CreateExam(Exam exam)
        {
            return "();";
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
                var content = GetContentOfPost(link);
                posts.Add(new Post { Title = title, Content = content, Link = link });
                // /html/body/div[3]/div/div[3]/div/div/div[2]/div[1]/div/div[1]/div[1]/div[1]/div/ul/li[2]/a[2]/h2
            }

            return posts;
        }

        private string GetContentOfPost(string link)
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

            var htmlWeb = new HtmlWeb();
            htmlWeb.OverrideEncoding = Encoding.UTF8;
            var doc = htmlWeb.Load(url);
            return doc;
        }
    }
}
