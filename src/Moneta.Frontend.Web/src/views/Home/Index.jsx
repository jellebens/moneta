import {useState , useEffect } from "react";
import { useIsAuthenticated, useMsal } from "@azure/msal-react";

export const  Dashboard = () => {
    const isAuthenticated = useIsAuthenticated();
    const {accounts} = useMsal();

    const [name, setName] = useState(null);
    const [username, setUserName] = useState(null);

      useEffect(() => {
        if (accounts.length > 0) {
          setName(accounts[0].name);
          setUserName(accounts[0].username)
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


export default Dashboard;