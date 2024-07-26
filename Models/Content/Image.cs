﻿namespace SardCoreAPI.Models.Content
{
    public class Image
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public long Size { get; set; }
        public DateTime CreationDate { get; set; }
    }
}