using AuthService.Data;
using AuthService.Models.RequestsDto;
using AuthService.Models.ResponseDto;
using AuthService.Services.IServices;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace AuthService.Services
{
    public class UserService : IUserInterface
    {
        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly AppDbContext _appDbContext;
        private readonly IHttpClientFactory _clientFactory;

        public UserService(IMapper mapper, UserManager<IdentityUser> userManager, ITokenGenerator tokenGenerator, AppDbContext appDbContext, IHttpClientFactory clientFactory)
        {
            _mapper = mapper;
            _userManager = userManager;
            _tokenGenerator = tokenGenerator;
            _appDbContext = appDbContext;
            _clientFactory = clientFactory;
        }


        public async Task<string> LoginUser(LoginDto loginDto)
        {
           var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.Email == loginDto.Email);
            if (user == null)
            {
                throw new Exception("User not found!");
              }
            else
            {
                var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);
                if (result)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    return _tokenGenerator.GenerateToken(user, roles);
                }
                else
                {
                    throw new Exception("Invalid login credentials!");
                }
            }
        }

        public async Task<string> RegisterUser(RegistrationDto registrationDto)
        {
            var user = _mapper.Map<IdentityUser>(registrationDto);
            try
            {
                var result = await _userManager.CreateAsync(user, registrationDto.Password);
                if (result.Succeeded)
                {
                    return "User created successfully!";
                }
                else
                {
                    return result.Errors.FirstOrDefault().Description;
                }
            }
            catch (Exception e)
            {
               
                throw e;
            }

        }
        public async Task<IEnumerable<PostDto>> GetPostsOfThisUser()
        {
            try
            {
                var client = _clientFactory.CreateClient("Posts");
                var response = await client.GetAsync("api/Post");
                var content = await response.Content.ReadAsStringAsync();
                var posts = JsonConvert.DeserializeObject<responsedto>(content);
             if(posts.IsSuccess)
                {
                    return JsonConvert.DeserializeObject<IEnumerable<PostDto>>(posts.Result.ToString());
                }
                else
                {
                    throw new Exception("Unable to get posts!");
                }


            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
