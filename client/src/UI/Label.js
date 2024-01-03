import React from 'react'
import './ui.css'

const Label = ({children}) => {
  return (
    <label className='label'>
        {children}
    </label>
  )
}

export default Label