using HtmlAgilityPack;
using System.Drawing;

namespace ComicRebuilder
{
    internal class Program
    {
        // This example shows how one might use HTMLAgilityPack to automate the aquisition of image content from a web page.
        static async Task Main(string[] args)
        {
            string includeFilter = "2.bp.blogspot.com";

            for (int i=1; i<84; i++)
            {
                string issue = "issue-" + i.ToString();
                string endpoint = "https://viewcomics.me/futurama-comics/" + issue + "/full";
                await DownLoadComic(issue, endpoint, includeFilter);
            }            
        }

        static async Task DownLoadComic(string outputFolder, string url, string includeFilter)
        {
            Directory.CreateDirectory(outputFolder);
            var web = new HtmlWeb();
            var doc = await web.LoadFromWebAsync(url);
            int page = 1;

            var httpClient = new HttpClient();

            foreach (var link in doc.DocumentNode.SelectNodes("//img[@src]"))
            {
                string imageUrl = link.GetAttributeValue("src", null);
                if (!Uri.IsWellFormedUriString(imageUrl, UriKind.Absolute))
                    imageUrl = new Uri(new Uri(url), imageUrl).AbsoluteUri;

                if (!imageUrl.Contains(includeFilter))
                    continue;

                var imageData = await httpClient.GetByteArrayAsync(imageUrl);
                using (var ms = new MemoryStream(imageData))
                {
                    var image = Image.FromStream(ms);

                    string filename = Path.Combine(outputFolder, outputFolder + "-page" + page.ToString() + ".jpeg");
                    File.WriteAllBytes(filename, imageData);
                    Console.WriteLine($"Saved {filename}");
                    page++;
                }
            }
        }
    }
}