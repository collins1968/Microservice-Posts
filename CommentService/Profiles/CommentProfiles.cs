using AutoMapper;
using CommentService.Models;
using CommentService.Models.Dto;

namespace CommentService.Profiles
{
    public class CommentProfiles : Profile
    {
        public CommentProfiles()
        {
           CreateMap<Comment, AddComment>().ReverseMap();
          CreateMap<Comment, UpdateComment>().ReverseMap();
          
        }

    }
}
