import React from 'react'
import '../css/main.css'

const Message = ({message, isMine}) => {

  const convertTime = (utc) => {
    var pdt = new Date(utc);
    return pdt.getHours() + ":" + pdt.getMinutes();
  }

  return (
    <div className='message' style={{'alignSelf': isMine ? 'flex-end' : 'flex-start'}}>
        {message?.author !== null &&
          <div className='message-author'>
            {message.userName}
          </div>
        }
        <div className='message-content'>
          {message.content}
        </div>
        {message?.postedTime &&
          <div className='message-date'>
            {convertTime(message.postedTime)}
          </div>
        }
    </div>
  )
}

export default Message