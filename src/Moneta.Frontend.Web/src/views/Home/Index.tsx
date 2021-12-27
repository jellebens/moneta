import React, {useState , useEffect } from "react";
import { useIsAuthenticated, useMsal } from "@azure/msal-react";




export const  Main = () => {
    const isAuthenticated = useIsAuthenticated();
    
    const accounts = useMsal().accounts;

    const [username, setUsername] = useState<string>();
    const [name, setName] = useState<string>();

      useEffect(() => {
        if (accounts.length > 0) {
          console.log(accounts[0]);
          setUsername(accounts[0].username);
          setName(accounts[0].username);
        }
    }, [accounts]);

    return (
        <div className="row">
            <div className="col-lg12">
            { isAuthenticated ? <p>Signed in as {name} ({username})</p> : <p>not signed in</p> }
            </div>
        </div>
    )
}


export default Main;