﻿namespace TaskManagementApp.SharedLibraries.BaseSharedLibraries.SharedDtos
{
    public class AdminUserDto : IEntity
    {
        public Guid Id { get; set; }
        public required string UserName { get; set; }
        [EmailAddress]
        public required string Email { get; set; }
        [Phone]
        public required string PhoneNumber { get; set; }
    }
}
