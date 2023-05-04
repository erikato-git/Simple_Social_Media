using Microsoft.EntityFrameworkCore;
using webapi;
using webapi.Model;

public class DataContext: DbContext
{
    public DataContext(DbContextOptions<DataContext> options): base(options)
    {
    }

    public DbSet<WeatherForecast> WeatherForecasts { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Comment> Comments { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var WeatherForecastId1 = "AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAA1";

        modelBuilder.Entity<WeatherForecast>().HasData(
            new WeatherForecast{
                Id = Guid.Parse(WeatherForecastId1),
                TemperatureC = 25,
                Summary = "Test-object"
            }
        );

                // Relations

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId);

            entity.HasMany(e => e.Posts)
                .WithOne(e => e.User)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasMany(e => e.Comments)
                .WithOne(e => e.User)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.SetNull);

        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(e => e.PostId);

            entity.HasMany(e => e.Comments)
                .WithOne(e => e.Post)
                .HasForeignKey(e => e.PostId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        });


        // Seeding

        var userId1 = "AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAA111";
        var userId2 = "AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAA112";
        var userId3 = "AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAA113";

        var postId1 = "AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAA211";
        var postId2 = "AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAA212";
        var postId3 = "AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAA213";

        var commentId1 = "AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAA311";
        var commentId2 = "AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAA312";
        var commentId3 = "AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAA313";


        modelBuilder.Entity<User>().HasData(

            new User
            {
                UserId = Guid.Parse(userId1),
                Email = "user1@mail.com",
                Password = "123",
                Full_Name = "user 1",
            },

            new User
            {
                UserId = Guid.Parse(userId2),
                Email = "user2@mail.com",
                Password = "123",
                Full_Name = "user 2",
            },

            new User
            {
                UserId = Guid.Parse(userId3),
                Email = "user3@mail.com",
                Password = "123",
                Full_Name = "user 3",
            }
        );


        modelBuilder.Entity<Post>().HasData(

            new Post
            {
                PostId = Guid.Parse(postId1),
                Content = "user 1",
                UserId = Guid.Parse(userId1),
            },
            new Post
            {
                PostId = Guid.Parse(postId2),
                Content = "user 2",
                UserId = Guid.Parse(userId2),
            },
            new Post
            {
                PostId = Guid.Parse(postId3),
                Content = "user 3",
                UserId = Guid.Parse(userId3),
            }
        );


        modelBuilder.Entity<Comment>().HasData(

            new Comment
            {
                CommentId = Guid.Parse(commentId1),
                Content = "comment 1",
                UserId = Guid.Parse(userId1),
                PostId = Guid.Parse(postId1),
            },
            new Comment
            {
                CommentId = Guid.Parse(commentId2),
                Content = "comment 2",
                UserId = Guid.Parse(userId2),
                PostId = Guid.Parse(postId2),
            },
            new Comment
            {
                CommentId = Guid.Parse(commentId3),
                Content = "comment 3",
                UserId = Guid.Parse(userId1),
                PostId = Guid.Parse(postId1),
            }
        );

    }
}
