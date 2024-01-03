import React, { useState } from 'react'
import '../css/chat-members.css'
import Label from '../UI/Label'
import { useLocation, useNavigate, useParams } from 'react-router-dom'
import { Input } from '../UI/Input'
import { Button } from '../UI/Button'
import { useEffect } from 'react'

const ChatMembers = () => {
    const params = useParams();
    const location = useLocation();
    const navigate = useNavigate();

    const [language, setLanguage] = useState("")

    const [users, setUsers] = useState([])
    const [chat, setChat] = useState()

    useEffect(() => {
      setUsers(location.state.users)
      setChat(location.state.chats.find(c => c.id === params.id))
      setLanguage(location.state.chats.find(c => c.id === params.id).language)

      // eslint-disable-next-line
    }, [params.id])
    

    const handleLanguageChange = (e) => {
      e.preventDefault();
      setLanguage(e.target.value)
    }

    const changeLanguage = (e) => {
      e.preventDefault();
      navigate(`/main/${params.id}`, {state: {language: language}})
    }

  return (
    <div className='chat-members'>
        <Label>Members of chat: {chat?.name}</Label>
        <div className='chat-members-list'>
            {users?.map(u => 
                <div className='member' style={{'color': localStorage.getItem("userName") === u.userName ? '#bb86ce' : ''}} key={u.id}>{u.userName}</div>
            )}
        </div>
        <div>
          <Input type="text" nameId="lang" placeholder="Language" value={language} handleChange={handleLanguageChange}></Input>
          <Button handleClick={changeLanguage}>Set</Button>
        </div>
    </div>
  )
}

export default ChatMembers