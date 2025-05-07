fetch("http://localhost:5041/user", {
    method: "PUT",
    headers: {
        "content-type": "application/json",
        "authorization": "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJhdWQiOiJDaGF0LWFwcC1Vc2VycyIsImlzcyI6IkNoYXQtYXBwIiwiZXhwIjoxNzQ2NjI3NDk3LCJuYW1lIjoicGV0YXIiLCJpYXQiOjE3NDY2MjM4OTcsIm5iZiI6MTc0NjYyMzg5N30.jcJMZPRjayOrXSy2487WFg2Loy5YSqzol5JraCo2zdw"
    },
    body: JSON.stringify({
        name: "leo",
        password: "12345",
        avatar: "new avatar"
    })
}).then(res => {
    return res.json();
}).then(d => console.log(d))