using iTextSharp.text;
using iTextSharp.text.pdf;

namespace ImageWebScrapingExample
{
    public static class PDF
    {
        public static void CreatePdfFromImages(string title, string folder)
        {
            string[] imageFiles = Directory.GetFiles(folder, "*.jpeg");
            Array.Sort(imageFiles);

            using (var stream = new FileStream(Path.Combine(folder, title + "-" + folder + ".pdf"), FileMode.Create, FileAccess.Write, FileShare.None))
            {
                var document = new iTextSharp.text.Document();

                var writer = PdfWriter.GetInstance(document, stream);
                document.Open();

                foreach (var imageFile in imageFiles)
                {
                    var image = Image.GetInstance(imageFile);
                    document.SetPageSize(new Rectangle(0, 0, image.Width, image.Height, 0));
                    document.NewPage();
                    image.SetAbsolutePosition(0, 0);
                    writer.DirectContent.AddImage(image);
                }

                document.Close();
            }
        }
    }
}
