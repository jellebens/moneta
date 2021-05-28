import React from "react";
import ReactDOM from "react-dom";
import { BrowserRouter,  Switch } from "react-router-dom";
import { PublicClientApplication  } from "@azure/msal-browser";
import { MsalProvider } from "@azure/msal-react";
import { msalConfig } from "AuthConfig";

import "bootstrap/dist/css/bootstrap.css";
import "assets/scss/paper-dashboard.scss?v=1.3.0";
import "perfect-scrollbar/css/perfect-scrollbar.css";

//https://github.com/AzureAD/microsoft-authentication-library-for-js/tree/dev/samples/msal-react-samples/react-router-sample
//https://github.com/AzureAD/microsoft-authentication-library-for-js/blob/dev/samples/msal-react-samples/react-router-sample/src/App.js
//https://www.youtube.com/watch?v=3PyUjOmuFic
//https://github.com/Azure-Samples/ms-identity-javascript-react-spa-dotnetcore-webapi-obo/

import AdminLayout from "layouts/Admin";

const msalInstance = new PublicClientApplication(msalConfig);


ReactDOM.render(
  <React.StrictMode>
    <MsalProvider instance={msalInstance}>
    <BrowserRouter>
    <Switch>
      <AdminLayout  />
    </Switch>
  </BrowserRouter>
    </MsalProvider>
  
  </React.StrictMode>
  , document.getElementById("root")
);