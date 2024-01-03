import React from 'react'
import './ui.css'

export const Button = ({children, handleClick, style}) => {
  return (
    <label onClick={handleClick} className='button' style={style}>
        {children}
    </label>
  )
}
