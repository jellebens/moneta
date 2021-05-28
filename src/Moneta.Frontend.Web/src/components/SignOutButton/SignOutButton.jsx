import { React, useEffect, useState } from "react";
import { useMsal } from "@azure/msal-react";
import {Dropdown, DropdownToggle, DropdownMenu, DropdownItem } from "reactstrap";

function handleLogout(instance) {
    instance.logoutPopup().catch(e => {
        console.error(e);
    });
}

function Greeting(props){
  if(props.name){
    return <p>Hello,&nbsp;{props.name}</p>
  }else{
    return "";
  }

}

export const SignOutButton = () => {
    const { instance, accounts } = useMsal();
    const [name, setName] = useState(null);
    const [dropdownOpen, setDropdownOpen] = useState(false);

    const dropdownToggle = (e) => {
        setDropdownOpen(!dropdownOpen);
      };

      
      //
      useEffect(() => {
        if (accounts.length > 0) {
          setName(accounts[0].name.split(" ")[0]);
        }
    }, [accounts]);

    return (
      

        <Dropdown
              nav
              isOpen={dropdownOpen}
              toggle={(e) => dropdownToggle(e)}
            >
              <DropdownToggle caret nav>
                <i className="nc-icon nc-single-02" />
                <Greeting name={name}/>
              </DropdownToggle>
              <DropdownMenu right>
                <DropdownItem tag="a" onClick={() => handleLogout(instance)} >Sign out</DropdownItem>
              </DropdownMenu>
            </Dropdown>
    );
}