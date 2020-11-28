let chat_body = document.querySelector('.chat-body');
let sended = true;


function RecieveMessage(name, message) {
    let time = new Date();
    let msgData = document.createElement("div");
    time = time.toLocaleTimeString([], { timeStyle: 'short' });
    
    message = message.replace(/<\/?[^>]+(>|$)/g, "");
    time = time.replace(/<\/?[^>]+(>|$)/g, "");
    name = name.replace(/<\/?[^>]+(>|$)/g, "");
    msgData.className = "message-item";
    msgData.innerHTML = `<figure class="avatar">
                             <img src="/img/avatars/man_avatar4.jpg" class="avatar avatar-circle" alt="dialog-avatar">
                             </figure>
                          <div class="message">
                            <div class="dialog-user-title">
                             <h5 class="reciever-name">`+ name +`</h5>
                             <div class="time">` + time + `<i class="double-checke-message"></i></div>                        
                            </div>
                            <div class="message-content">
                                `+ message + ` 
                          </div>
                        </div>`;
    let msgs = document.querySelector(".messages");
    msgs.append(msgData);

    chat_body.scrollTop = chat_body.scrollHeight;
}

function AddMessage(message) {
    if (message.trim() !== "" ) {
        let time = new Date();
        let msgData = document.createElement("div");
        time = time.toLocaleTimeString([], { timeStyle: 'short' });
        time = time.replace(/<\/?[^>]+(>|$)/g, "");
        message = message.replace(/<\/?[^>]+(>|$)/g, "");

        msgData.className = "message-item outgoing-message";
        msgData.innerHTML = `<div class="message">
                            <div class="dialog-user-title">
                                <div class="time">` + time + `<i class="double-checke-message"></i></div>
                                <h5 class="sender-name">You</h5>
                            </div>
                            <div class="message-content">`+ message + `</div>
                        </div>
                        <figure class="avatar">
                            <img src="/img/avatars/man_avatar1.jpg" class="avatar avatar-circle" alt="dialog-avatar">
                        </figure>`;


        let msgs = document.querySelector(".messages");
        msgs.append(msgData);

        chat_body.scrollTop = chat_body.scrollHeight;
    }
}


// Open Connection
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/chat")
    .configureLogging(signalR.LogLevel.Information)
    .build();



connection.on("Typing", (message) => { console.log(message); });

connection.on("CheckStatus", (username, status) => {
    console.log(username + " is " + status);
});


//Subscribe to hub messages
connection.on("RecieveMessage", (name, message) => {
         RecieveMessage(name, message);    
});

document.getElementById("sendBtn").addEventListener("click", function (e) {   
    let message = document.getElementById("messageInput").value;
    let mockName = document.querySelector('.search-input').value;
    connection.invoke("SendPrivateMessage", mockName, message).then(res => res == true ? AddMessage(message) : "");

    
    //if (result != false) {
    //    AddMessage(message);
    //    sended = true;
    //}
   
 

     // connection.invoke("SendStatus", 1); //status

    //let res = connection.invoke("GetLastMessages", "e2c0bfd5-0bb8-488c-b5f7-9e414224f435");
    //console.log(res);
});





connection.start().catch(function (err) {
    return console.error(err.toString());
});











    //msg.oninput = function () {
    //    if (sended == true) {
    //        connection.invoke("Typing", "kekushpekush");
    //        sended = false;
    //    }
    //    if (msg.value == '') {
    //        console.clear();
    //        sended = true;
    //    }

   // }   


//msg.oninput = function () {
  //  connection.invoke("Typing");
//}


