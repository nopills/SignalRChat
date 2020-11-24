const connection = new signalR.HubConnectionBuilder()
    .withUrl("/chat")
    .configureLogging(signalR.LogLevel.Information)
    .build();

//Signal method invoked from server
connection.on("RecieveMessage", (name, message) => {
    console.clear();
    console.log(name +": " + message);
});

connection.on("Typing", (message) => { console.log(message); });


//s = ' ';
//s = s.replace(/^\s+|\s+$/g, '');
let sended = true;

let msg = document.getElementById("message");
    msg.oninput = function () {
        if (sended == true) {
            connection.invoke("Typing");
            sended = false;
        }
        if (msg.value == '') {
            console.clear();
            sended = true;
        }

    }   


//msg.oninput = function () {
  //  connection.invoke("Typing");
//}


document.getElementById("sendBtn").addEventListener("click", function (e) {
    let res = connection.invoke("GetLastMessages", "e2c0bfd5-0bb8-488c-b5f7-9e414224f435");
    console.log(res);
    //let message = document.getElementById("message").value;   
    //connection.invoke("SendPrivateMessage", "kekushpekush", message);
    //sended = true;
});






connection.start().catch(function (err) {
    return console.error(err.toString());
});
