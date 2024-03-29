﻿using SardCoreAPI.Utility.Files;

namespace SardCoreAPI.Models.Content
{
    public class ImagePostRequest : ImageRequest
    {
        public IFormFile? Data { get; set; }

        public async Task<byte[]> GetByteArray()
        {
            byte[] bytes = await new FileHandler().FormToByteArray(Data);
            return bytes;
        }
    }
}
