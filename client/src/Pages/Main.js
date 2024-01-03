import React, { useEffect, useState } from 'react'
import { HubConnectionBuilder, HttpTransportType } from '@microsoft/signalr'
import ListOfChats from '../Components/ListOfChats';
import Chat from '../Components/Chat';
import '../css/main.css'
import { useLocation, useNavigate, useParams } from 'react-router-dom';
import axios from 'axios';
import { API_URL } from '../API/ApiConstants';
import Head from '../UI/Head';
import Assistant from '../Components/Assistant';

export const Main = ({pull_connection}) => {
    const [connection, setConnection] = useState(null);

    const [chats, setChats] = useState([])
    const [messages, setMessages] = useState([]);

    const [messageRecieved, setMessageRecieved] = useState()
    const [newChatRecieved, setNewChatRecieved] = useState()
    const params = useParams();

    const navigate = useNavigate();
    const location = useLocation();

    useEffect(() => {
        var c = new HubConnectionBuilder()
            .withUrl("wss://localhost:7152/messages", {
                skipNegotiation: true,
                transport: HttpTransportType.WebSockets,
                accessTokenFactory: () => localStorage.getItem("accessToken")
            })
            .withAutomaticReconnect()
            .build();

        setConnection(c);
    }, [])    
    
    useEffect(() => {
        // console.log(connection)
        if (connection !== null) {
            connection.on("OnAddedToGroup", onNewChatRecieved);
            
            connection.on("OnRemovedFromGroup", data => {
                getChats();
            });
            
            connection.on("OnRecieveMessage", onRecieveMessage)

            connection.start();
            pull_connection(connection);
        }
        // eslint-disable-next-line
    }, [connection])    

    const onRecieveMessage = (data) => {
        setMessageRecieved(data);
    }

    const onNewChatRecieved = (data) => {
        connection.invoke("SubscribeToGroupAsync", data.id);
        setNewChatRecieved(data);
    }

    useEffect(() => {
        if (messageRecieved)
        {
            if (params.id === messageRecieved.chatId) {
                let c = chats.find(c => c.id === params.id)
                console.log(messageRecieved)
                console.log("lan: " + c.language)
                
                if (c.language !== "" && c.language !== null) {
                    if (messageRecieved.userName !== localStorage.getItem("userName")) {
                        let token = localStorage.getItem("accessToken");

                        axios.get(API_URL + "/chats/" + params.id + "/translate/" + messageRecieved.id + `?language=${c.language}`, {
                            headers: {
                                'Authorization': `Bearer ${token}`
                            }
                        }).then(res => {
                            let msgs = [...messages, res.data];
                            setMessages(msgs);
                        })
                        // console.log("SEND REQUEST TO TRANSLATE")
                    }
                }

                let msgs = [...messages, messageRecieved];
                setMessages(msgs);
            } else {
                setChats(chats.map(c => c.id === messageRecieved.chatId ? {...c, new: true} : c));
            }
        }
        // eslint-disable-next-line
    }, [messageRecieved])

    useEffect(() => {
      if (newChatRecieved ) {
        console.log(newChatRecieved)
        setChats([...chats, newChatRecieved])
      }
      // eslint-disable-next-line
    }, [newChatRecieved])

    useEffect(() => {
        if (params.id != null) {
            getMessages(chats.find(c => c.id === params.id)?.language ?? "");
        }
        // eslint-disable-next-line
    }, [params.id, location?.state?.language])

    useEffect(() => {
        if (params.id != null) {
            console.log("lang:" + location?.state?.language ?? "nothing")
            getMessages(location?.state?.language);
        }
        // eslint-disable-next-line
    }, [location?.state?.language])

    useEffect(() => {
        getChats()
    }, [])

    const getChats = async () => {
        var token = localStorage.getItem("accessToken");
        await axios.get(API_URL + "/chats", {
            headers: {
                'Authorization': `Bearer ${token}`
            }
          })
          .then(res => {
            setChats(res.data);
          })
    }

    const getMessages = async (language = "") => {
        var token = localStorage.getItem("accessToken");
        // console.log("lang:" + language);

        let url = API_URL + "/chats/" + params.id

        if (language !== "") {
            url += `?language=${language}`
        }

        await axios.get(url, {
            headers: {
                'Authorization': `Bearer ${token}`
            }
          })
          .then(res => {
            console.log(res.data)
            // setChats(chats.map(c => c.id === res.data.id ? res.data : c));
            setMessages(res.data.messages);
          })
    }

    const redirectToCreateChat = () => {
        navigate("/create-chat")
    }
    
    const logout = (e) => {
        e.preventDefault();
        localStorage.clear();
        navigate('/login')
    }

    const redirectToMembers = (e) => {
        e.preventDefault();
        let chat = chats.find(c => c.id === params.id);
        // console.log("ch: ", chat)
        navigate('/main/' + params.id + "/members", {state: {chats: chats, users: chat?.users}})
    }

  return (
    <div className='main'>
        <Head 
            redirectToCreateChat={redirectToCreateChat} 
            chatName={chats.find(c => c?.id === params.id)?.name ?? ''} 
            logout={logout} 
            redirectToMembers={redirectToMembers}
        />
        <div className='main-chats'>
            <ListOfChats chats={chats}></ListOfChats>
            {params.id != null ?
                <Chat connection={connection} messages={messages}></Chat>
                :
                <Chat>Select chat to start messaging.</Chat>
            }
            <Assistant></Assistant>
        </div>
    </div>
  )
}
