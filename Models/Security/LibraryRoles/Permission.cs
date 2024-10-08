﻿using Newtonsoft.Json;

namespace SardCoreAPI.Models.Security.LibraryRoles
{
    public class Permission
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public List<Permission> Children
        {
            get
            {
                List<Permission> list = new List<Permission>();
                foreach (var key in ChildrenDictionary.Keys)
                {
                    list.Add(ChildrenDictionary.GetValueOrDefault(key)!);
                }
                return list;
            }
        }
        [JsonIgnore]
        public Dictionary<string, Permission> ChildrenDictionary { get; set; } = new Dictionary<string, Permission>();

        public Permission() { }
        public Permission(string id)
        {
            Id = id;
        }
        public Permission(string id, string description)
        {
            Id = id;
            Description = description;
        }
    }
}
