import React from 'react'
import CreateChat from './Pages/CreateChat';
import { useState } from 'react';
import { Main } from './Pages/Main';
import { Registration } from './Pages/Registration';
import { SignInPage } from './Pages/SignInPage';
import { Routes, Route, Navigate } from 'react-router-dom';
import ChatMembers from './Pages/ChatMembers';
import SetCompanyInfo from './Pages/SetCompanyInfo';

const AppRoutes = () => {
    const [connection, setConnection] = useState();

    const pull_connection = (connection) => {
      setConnection(connection);
    }

  return (
    localStorage.getItem("accessToken") !== null ?
      <Routes>
        <Route path='/login' element={ <SignInPage /> } />
        <Route path='/registration' element={ <Registration /> } />
        <Route path='/main' element={ <Main pull_connection={pull_connection} /> } />
        <Route path='/main/:id' element={ <Main pull_connection={pull_connection} /> } />
        <Route path='/main/:id/members' element={ <ChatMembers /> } />
        <Route path='/create-chat' element={ <CreateChat connection={connection} /> } />
        {localStorage.getItem("userName") === "admin" &&
          <Route path='/set-info' element={ <SetCompanyInfo /> } />
        }

        <Route path='*' element={<Navigate to="/main" replace></Navigate>}></Route>
      </Routes>
    :
    <Routes>
      <Route path='/login' element={ <SignInPage /> } />
      <Route path='/registration' element={ <Registration /> } />
      
      <Route path='*' element={<Navigate to="/login" replace></Navigate>}></Route>
    </Routes>
  )
}

export default AppRoutes