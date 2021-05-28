import axios from 'axios';

export const callApiWithToken = async(accessToken) => {
    const url = "http://localhost:5000/WeatherForecast"
    // const headers = new Headers();
    // const bearer = `Bearer "+ ${accessToken}`;
    
    // headers.append('Authorization', bearer);
    
    // const options = {
    //     method: "GET",
    //     headers: headers,
    //     mode: "no-cors",
    //     credentials: 'same-origin', 
    // };
    // 
    // return await fetch(url, options)
    //     .then(response => response.json())
    //     .catch(err => console.error(err));


    const config = {
        headers: { Authorization: `Bearer ${accessToken}` },
        mode: "no-cors",
    };

    axios.get(url, config).then(response => console.log(response.data))
    
}