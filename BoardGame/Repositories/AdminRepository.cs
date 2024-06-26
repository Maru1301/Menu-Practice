﻿using BoardGame.Models.EFModels;
using BoardGame.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BoardGame.Repositories
{
    public class AdminRepository(AppDbContext dbContext) : IAdminRepository
    {
        private readonly AppDbContext _db = dbContext;

        public async Task<Admin?> GetByIdAsync(ObjectId id)
        {
            return await _db.Admins.FindAsync(id);
        }

        public async Task<IEnumerable<Admin>> GetAllAsync()
        {
            return await _db.Admins.ToListAsync();
        }

        public async Task<ObjectId> AddAsync(Admin entity)
        {
            var entry = await _db.Admins.AddAsync(entity);
            await _db.SaveChangesAsync();

            return entry.Entity.Id;
        }

        public async Task UpdateAsync(Admin entity)
        {
            _db.Admins.Update(entity);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(ObjectId id)
        {
            var entity = await _db.Admins.FindAsync(id);
            if (entity != null)
            {
                _db.Admins.Remove(entity);
                await _db.SaveChangesAsync();
            }
        }

        public async Task<Admin?> GetByAccountAsync(string account)
        {
            var admin = await _db.Admins.FirstOrDefaultAsync(admin => admin.Account == account);

            return admin;
        }
    }
}
