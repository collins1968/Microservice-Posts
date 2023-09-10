namespace CommentService.Models.Dto
{
    public class AddComment
    {
        public string CommentText { get; set; } = string.Empty;
        public Guid PostId { get; set; }
    }
}
