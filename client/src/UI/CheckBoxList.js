import React from 'react'
import './ui.css'
import { Input } from './Input'
import { useState } from 'react'
import { useEffect } from 'react'

const CheckBoxList = ({elements, checkedElements, handleClick}) => {
    const [filter, setFilter] = useState("")
    const [filteredElements, setFilteredElements] = useState(elements)

    useEffect (() => {
        let username = localStorage.getItem("userName");
        let hideCurrentUser = elements.filter(item => item.userName !== username);
        setFilteredElements(hideCurrentUser);
    }, [elements])

    const handleFilterChange = (e) => {
        e.preventDefault();
        setFilter(e.target.value);
        let username = localStorage.getItem("userName");
        let hideCurrentUser = elements.filter(item => item.userName !== username);

        if (e.target.value === "") {
            setFilteredElements(hideCurrentUser)
        } else {
            let filtered = hideCurrentUser.filter(el => el.userName.startsWith(e.target.value))
            setFilteredElements(filtered);
        }
    }

  return (
    <div className='checked-list'>
        <Input type={'text'} nameId={'filter'} placeholder={'Filter'} value={filter} handleChange={handleFilterChange} ></Input>
        {filteredElements.map(element => 
            <div 
                className='list-element'
                key={element.id}
                onClick={(e) => handleClick(e, element)}
                style={{'color': checkedElements.includes(element) ? '#bb86ce' : ''}}
            >
                {element.userName}
            </div>
        )}
    </div>
  )
}

export default CheckBoxList