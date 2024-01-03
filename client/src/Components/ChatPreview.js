import React from 'react'
import '../css/main.css'
import { useNavigate } from 'react-router-dom'

const ChatPreview = ({ chat }) => {

  const navigate = useNavigate();

  const handleClick = (e) => {
    e.preventDefault();
    navigate("/main/" + chat.id);
    chat.new = false;
  }

  return (
    <div className='chat-preview' onClick={handleClick}> 
      {chat.name}
      {chat.new &&
        <div className='chat-preview-dot' />
      }
    </div>
  )
}

export default ChatPreview