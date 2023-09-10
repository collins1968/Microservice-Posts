using AutoMapper;
using Post_Service.Model;
using Post_Service.Model.Dto;

namespace Post_Service.Profiles
{
    public class PostProfiles: Profile
    {
        public PostProfiles()
        {
            CreateMap<AddPost, Post>().ReverseMap();
            CreateMap<Post, PostResponse>().ReverseMap();
        }
    }
}
