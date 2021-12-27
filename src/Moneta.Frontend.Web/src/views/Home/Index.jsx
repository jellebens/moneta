import { React, useEffect, useState } from "react";
import { useMsal, useIsAuthenticated } from "@azure/msal-react";



export const  Main = () => {
    const isAuthenticated = useIsAuthenticated();
    
    const {accounts} = useMsal();

    const [username, setUsername] = useState();
    const [name, setName] = useState();
    

      useEffect(() => {
        if (accounts.length > 0) {
          setUsername(accounts[0].username);
          setName(accounts[0].name);
        }
    }, [accounts]);

    return (
        <div className="row">
            <div className="col-lg12">
            { isAuthenticated ? <p>Signed in as {name} ({username})</p> : <p>You are not signed in! Please sign in.</p> }
            </div>
        </div>
    )
}


export default Main;