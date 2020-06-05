using System;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace ImgScraper
{
    class Program
    {
        static void Main(string[] args)
        {
            GetSearchHtmlAsync("5590");
        }

        static void GetSearchHtmlAsync(string prodId)
        {
            var url = string.Format("http://www.leemarpet.com/searchx/0/0/1/1/?s={0}&g=Search", prodId);
            var httpclient = new HttpClient();
            var html = httpclient.GetStringAsync(url);
            HtmlNode specNode = GetNodesFromSearch(html.Result);
            ParseThumbnail(specNode);
            GetImageFromDetail(ProdDetailLink(specNode));
        }

        //private async Task<string> GetHtmlAsync(string url)
        //{
        //    var httpclient = new HttpClient();
        //    var html = await httpclient.GetStringAsync(url);
        //    return html;
        //}

        private static HtmlNode GetNodesFromSearch(string html)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            //we are expecting only one result since we are searching by item id
            HtmlNode specificNode = doc.GetElementbyId("products");
            //return specificNode.InnerHtml.ToString();
            //This node contains the thumbnail image link and the product detail link
            return specificNode;
        }

        private static void GetImageFromDetail(string detailUrl)
        {
            
            var httpclient = new HttpClient();
            var html = httpclient.GetStringAsync(detailUrl).Result;
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            HtmlNode specificNode = doc.GetElementbyId("main_imagephoto");
            var img = specificNode.SelectSingleNode("//a[@id='main_image_large']").Attributes["href"].Value;
            Console.Write(img);
        }

        private static void ParseThumbnail(HtmlNode htmlnode)
        {
            var tImg = htmlnode.SelectSingleNode("//img").Attributes["src"].Value;
            Console.WriteLine(tImg);
        }

        private static string ProdDetailLink(HtmlNode htmlnode)
        {
            var detailUri = htmlnode.SelectSingleNode("//a[@class='product_link']").Attributes["href"].Value;

            //Console.WriteLine(detailUri);
            return detailUri;
        }
    }
}
