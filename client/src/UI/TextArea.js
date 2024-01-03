import React from 'react'
import './ui.css'

const TextArea = ({value, onChange, placeholder}) => {
  return (
    <textarea className='input' rows={3} cols={70} value={value} onChange={onChange} placeholder={placeholder}>
        
    </textarea>
  )
}

export default TextArea