import React, { useEffect } from 'react'
import Message from './Message'
import '../css/main.css'

const ListOfMessages = ({messages, children}) => {
  const messagesEndRef = React.createRef()

  useEffect(() => {
    messagesEndRef.current?.scrollIntoView();
    // eslint-disable-next-line
  }, [messages])
  

  return (
    <div className='messages'>
        {messages.length > 0 ? messages.map(m => 
            <Message key={m.id} message={m} isMine={m.userName === localStorage.getItem("userName")}></Message>)
          :
          <div className='chat-empty'>
            {children}
          </div>
          }
        <div ref={messagesEndRef} />
    </div>
  )
}

export default ListOfMessages