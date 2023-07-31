namespace VirtualWallet.Web.Helper.Contracts
{
    public interface IImageManager
    {
        IFormFile GeneratePlaceholderAvatar(String firstName, String lastName);
        string UploadGeneratedProfilePicInRoot(IFormFile generatedPic);

        string UploadOriginalProfilePicInRoot(IFormFile originalPic);

        bool DeleteProfilePicFromRoot(string fileName);
    }
}
