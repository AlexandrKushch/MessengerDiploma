import React from 'react'
import { Input } from '../UI/Input'
import { useState } from 'react'
import '../css/main.css'
import ListOfMessages from './ListOfMessages'
import { Button } from '../UI/Button'
import { useParams } from 'react-router-dom'

const Chat = ({children, connection, messages}) => {
    const [message, setMessage] = useState("")
    const params = useParams()

    const handleMessageChange = (e) => {
        e.preventDefault();
        setMessage(e.target.value);
    }    

    const sendMessage = (e) => {
      e.preventDefault();
      setMessage("");
      let userId = localStorage.getItem("userId");
      let chatId = params.id;
      
      connection.invoke("Send", userId, chatId, {
        content: message
      });
    }

  if (children) {
    return (
      <div className='chat'>
          <div className='chat-empty'>
            {children}
          </div>
      </div>
    )
  }
  else {
    return (
      <div className='chat'>
          <ListOfMessages messages={messages}>Say Hello!</ListOfMessages>
          <div className='chat-input'>
            <Input 
                type='text' 
                nameId='message' 
                placeholder='Enter the message' 
                value={message} 
                handleChange={handleMessageChange}
                style={{'width': '500%'}}
            ></Input>
            <Button
                handleClick={sendMessage}
            >Send</Button>
          </div>
      </div>
    )
  }
}

export default Chat