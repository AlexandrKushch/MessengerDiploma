import React from 'react'
import ChatPreview from './ChatPreview'
import '../css/main.css'

const ListOfChats = ({chats}) => {
  return (
    <div className='list'>
        {chats.map(chat => 
            <ChatPreview className='chat' chat={chat} key={chat.id}></ChatPreview>)}
    </div>
  )
}

export default ListOfChats