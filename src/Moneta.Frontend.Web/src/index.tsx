import React from "react";
import ReactDOM from "react-dom";
import { BrowserRouter, Switch } from "react-router-dom";
import { PublicClientApplication } from "@azure/msal-browser";
import { MsalProvider } from "@azure/msal-react";
import { msalConfig } from "AuthConfig";
import axios from "axios";

import "bootstrap/dist/css/bootstrap.css";
import "assets/scss/paper-dashboard.scss?v=1.3.0";
import "perfect-scrollbar/css/perfect-scrollbar.css";

//https://github.com/AzureAD/microsoft-authentication-library-for-js/tree/dev/samples/msal-react-samples/react-router-sample
//https://github.com/AzureAD/microsoft-authentication-library-for-js/blob/dev/samples/msal-react-samples/react-router-sample/src/App.js
//https://www.youtube.com/watch?v=3PyUjOmuFic
//https://github.com/Azure-Samples/ms-identity-javascript-react-spa-dotnetcore-webapi-obo/

//Layout
//https://demos.creative-tim.com/paper-dashboard-pro-react/?_ga=2.87622104.1789144586.1641464915-1173133874.1641464915#/admin/dashboard

import AdminLayout from "layouts/Admin";

export const msalInstance = new PublicClientApplication(msalConfig);

if (process.env.REACT_APP_API !== undefined ) {
  axios.defaults.baseURL = process.env.REACT_APP_API + "/api/accounts";
}


ReactDOM.render(
  <React.StrictMode>
    <MsalProvider instance={msalInstance}>
        <BrowserRouter>
          <Switch>
            <AdminLayout />
          </Switch>
        </BrowserRouter>
    </MsalProvider>

  </React.StrictMode>
  , document.getElementById("root")
);