import React from 'react'
import axios from 'axios'
import { useNavigate } from 'react-router-dom'
import { useState } from 'react'
import {Button} from '../UI/Button'
import {Input} from '../UI/Input'
import '../css/authorization.css'
import Label from '../UI/Label'

export const SignInPage = () => {
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const navigate = useNavigate();

    const login = async (e) => {
        e.preventDefault();
      
        await axios.post("https://localhost:7152/users/login", {
            email: username,
            password: password
        }).then(response => {
            localStorage.setItem("userId", response.data.id);
            localStorage.setItem("accessToken", response.data.token);
            localStorage.setItem("userName", response.data.userName);
        }).catch(function (error) {
            if (error.response) {
              console.log(error.response.data);
              console.log(error.response.status);
              console.log(error.response.headers);
            } else if (error.request) {
              console.log(error.request);
            } else {
              console.log('Error', error.message);
            }
        });
        
        navigate("/main")
    }

    const navigateToRegistration = (e) => {
      e.preventDefault();
      navigate("/registration")
    }

    const handleUsernameChange = (e) => {
        setUsername(e.target.value);
    }

    const handlePasswordChange = (e) => {
        setPassword(e.target.value);
    }

  return (
    <div className='auth'>
        <Label>Login</Label>
        <Input type="text" nameId="username" placeholder="Email Or Username" value={username} handleChange={handleUsernameChange} />
        <Input type="password" nameId="password" placeholder="Password" value={password} handleChange={handlePasswordChange} />
        <Button handleClick={login}>Login</Button>
        <Button handleClick={navigateToRegistration} >Registration</Button>
    </div>
  )
}
