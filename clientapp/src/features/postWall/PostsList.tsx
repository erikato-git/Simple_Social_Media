import React from 'react'

function PostsList() {

  interface Comment {
    id: number;
    text: string;
  }
  
  interface Post {
    id: number;
    title: string;
    body: string;
    comments: Comment[];
  }
  
  const comments: Comment[] = [
    { id: 1, text: "Hej 1"},
    { id: 2, text: "Hej 2"},
    { id: 3, text: "Hej 3"},
  ]

  const posts: Post[] = [
    {id: 1, title: "title 1", body: "body", comments: comments},
    {id: 2, title: "title 2", body: "body", comments: comments},
    {id: 3, title: "title 3", body: "body", comments: comments}
  ]
  


  return (
    <div>
      <div>PostsList</div>
      <div>
      {posts.map((post) => (
        <li key={post.id}>
          <h2>{post.title}</h2>
          <p>{post.body}</p>
          <ul>
            {post.comments.map((comment) => (
              <li key={comment.id}>{comment.text}</li>
            ))}
          </ul>
        </li>
      ))}
    </div>
    </div>
  )
}

export default PostsList