using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.VisualBasic.FileIO;


namespace ImgScraper
{
    class Program
    {
        static List<csvDetails> list;
        static csvDetails CSVDetails;

        static void Main(string[] args)
        {

            var sheet = GetSheetInfo("LeeMarPet.com_Inventory_06_04_20.csv");

            var logPath = System.IO.Path.GetTempFileName();
            var logFile = System.IO.File.Create(logPath);
            //var logWriter = new System.IO.StreamWriter(logFile);

            foreach (var item in sheet)
            {
                //item.thumb =
                //var webInfo = GetNodesFromSearch(item.Item);   
                //item.thumb = ParseThumbnail(webInfo);
                //item.full = GetImageFromDetail(ProdDetailLink(webInfo));
                //logWriter.WriteLine(item.ToString());
                Console.WriteLine(string.Format("{0},{1}", item.thumb, item.full));
            }

            
            
            //logWriter.Dispose();

            //Console.Write();
            //GetSearchHtmlAsync("5590");
            //dailyinventory060420e
        }

        static List<csvDetails> GetSheetInfo(string fileName)
        {
            list = new List<csvDetails>();

            string[,] values = LoadCSV(fileName);
            int num_rows = values.GetUpperBound(0) + 1;

            for (int r = 1; r < num_rows; r++)
            {
                CSVDetails = new csvDetails(values[r, 0], values[r, 1], values[r, 2], values[r, 3], values[r, 4], values[r, 5], values[r, 6], values[r, 7], values[r, 8], values[r, 9], values[r, 10], values[r, 11], values[r, 12], values[r, 13], values[r, 14], values[r, 15], values[r, 16], values[r, 17]);
                list.Add(CSVDetails);
            }
            return list;
        }

        static HtmlNode GetSearchHtmlAsync(string prodId)
        {
            var url = string.Format("http://www.leemarpet.com/searchx/0/0/1/1/?s={0}&g=Search", prodId);
            var httpclient = new HttpClient();
            var html = httpclient.GetStringAsync(url);
            HtmlNode specNode = GetNodesFromSearch(html.Result);
            return specNode;
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

        private static string GetImageFromDetail(string detailUrl)
        {
            
            var httpclient = new HttpClient();
            if (detailUrl == "No detail found")
            {
                return "No Image Found";
            }
            var html = httpclient.GetStringAsync(detailUrl).Result;
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            HtmlNode specificNode = doc.GetElementbyId("main_imagephoto");
            var img = specificNode.SelectSingleNode("//a[@id='main_image_large']").Attributes["href"].Value;
            //Console.Write(img);
            return img;
        }

        private static string ParseThumbnail(HtmlNode htmlnode)
        {
            string retVal = "No Image Found";
            try
            {
                retVal = htmlnode.SelectSingleNode("//img").Attributes["src"].Value;
            }
            catch (Exception ex)
            {
            //nothing
            }
            
            //Console.WriteLine(tImg);
            return retVal;
        }

        private static string ProdDetailLink(HtmlNode htmlnode)
        {
            string detailUri = "No detail found";

            try
            {
                detailUri = htmlnode.SelectSingleNode("//a[@class='product_link']").Attributes["href"].Value;
            }
            catch (Exception ex)
            {
                //nothing
            }
            //Console.WriteLine(detailUri);
            return detailUri;
        }

        private static string[,] LoadCSV(string filename)
        {
            // Get the file's text.
            string whole_file = System.IO.File.ReadAllText(filename);

            // Split into lines.
            whole_file = whole_file.Replace('\n', '\r');
            string[] lines = whole_file.Split(new char[] { '\r' },
                StringSplitOptions.RemoveEmptyEntries);

            // See how many rows and columns there are.
            int num_rows = lines.Length;
            int num_cols = lines[0].Split(',').Length;

            // Allocate the data array.
            string[,] values = new string[num_rows, num_cols];

            // Load the array.
            for (int r = 0; r < num_rows; r++)
            {
                string[] line_r = lines[r].Split(',');
                for (int c = 0; c < num_cols; c++)
                {
                    values[r, c] = line_r[c];
                }
            }

            // Return the values.
            return values;
        }
    }

    public class csvDetails
    {
        public string Item { get; set; }
        public string MfgPart { get; set; }
        public string UPC { get; set; }
        public string PetType { get; set; }
        public string Price { get; set; }
        public string MfgMap { get; set; }
        public string Amazon { get; set; }
        public string Walmart { get; set; }
        public string Ebay { get; set; }
        public string Status { get; set; }
        public string QtyAvailable { get; set; }
        public string Weight { get; set; }
        public string Length { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
        public string Created { get; set; }
        public string thumb { get; set; }
        public string full { get; set; }

        public csvDetails(string _Item, string _MfgPart, string _UPC, string _PetType, string _Price, string _MfgMap, string _Amazon, string _Walmart, string _Ebay, string _Status, string _QtyAvailable, string _Weight, string _Length, string _Width, string _Height, string _Created, string _thumb, string _full)
        {
            Item = _Item;
            MfgPart = _MfgPart;
            Item = _Item;
            MfgPart = _MfgPart;
            UPC = _UPC;
            PetType = _PetType;
            Price = _Price;
            MfgMap = _MfgMap;
            Amazon = _Amazon;
            Walmart = _Walmart;
            Ebay = _Ebay;
            Status = _Status;
            QtyAvailable = _QtyAvailable;
            Weight = _Weight;
            Length = _Length;
            Width = _Width;
            Height = _Height;
            Created = _Created;
            thumb = _thumb;
            full = _full;

        }
    }
}
