import userAgent from '../../utils/UserAgent';
import { LoginDTO } from '../../utils/DTOs/LoginDTO';
import { useForm } from 'react-hook-form';
import { yupResolver } from '@hookform/resolvers/yup';
import * as Yup from "yup";
import { Link, useNavigate } from 'react-router-dom';
import { observer } from 'mobx-react-lite';
import { useStore } from '../../utils/state_mgmt/Store';




const loginSchema = Yup.object().shape({
    email: Yup.string()
        .required('Email is required')
        .email('Invalid email address'),
    password: Yup.string()
        // .min(8, 'Password must be at least 8 characters')
        // .max(50, 'Password cannot be more than 50 characters')
        // .matches(
        // /^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%^&*])(?=.{8,})/,
        // 'Password must contain at least one uppercase letter, one lowercase letter, one number and one special character')
});


export default observer(function LoginOrRegister() {

    const { UserRequests } = userAgent;
    const { userStore } = useStore();
    const navigate = useNavigate();

    const { register, handleSubmit, formState: { errors } } = useForm<LoginDTO>({
        resolver: yupResolver(loginSchema)
    })

    const submitForm = async (loginDto: LoginDTO) => {

      await UserRequests.login(loginDto)
            .then((e) => {
              userStore.loggedInUser = e.data;
              navigate('/Home')
            })
            .catch((e) => {
              alert("Account doesn't exist")
              window.location.reload()
            })

          }


    return (
        <div className="flex flex-col items-center justify-center min-h-screen bg-gray-400">
          <div className="w-full max-w-md p-9 bg-white rounded-md shadow-md">
            <h2 className="text-giant font-big text-gray-900 mb-6 flex flex-col items-center justify-center">
              Login</h2>
            <form className='pt-1' onSubmit={handleSubmit(submitForm)}>
              <div className="mb-4">
                <label className="block text-gray-700 font-medium mb-2">
                  Email
                </label>
                <input className="w-full p-2 border border-gray-400 rounded-md focus:outline-none focus:border-blue-500"
                  type="email"
                  placeholder="Enter your email"
                  {...register("email")}
                />
                {errors.email && <span>{errors.email.message}</span>}
              </div>

              <div className="mb-4 pt-1">
                <label className="block text-gray-700 font-medium mb-2">
                  Password
                </label>
                <input className="w-full p-2 border border-gray-400 rounded-md focus:outline-none focus:border-blue-500"
                  type="password"
                  placeholder="Enter your password"
                  {...register("password")}
                />
                {errors.password && <span>{errors.password.message}</span>}
              </div>

              <div className='pt-2 '>
                <button className="bg-blue-500 text-white py-2 px-4 rounded-md hover:bg-blue-600 transition duration-200" type="submit"
                  >
                      Login
                  </button>

              </div>
            </form>

            <hr className="my-6 border-gray-300 w-full" />
            <button
              className="bg-gray-300 text-gray-700 py-2 px-4 rounded-md hover:bg-gray-400 transition duration-200"
            >
              <Link to="/CreateAccount" style={{ color: 'black' }}>Create Account</Link>
            </button>
          </div>
        </div>
      );
    })
 