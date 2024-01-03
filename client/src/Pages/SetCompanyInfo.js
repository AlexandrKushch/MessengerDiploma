import React, { useEffect } from 'react'
import { Input } from '../UI/Input'
import { Button } from '../UI/Button'
import TextArea from '../UI/TextArea'
import Label from '../UI/Label'
import { useState } from 'react'
import '../css/set-info.css'
import { useNavigate } from 'react-router-dom'
import axios from 'axios'
import { API_URL } from '../API/ApiConstants'

const SetCompanyInfo = () => {

    const navigate = useNavigate();

    const [history, setHistory] = useState("")
    const [mission, setMission] = useState("")
    const [organization, setOrganization] = useState("")
    const [benefits, setBenefits] = useState("")
    const [culture, setCulture] = useState("")
    const [procedures, setProcedures] = useState("")
    const [contacts, setContacts] = useState("")
    const [tools, setTools] = useState("")

    useEffect(() => {
      getInfo();
    }, [])

    const set = (e) => {
      e.preventDefault();
      var info = `Company History: ${history}\nMission and values: ${mission}\nOrganizational Structer: ${organization}\nBenefits and avantages: ${benefits}\nCulture and work environment: ${culture}\nKey processes and procedures: ${procedures}\nKey contacts: ${contacts}\nTechnologies and tools: ${tools}`;

      setInfo(info);
      navigate("/main")
    }

    const setInfo = async (info) => {
      await axios.post(API_URL + "/gpt/info", {
        content: info
      }, {
        headers: {
          'Authorization': `Bearer ${localStorage.getItem("accessToken")}`
        }
      })
    }

    const getInfo = async () => {
      await axios.get(API_URL + "/gpt/info", {
        headers: {
          'Authorization': `Bearer ${localStorage.getItem("accessToken")}`
        }
      }).then(res => {
        if (res.data !== "") {
          console.log(res.data);
  
          var infos = res.data.split("\n");
          console.log(infos)
          
          setHistory(infos[0].slice(infos[0].indexOf(": ") + 2));
          setMission(infos[1].slice(infos[1].indexOf(": ") + 2));
          setOrganization(infos[2].slice(infos[2].indexOf(": ") + 2));
          setBenefits(infos[3].slice(infos[3].indexOf(": ") + 2));
          setCulture(infos[4].slice(infos[4].indexOf(": ") + 2));
          setProcedures(infos[5].slice(infos[5].indexOf(": ") + 2));
          setContacts(infos[6].slice(infos[6].indexOf(": ") + 2));
          setTools(infos[7].slice(infos[7].indexOf(": ") + 2));
        }
      });
    }

    const handleHistoryChange = (e) => {
        e.preventDefault();

        setHistory(e.target.value)
    }

    const handleMissionChange = (e) => {
        e.preventDefault();

        setMission(e.target.value)
    }

    const handleOrganizationChange = (e) => {
        e.preventDefault();

        setOrganization(e.target.value)
    }

    const handleBenefitsChange = (e) => {
        e.preventDefault();

        setBenefits(e.target.value)
    }

    const handleCultureChange = (e) => {
        e.preventDefault();

        setCulture(e.target.value)
    }

    const handleProceduresChange = (e) => {
        e.preventDefault();

        setProcedures(e.target.value)
    }

    const handleContactsChange = (e) => {
        e.preventDefault();

        setContacts(e.target.value)
    }

    const handleToolsChange = (e) => {
        e.preventDefault();

        setTools(e.target.value)
    }

  return (
    <div className='set-info'>
        <Label>History:</Label>
        <br></br>
        <TextArea value={history} onChange={handleHistoryChange} placeholder={"Company History: A brief overview of the company's history, including the founding date, key events and achievements."}></TextArea>
        
        <Label>Mission:</Label>
        <br></br>
        <TextArea value={mission} onChange={handleMissionChange} placeholder={"Mission and values: Clarification of the company's core mission and the values it strives to embody in its work."}></TextArea>
        
        <Label>Organization Structure:</Label>
        <br></br>
        <TextArea value={organization} onChange={handleOrganizationChange} placeholder={"Organizational Structure: A description of the company's structure, including divisions, departments, and key managers."}></TextArea>
        
        <Label>Benefits:</Label>
        <br></br>
        <TextArea value={benefits} onChange={handleBenefitsChange} placeholder={"Benefits and advantages: Information about the package of social guarantees, rewards, opportunities for development and other benefits offered by the company."}></TextArea>
        
        <Label>Culture:</Label>
        <br></br>
        <TextArea value={culture} onChange={handleCultureChange} placeholder={"Culture and work environment: A description of the atmosphere in the company, including the meaning of work culture, ways of communicating and collaborating, and general expectations."}></TextArea>
        
        <Label>Procedures:</Label>
        <br></br>
        <TextArea value={procedures} onChange={handleProceduresChange} placeholder={"Key processes and procedures: Explanation of key processes in the company, such as internal communications, reporting procedures, interaction with other departments, etc."}></TextArea>
        
        <Label>Contacts:</Label>
        <br></br>
        <TextArea value={contacts} onChange={handleContactsChange} placeholder={"Key Contacts: Providing a list of key individuals or departments that can be contacted for support, advice or information."}></TextArea>
        
        <Label>Tools:</Label>
        <br></br>
        <TextArea value={tools} onChange={handleToolsChange} placeholder={"Technologies and tools: Information about the technologies, programs, systems and other tools used in the company that new employees will use in their work."}></TextArea>
        
        <Button handleClick={set}>Set</Button>
    </div>
  )
}

export default SetCompanyInfo