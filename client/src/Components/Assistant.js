import React from 'react'
import { useEffect } from 'react'
import { useState } from 'react'
import ListOfMessages from './ListOfMessages'
import { Input } from '../UI/Input'
import { Button } from '../UI/Button'
import '../css/main.css'
import axios from 'axios'
import { API_URL } from '../API/ApiConstants';

const Assistant = () => {
    const [messages, setMessages] = useState([])
    const [message, setMessage] = useState("")
    const [isHidden, setIsHidden] = useState(true)

    useEffect(() => {
        getMessages();
    }, [isHidden])

    const handleMessageChange = (e) => {
        e.preventDefault();
        setMessage(e.target.value);
    }

    const sendMessage = async (e) => {
        e.preventDefault();

        setMessages([...messages, {'userName': localStorage.getItem("userName"), 'content': message}]);
        setMessage("");
        await axios.post(API_URL + "/gpt/ask", {
            content: message
        }, {
            headers: {
                'Authorization': `Bearer ${localStorage.getItem("accessToken")}`
            }
        }).then(res => {
            getMessages();
        })
    }

    const getMessages = async () => {
        await axios.get(API_URL + "/gpt", {
            headers: {
                'Authorization': `Bearer ${localStorage.getItem("accessToken")}`
            }
        }).then(res => {
            console.log(res);
            if (res.data.length > 0) {
                setMessages(res.data);
            }
        })
    }

    if (isHidden) {
        return (
            <div className='assistant-button' onClick={() => setIsHidden(!isHidden)}>
                <div className='assistant-button-content'>V</div>
            </div>
        ) 
    } else {
        return (
          <div className='assistant'>
              <div className='assistant-button' onClick={() => setIsHidden(!isHidden)}>
                <div className='assistant-button-content'>X</div>
              </div>
              <ListOfMessages messages={messages}>I'm Assistant.</ListOfMessages>
              <Input 
                  type='text' 
                  nameId='message' 
                  placeholder='Enter the message' 
                  value={message} 
                  handleChange={handleMessageChange}
              ></Input>
              <Button
                  handleClick={sendMessage}
              >Send</Button>
          </div>
        )
    }
}

export default Assistant