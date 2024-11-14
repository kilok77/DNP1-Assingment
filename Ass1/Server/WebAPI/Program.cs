using Entities;
using FileRepositories;
using Microsoft.AspNetCore.Identity;
using RepositoryContracts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IPostRepository, PostFileRepository>();
builder.Services.AddScoped<IUserRepository, UserFileRepository>();
builder.Services.AddScoped<ICommentRepository, CommentFileRepository>();
builder.Services.AddScoped<IVoteRepository, VoteFileRepository>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();


// Add logging to help diagnose issues
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var app = builder.Build();

app.MapControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();
// app.UseStaticFiles();
//
app.UseRouting();
//
// app.UseAuthorization();
//
// app.MapRazorPages();

app.Run();