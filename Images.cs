using HtmlAgilityPack;
using System.Drawing;

namespace ImageWebScrapingExample
{
    public static class Images
    {
        public static async Task DownloadImages(string outputFolder, string url, string includeFilter)
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

                    string filename = Path.Combine(outputFolder, outputFolder + "-page" + page.ToString().PadLeft(3, '0') + ".jpeg");
                    File.WriteAllBytes(filename, imageData);
                    Console.WriteLine($"Saved {filename}");
                    page++;
                }
            }
        }
    }
}
