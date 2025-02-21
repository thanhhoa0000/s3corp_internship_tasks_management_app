﻿using System.Linq.Expressions;

namespace TaskManagementApp.Services.AuthenticationApi.Repositories.IRepositories
{
    public interface IAuthRepository : IDisposable
    {
        Task<AppUser> GetUserAsync(Expression<Func<AppUser, bool>>? filter = null, bool tracked = true);
        Task<RefreshToken> GetRefreshTokenAsync(
            Expression<Func<RefreshToken, bool>>? filter = null, 
            Func<IQueryable<RefreshToken>, IQueryable<RefreshToken>>? include = null, 
            bool tracked = true);
        
        Task CreateRefreshTokenAsync(RefreshToken refreshToken);
        Task UpdateRefreshTokenAsync(RefreshToken refreshToken);
    }
}
