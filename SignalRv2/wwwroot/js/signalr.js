import * as core from '../js/core.js';
import * as service from '../js/service.js';

document.addEventListener('DOMContentLoaded', () => {  
    let sended = true;

    // Open Connection
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/chat")
        .configureLogging(signalR.LogLevel.Information)
        .build();


   /* SUBSCRIBE TO BROADCASTING */
    connection.on("CheckStatus", (username, status) => {
        console.log(username + " is " + status);
    });

    connection.on("RecieveMessage", (senderFullName, senderUserName, message) => {      
        let groupItemActive = document.querySelector('.group-item-active');
        if (groupItemActive == null) {
            core.NewMessageNotification(senderUserName, message);
         }
         else {      
            let username = groupItemActive.querySelector('.userName').innerHTML;
            if (username == senderUserName) {
                core.RecieveMessage(senderFullName, message);
            } else {
                core.NewMessageNotification(senderUserName, message);
            }
         }
    });

    /* ~~ */
    if (sendBtn != null) {
     sendBtn.addEventListener("click", function (e) {
        let message = document.getElementById("messageInput").value;
        message = message.replace(/<\/?[^>]+(>|$)/g, "");
        if (message.trim() !== "" && message.length > 0) {
            let group_item = document.querySelector('.group-item-active');
            let userName = group_item.querySelector('.userName').innerHTML;
            connection.invoke("SendPrivateMessage", userName, message).then(res => res == true ? core.AddMessage(message) : "");

            document.getElementById("messageInput").value = "";
        }
    });
   }
 
        
    connection.start().then(conn => connection.invoke("GetDialogs").then(dialogData =>
    {
        core.GetDialogs(dialogData);
        service.setActiveDialog(connection);       
    })
    ).catch(function (err) { return console.error(err.toString()); });
});