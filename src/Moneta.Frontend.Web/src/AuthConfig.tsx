export const msalConfig = {
    auth: {
        clientId: "d6d03eda-82b7-4298-8940-7f72805cf398",
        authority: `https://login.microsoftonline.com/6e37ccb0-a61f-4da3-a953-9cde6306d7eb`,
        redirectUri: window.location.origin + '/',
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