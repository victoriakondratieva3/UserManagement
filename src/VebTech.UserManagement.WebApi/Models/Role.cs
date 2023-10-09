﻿namespace VebTech.UserManagement.WebApi.Models;

using System.Text.Json.Serialization;

public class Role
{
    public int Id { get; set; }
    public string Name { get; set; }

    [JsonIgnore]
    public List<User> Users { get; set; } = new();
}