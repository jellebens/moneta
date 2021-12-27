import React, { useState } from "react";
import { useMsal } from "@azure/msal-react";
import { loginRequest } from "AuthConfig";
import { Button } from "reactstrap"
import axios from "axios";

//https://github.com/Azure-Samples/ms-identity-javascript-react-tutorial/blob/main/3-Authorization-II/1-call-api/SPA/src/authConfig.js


export const Dashboard = () => {
    const { instance, accounts } = useMsal();
    const [accessToken, setAccessToken] = useState(null);
    const [serverName, setServerName] = useState(null);

    const name = accounts[0] && accounts[0].name;

    function RequestAccessToken() {
        const request = {
            ...loginRequest,
            account: accounts[0]
        };

        instance.acquireTokenSilent(request).then(async (response) => {
            setAccessToken(response.accessToken);
            let url = "/api/users/me";

            if (process.env.REACT_APP_API !== undefined ) {
                url = process.env.REACT_APP_API + "/api/users/me";
            }
            console.log("url=" + url);

            const config = {
                headers: { Authorization: `Bearer ${response.accessToken}` },
                mode: "no-cors",
            };
            
            axios.get(url, config).then(r => setServerName(r.data.name));
        }).catch((e) => {
            console.log(e);
            instance.acquireTokenPopup(request).then((response) => {
                setAccessToken(response.accessToken);
            });
        });
    }

    return (
        <>
            <h5 className="card-title">Welcome {name}</h5>
            {accessToken ?
                <>
                    <p>Access Token Acquired! {accessToken}</p>
                    <p>On the server you are known as:&nbsp;{serverName}</p>
                </>
                :
                <>
                {sessionStorage.getItem("bearer_token")}
                <Button variant="secondary" onClick={RequestAccessToken}>Request Access Token</Button>
                </>
                


            }


        </>
    );

}


export default Dashboard;