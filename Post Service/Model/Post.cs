﻿namespace Post_Service.Model
{
    public class Post
    {
        public Guid Id { get; set; }
        public string Title { get; set; }

        public string Description { get; set; }

        public string UserId { get; set; }

        
    }
}
