import React from 'react'
import './ui.css'

export const Input = ({type, nameId, placeholder, value, handleChange, style}) => {
  return (
    <input type={type} id={nameId} name={nameId} placeholder={placeholder} value={value} onChange={handleChange} className='input' style={style}>

    </input>
  )
}
