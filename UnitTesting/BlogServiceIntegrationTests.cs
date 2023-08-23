using API.Interfaces;
using API.Models.RequestModels;
using API.Services;
using AutoMapper;
using Azure;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace UnitTesting
{
    public class BlogServiceIntegrationTests : IDisposable
    {
        private readonly BlogService _blogService;
        private readonly AppDbContext _context;
        private readonly IDbContextTransaction _transaction; // Add this field

        public BlogServiceIntegrationTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer("Server=localhost;Database=iAPSBlogManagementSystem;Trusted_Connection=True;TrustServerCertificate=True")
                .Options;

            _context = new AppDbContext(options);
            _transaction = _context.Database.BeginTransaction();

            var mapper = new MapperConfiguration(mc => { mc.AddProfile(new MapperProfile()); }).CreateMapper();
            _blogService = new BlogService(_context, mapper);
        }

        public void Dispose()
        {
            _transaction.Rollback();
            _transaction.Dispose();
            _context.Dispose();
        }

        [Fact]
        public async Task GetBlogPostsAsync_ReturnsBlogs()
        {
            var result = await _blogService.GetBlogPostsAsync();

            Assert.NotNull(result);
            Assert.NotEmpty(result.Data);
        }

        [Fact]
        public async Task GetBlogByIdAsync_ReturnsBlog()
        {
            var blogId = Guid.Parse("F88519DC-DCFF-479A-97FB-08DBA4056B0B");
            var result = await _blogService.GetBlogByIdAsync(blogId);

            Assert.NotNull(result);
            Assert.Equal(blogId, result.Id);
        }
    }
}
