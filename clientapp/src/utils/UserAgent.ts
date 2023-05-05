import axios, { AxiosResponse } from "axios";
import { LoginDTO } from "./DTOs/LoginDTO";
import { Post } from "./models/Post";
import { PostCreateDTO } from "./DTOs/PostCreateDTO";
import { PostUpdateDTO } from "./DTOs/PostUpdateDTO";
import { User } from "./models/User";
import { UserCreateDTO } from "./DTOs/UserCreateDTO";
import { UserUpdateDTO } from "./DTOs/UserUpdateDTO";
import { CommentCreateDTO } from "./DTOs/CommentCreateDTO";
import { CommentUpdateDTO } from "./DTOs/CommentUpdateDTO";
import { PasswordChangeDTO } from "./DTOs/PasswordChangeDTO";
import { Comment } from "./models/Comment";


axios.defaults.baseURL = process.env.REACT_APP_BASE_URL;

axios.interceptors.response.use(
    (response) => {
      return response;
    },
    (error) => {
      return Promise.reject(error);
    }
  );

axios.interceptors.request.use((config) => {
  config.withCredentials = true;
  return config;
});


const responseBody = <T> (response: AxiosResponse<T>) => response.data;

// Generic requests from our base-url, we can later attach the particular API-part
const requests = {
    get: <T> (url: string) => axios.get<T>(url).then(responseBody),
    post: <T> (url: string, body: {}) => axios.post<T>(url, body).then(responseBody),
    put: <T> (url: string, body: {}) => axios.put<T>(url, body).then(responseBody),
    delete: <T> (url: string) => axios.delete<T>(url).then(responseBody)
}

// Get domain-specific requests and makes it type-safety
const UserRequests = {
    getAll: () => requests.get<User[]>('/get_all_users'),   // TODO: Use a UserDTO, never expose passwords etc.
    getById: (id: string) => requests.get<User>(`/get_user/${id}`),   // TODO: Use a UserDTO, never expose passwords etc.
    create: (userDto: UserCreateDTO) => requests.post<void>(`/create_user`,userDto),
    update: (id: string, userDTO: UserUpdateDTO) => requests.put<void>(`/update_user/${id}`,userDTO),
    delete: (id: string) => requests.delete<void>(`/delete_user/${id}`),
    login: (loginDto: LoginDTO) => axios.post(`/login`,loginDto),
    logout: () => axios.post(`/logout`),
    getByEmail: (email: string) => axios.get<User>(`/findUserByEmail/${email}`),
    checkEmailIsFree: (email: string) => axios.get<boolean>(`/CheckEmailIsFree/${email}`),
    refreshLoggedInUser: () => axios.get<User>(`/returnLoggedInUserWhileSessionHasntExpired`),
    changePassword: (passwordChangeDTO: PasswordChangeDTO) => axios.post<boolean>(`/changePassword`,passwordChangeDTO)
}

const PostRequests = {
    getAll: () => requests.get<Post[]>('/get_all_posts'),
    getById: (id: string) => requests.get<Post>(`/get_post/${id}`),
    create: (postCreateDTO: PostCreateDTO) => requests.post<Post>(`/create_post`,postCreateDTO),
    update: (id: string, postUpdateDTO: PostUpdateDTO) => requests.put<void>(`/update_post/${id}`,postUpdateDTO),
    delete: (id: string) => requests.delete<string>(`/delete_post/${id}`),
    getPostsByUserid: (id: string) => requests.get<Post[]>(`/GetPostsByUserId/${id}`),
    getCommentsByPostId: (id: string) => requests.get<Comment[]>(`/getCommentsByPostId/${id}`)
}

const CommentRequests = {
    getAll: () => requests.get<Comment[]>('/get_all_comments'),
    getById: (id: string) => requests.get<Comment>(`/get_comment/${id}`),
    create: (commentCreateDTO: CommentCreateDTO) => requests.post<Comment>(`/create_comment`,commentCreateDTO),
    update: (id: string, commentUpdateDTO: CommentUpdateDTO) => requests.put<void>(`/update_comment/${id}`,commentUpdateDTO),
    delete: (id: string) => requests.delete<void>(`/delete_comment/${id}`),
}



const userAgent = {
  UserRequests,
  PostRequests,
  CommentRequests
}

export default userAgent;
