using AuthService.Models.RequestsDto;
using AuthService.Models.ResponseDto;

namespace AuthService.Services.IServices
{
    public interface IUserInterface
    {
        Task<string> RegisterUser(RegistrationDto registrationDto);

        Task<string> LoginUser(LoginDto loginDto);

        //get all post of this user
        Task<IEnumerable<PostDto>> GetPostsOfThisUser();
    }
}
