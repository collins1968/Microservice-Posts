using Post_Service.Model;
using Post_Service.Model.Dto;

namespace Post_Service.Services.IServices
{
    public interface IPostInterface
    {

        //create post
        Task<string> CreatePost(Post post);
        //get all posts
        Task<List<Post>> GetPosts();

        //get post by id
        Task<Post> GetPostById(Guid id);

        //update post
        Task<string> UpdatePost(Post post);

        //delete post
        Task<string> DeletePost(Post post);

    }
}
