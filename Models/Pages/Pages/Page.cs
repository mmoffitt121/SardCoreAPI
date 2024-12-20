﻿using SardCoreAPI.Attributes.Easy;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SardCoreAPI.Models.Pages.Pages
{
    public class Page
    {
        public string? Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Path { get; set; }

        public string HeaderSettings { get; set; }

        [NotMapped]
        public PageElement? Root { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        [JsonIgnore]
        public string? PageData 
        { 
            get 
            {
                return JsonSerializer.Serialize(Root);
            } 
            set
            {
                Root = !string.IsNullOrEmpty(value) ? JsonSerializer.Deserialize<PageElement>(value)! : new PageElement();
            }
        }
    }
}
