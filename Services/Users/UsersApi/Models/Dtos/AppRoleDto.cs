﻿namespace TaskManagementApp.Services.UsersApi.Models.Dtos
{
    public class AppRoleDto : IdentityRole<Guid>, IEntity
    {
        [StringLength(30)]
        public string? Description { get; set; }
    }
}
