using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapi;
using webapi.Model;

namespace webapi_test.utils
{
    public class DbContextMoq
    {
        public async Task<DataContext> Instance()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var fakeDb = new DataContext(options);


            // --- Users ---

            var userId1 = "AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAA111";
            var userId2 = "AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAA112";
            var userId3 = "AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAA113";

            if (await fakeDb.Users.CountAsync() == 0)
            {
                var users = new List<User>()
                {
                    new User()
                    {
                        UserId = Guid.Parse(userId1),
                        Email = "user1@mail.com",
                        Password = "123",
                        Full_Name = "user 1",
                    },
                    new User()
                    {
                        UserId = Guid.Parse(userId2),
                        Email = "user2@mail.com",
                        Password = "123",
                        Full_Name = "user 2",
                    },
                    new User()
                    {
                        UserId = Guid.Parse(userId3),
                        Email = "user3@mail.com",
                        Password = "123",
                        Full_Name = "user 3",
                    }
                };

                foreach (var i in users)
                {
                    fakeDb.Users.Add(i);
                }
            }


            // --- Posts ---

            var postId1 = "AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAA211";
            var postId2 = "AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAA212";
            var postId3 = "AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAA213";


            if (await fakeDb.Posts.CountAsync() == 0)
            {
                var posts = new List<Post>()
                {
                    new Post()
                    {
                        PostId = Guid.Parse(postId1),
                        Content = "user 1",
                        UserId = Guid.Parse(userId1),
                    },
                    new Post()
                    {
                        PostId = Guid.Parse(postId2),
                        Content = "user 2",
                        UserId = Guid.Parse(userId2),
                    },
                    new Post()
                    {
                        PostId = Guid.Parse(postId3),
                        Content = "user 3",
                        UserId = Guid.Parse(userId3),
                    }

                };

                foreach (var i in posts)
                {
                    fakeDb.Posts.Add(i);
                }
            }


            // --- Comments ---

            var commentId1 = "AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAA311";
            var commentId2 = "AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAA312";
            var commentId3 = "AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAA313";

            if (await fakeDb.Comments.CountAsync() == 0)
            {
                var comments = new List<Comment>()
                {
                    new Comment()
                    {
                        CommentId = Guid.Parse(commentId1),
                        Content = "comment 1",
                        UserId = Guid.Parse(userId1),
                        PostId = Guid.Parse(postId1),

                    },
                    new Comment()
                    {
                        CommentId = Guid.Parse(commentId2),
                        Content = "comment 2",
                        UserId = Guid.Parse(userId2),
                        PostId = Guid.Parse(postId2),
                    },
                    new Comment()
                    {
                        CommentId = Guid.Parse(commentId3),
                        Content = "comment 3",
                        UserId = Guid.Parse(userId1),
                        PostId = Guid.Parse(postId1),

                    }
                };

                foreach (var i in comments)
                {
                    fakeDb.Comments.Add(i);
                }
            }


            await fakeDb.SaveChangesAsync();

            return fakeDb;
        }
    }

}