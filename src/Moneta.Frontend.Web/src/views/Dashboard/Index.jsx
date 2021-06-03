import React, { useState } from "react";
import { useMsal  } from "@azure/msal-react";
import { loginRequest } from "AuthConfig";
import {Button} from "reactstrap";
import { callApiWithToken } from "fetch";

//https://github.com/Azure-Samples/ms-identity-javascript-react-tutorial/blob/main/3-Authorization-II/1-call-api/SPA/src/authConfig.js


function Dashboard() {
    const { instance, accounts } = useMsal();
    const [accessToken, setAccessToken] = useState(null);

    const name = accounts[0] && accounts[0].name;

    function RequestAccessToken() {
        const request = {
            ...loginRequest,
            account: accounts[0]
        };

        instance.acquireTokenSilent(request).then((response) => {
            setAccessToken(response.accessToken);
            callApiWithToken(response.accessToken);
        }).catch((e) => {
            instance.acquireTokenPopup(request).then((response) => {
                setAccessToken(response.accessToken);
            });
        });
    }

    return (
        <>
            <h5 className="card-title">Welcome {name}</h5>
            {accessToken ? 
                <p>Access Token Acquired! {accessToken}</p>
                :
                <Button variant="secondary" onClick={RequestAccessToken}>Request Access Token</Button>
            }

            
        </>
    );
    
}


export default Dashboard;