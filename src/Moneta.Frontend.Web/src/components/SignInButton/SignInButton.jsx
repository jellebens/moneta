import React from "react";
import { useMsal } from "@azure/msal-react";
import { Button, NavItem } from "reactstrap";
import { loginRequest } from "AuthConfig";

function handleLogin(instance) {
    instance.loginPopup(loginRequest).catch(e => {
        console.error(e);
    });
}

/**
 * Renders a button which, when selected, will redirect the page to the login prompt
 */
export const SignInButton = () => {
    const { instance } = useMsal();

    return (
        <NavItem>
            <Button variant="secondary" className="btn btn-primary" onClick={() => handleLogin(instance)}>Sign in</Button>
        </NavItem>
    );
}