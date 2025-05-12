fetch("http://localhost:5041/user/login", {
    method: "POST",
    headers: {
        "content-type": "application/json",
    },
    body: JSON.stringify({
        name: "leo",
        password: "12345",
        avatar: "new avatar"
    })
}).then(res => {
    console.log(res)
    return res.json();
}).then(d => console.log(d))