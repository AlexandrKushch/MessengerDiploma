import React from 'react'
import axios from 'axios'
import { useNavigate } from 'react-router-dom'
import { Input } from '../UI/Input'
import { Button } from '../UI/Button'
import { useState } from 'react'
import '../css/authorization.css'
import Label from '../UI/Label'

export const Registration = ({navigation}) => {
    const [email, setEmail] = useState("");
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const navigate = useNavigate();

    const handleEmailChange = (e) => {
        setEmail(e.target.value);
    }

    const handleUsernameChange = (e) => {
        setUsername(e.target.value);
    }

    const handlePasswordChange = (e) => {
        setPassword(e.target.value);
    }

    const register = async (e) => {
        e.preventDefault();

        await axios.post("https://localhost:7152/users/", {
            email: email,
            username: username,
            password: password
        }).then(() => {
            navigate("/login")
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
    }

    const navigateToLogin = (e) => {
      e.preventDefault();
      navigate("/login")
    }

  return (
    <div className='auth'>
        <Label>Registration</Label>
        <Input type="email" nameId="email" placeholder="Email" value={email} handleChange={handleEmailChange}></Input>
        <Input type="text" nameId="username" placeholder="Username" value={username} handleChange={handleUsernameChange}></Input>
        <Input type="password" nameId="password" placeholder="Password" value={password} handleChange={handlePasswordChange}></Input>
        <Button handleClick={register}>Register</Button>
        <Button handleClick={navigateToLogin}>Back to Login</Button>
    </div>
  )
}
