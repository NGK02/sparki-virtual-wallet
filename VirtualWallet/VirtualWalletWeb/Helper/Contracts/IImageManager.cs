namespace VirtualWallet.Web.Helper.Contracts
{
    public interface IImageManager
    {
        IFormFile GeneratePlaceholderAvatar(String firstName, String lastName);
		Task<string> UploadGeneratedProfilePicInRoot(IFormFile generatedPic);

        Task<string> UploadOriginalProfilePicInRoot(IFormFile originalPic);

        bool DeleteProfilePicFromRoot(string fileName);
    }
}
