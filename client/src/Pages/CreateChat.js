import React from 'react'
import Label from '../UI/Label'
import { Input } from '../UI/Input'
import { Button } from '../UI/Button'
import { useEffect } from 'react'
import { useState } from 'react'
import axios from 'axios'
import { API_URL } from '../API/ApiConstants'
import CheckBoxList from '../UI/CheckBoxList'
import '../css/create-chat.css'
import { useNavigate } from 'react-router-dom'

const CreateChat = ({connection}) => {
    const [nameOfChat, setNameOfChat] = useState("")
    const [users, setUsers] = useState([])
    const [checkedUsers, setCheckedUsers] = useState([])

    const navigate = useNavigate();

    useEffect(() => {
      getUsers();
      // eslint-disable-next-line
    }, [])
    
    const getUsers = async () => {
        var token = localStorage.getItem("accessToken");
        await axios.get(API_URL + "/users", {
            headers: {
                'Authorization': `Bearer ${token}`
            }
          })
          .then(res => {
            setUsers(res.data);
          })
    }

    const handleNameChange = (e) => {
        e.preventDefault();
        setNameOfChat(e.target.value);
    }

    const create = async (e) => {
        e.preventDefault();

        if (nameOfChat !== "") {
            checkedUsers.push({
                id: localStorage.getItem("userId"),
                userName: localStorage.getItem("userName")
            })

            console.log(nameOfChat);
            console.log(checkedUsers);
            
            var token = localStorage.getItem("accessToken");
            console.log(token);
            await axios.post(API_URL + "/chats", {
                name: nameOfChat,
            }, {
                headers: {
                    'Authorization': `Bearer ${token}`
                }
            }).then (res => {
                console.log("chat", res.data);
                checkedUsers.forEach(user => {
                    connection.invoke("AddToGroupAsync", res.data.id, user.id);
                });

                navigate("/main");
            })            
        } else {
            console.log("Name is null");
        }
    }

    const addUserToChecked = (e, user) => {
        e.preventDefault();

        if (checkedUsers.includes(user))
        {
            let filtered = checkedUsers.filter(item => item !== user);
            setCheckedUsers(filtered);
        } else {
            setCheckedUsers([...checkedUsers, user])
        }
    }

  return (
    <div className='create-chat'>
        <Label>Create Chat</Label>
        <Input type="text" nameId="username" placeholder="Name of chat" value={nameOfChat} handleChange={handleNameChange} />
        <CheckBoxList elements={users} checkedElements={checkedUsers} handleClick={addUserToChecked}></CheckBoxList>
        <Button handleClick={create}>Create</Button>
    </div>
  )
}

export default CreateChat