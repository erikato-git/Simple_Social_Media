﻿using System.ComponentModel.DataAnnotations;
using webapi.Model;

namespace webapi.DTOs
{
    public class UserDTO
    {
        [Key]
        public Guid UserId { get; set; }
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Full_Name { get; set; } = string.Empty;
        public string? Profile_Picture { get; set; }
        public DateTime? DateOfBirth { get; set; } = DateTime.UtcNow;
        public string? Description { get; set; }
        public ICollection<Post>? Posts { get; set; }
        public ICollection<Comment>? Comments { get; set; }
    }
}
