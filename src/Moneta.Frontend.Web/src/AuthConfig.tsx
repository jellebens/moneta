const {CLIENT_ID, TENANT_ID} = process.env
export const msalConfig = {
    
    auth: {
        clientId: CLIENT_ID,
        authority: `https://login.microsoftonline.com/${TENANT_ID}`,
        redirectUri: window.location.origin + '/home',
        validateAuthority: true
    },
    cache: {
        cacheLocation: "sessionStorage", // This configures where your cache will be stored
        storeAuthStateInCookie: false, // Set this to "true" if you are having issues on IE11 or Edge
    }
}

export const loginRequest = {
    scopes: ["api://3652d22c-6197-44a5-9334-da5a8c45182d/access_as_user"]
   };