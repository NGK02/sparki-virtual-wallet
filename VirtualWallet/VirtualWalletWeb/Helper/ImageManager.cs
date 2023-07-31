using Avatarator;
using Microsoft.AspNetCore.Hosting;
using System.Drawing;
using System.Drawing.Imaging;
using VirtualWallet.Web.Helper.Contracts;

namespace VirtualWallet.Web.Helper
{
    public class ImageManager : IImageManager
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        public ImageManager(IWebHostEnvironment webHostEnvironment)
        {
            this.webHostEnvironment = webHostEnvironment;
        }
        // Generates a FormFile with the initials of the user.
        public IFormFile GeneratePlaceholderAvatar(String firstName, String lastName)
        {
            var avatarString = string.Format("{0}{1}", firstName[0], lastName[0]).ToUpper();

            var config = new AvataratorConfig();
            var generator = new AbbreviationGenerator(config);
            var image = generator.Generate(avatarString, 150, 150);

            // Load RGB array into MemoryStream
            MemoryStream rgbMemoryStream = new MemoryStream(image);
            // Create bitmap object from rgbMemoryStream
            Bitmap bitmap = new Bitmap(rgbMemoryStream);
            // Convert rgbMemoryStream to pngMemoryStream
            MemoryStream pngMemoryStream = new MemoryStream();
            bitmap.Save(pngMemoryStream, ImageFormat.Png);
            // Convert pngMemoryStream to FormFile
            return CreateTemporaryFile(avatarString, "image/png", pngMemoryStream);
        }

        private static IFormFile CreateTemporaryFile(string fileName, string contentType, Stream stream)
        {
            var formFile = new FormFile(
                baseStream: stream,
                baseStreamOffset: 0,
                length: stream.Length,
                name: "Data",
                fileName: fileName)
            {
                Headers = new HeaderDictionary()
            };

            formFile.ContentType = contentType;
            return formFile;
        }

        public string UploadGeneratedProfilePicInRoot(IFormFile generatedPic)
        {
            string folder = "Assets/GeneratedProfilePics/";
            folder += Guid.NewGuid().ToString() + "_" + generatedPic.FileName + ".png";


            string serverFolder = Path.Combine(webHostEnvironment.WebRootPath, folder);

            using (FileStream fileStream = new FileStream(serverFolder, FileMode.Create))
            {
                generatedPic.CopyToAsync(fileStream);
            }
            return "/" + folder;
        }

        public string UploadOriginalProfilePicInRoot(IFormFile originalPic)
        {
            string folder = "Assets/originalProfilePics/";
            folder += Guid.NewGuid().ToString() + "_" + originalPic.FileName;


            string serverFolder = Path.Combine(webHostEnvironment.WebRootPath, folder);

            using (FileStream fileStream = new FileStream(serverFolder, FileMode.Create))
            {
                originalPic.CopyToAsync(fileStream);
            }
            return "/" + folder;
        }
    }
}
