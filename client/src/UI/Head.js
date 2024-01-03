import React from 'react'
import { Button } from './Button'
import { useNavigate, useParams } from 'react-router-dom'
import './ui.css'

const Head = ({redirectToCreateChat, chatName, logout, redirectToMembers}) => {
    const navigate = useNavigate();
    const params = useParams();
    var button;

    const redirectToSetCompanyInfo = () => {
      navigate("/set-info")
    }

    if (params.id != null) {
      button = <Button handleClick={redirectToMembers} style={{'width': "30%", 'margin': '10px 22px'}}>{chatName}</Button>
    } else if (localStorage.getItem("userName") === "admin") {
      button = <Button handleClick={redirectToSetCompanyInfo} style={{'width': "30%", 'margin': '10px 22px'}}>Setting up Assistant</Button>
    }

  return (
    <div className='head'>
        <Button handleClick={redirectToCreateChat} style={{'width': "30%", 'margin': '10px 22px'}}>Create chat</Button>
        
        {button}
        
        <Button handleClick={logout} style={{'width': "30%", 'margin': '10px 22px'}}>{localStorage.getItem("userName")}</Button>
    </div>
  )
}

export default Head