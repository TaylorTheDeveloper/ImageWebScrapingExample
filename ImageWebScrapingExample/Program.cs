namespace ImageWebScrapingExample
{
    internal class Program
    {
        // This educational example shows how one might use HTMLAgilityPack to automate the aquisition of image content from a web page and assemble a PDF using C#.
        static async Task Main(string[] args)
        {
            // Filering images is really useful and how you filter images will depend on the site, and your collection use case, etc.
            // In this case, I am filtering using the expected hostname for the image CDN. You can filter based of this, the file name, the image size, etc.
            string includeFilter = "2.bp.blogspot.com";
            string title = "futurama-comics";
            string host = "https://viewcomics.me/";

            int issues = 1;

            for (int i = 1; i < issues + 1; i++)
            {
                string issue = "issue-" + i.ToString();
                string endpoint = host + title +"/" + issue + "/full";
                await Images.DownloadImages(issue, endpoint, includeFilter);
                PDF.CreatePdfFromImages(title, issue);
            }            
        }
    }
}