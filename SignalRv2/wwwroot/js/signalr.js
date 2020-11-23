const connection = new signalR.HubConnectionBuilder()
    .withUrl("/chat")
    .configureLogging(signalR.LogLevel.Information)
    .build();

//Signal method invoked from server
connection.on("RecieveMessage", (name, message) => {
    console.log(name +": " + message);
});

document.getElementById("sendBtn").addEventListener("click", function (e) {
    let message = document.getElementById("message").value;
    connection.invoke("SendPrivateMessage", "kekushpekush", message);
});


connection.start().catch(function (err) {
    return console.error(err.toString());
});
