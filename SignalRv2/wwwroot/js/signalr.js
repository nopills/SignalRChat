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
    connection.on("Typing", (message) => { console.log(message); });

    connection.on("CheckStatus", (username, status) => {
        console.log(username + " is " + status);
    });

    connection.on("RecieveMessage", (name, message) => {
        core.RecieveMessage(name, message);
    });
    /* ~~ */


    document.getElementById("sendBtn").addEventListener("click", function (e) {
        let message = document.getElementById("messageInput").value;
        let group_item = document.querySelector('.group-item-active');
        let userName = group_item.querySelector('.userName').innerHTML;      
        connection.invoke("SendPrivateMessage", userName, message).then(res => res == true ? core.AddMessage(message) : "");
       
        document.getElementById("messageInput").value = "";
      //  message.value = "";

        //if (result != false) {
        //    AddMessage(message);
        //    sended = true;
        //}



        // connection.invoke("SendStatus", 1); //status

        //let res = connection.invoke("GetLastMessages", "e2c0bfd5-0bb8-488c-b5f7-9e414224f435");
        //console.log(res);
    });


    //connection.start(connection.invoke("GetDialogs")).then(dialogData =>
    //{
    //    core.AddDialogs(dialogData);
    //    service.setActiveDialog();
    //}).catch(function (err) { return console.error(err.toString()); });
        
    connection.start().then(conn => connection.invoke("GetDialogs").then(dialogData =>
    {
       // console.log(dialogData);
        core.GetDialogs(dialogData);
        service.setActiveDialog(connection);       
    })
    ).catch(function (err) { return console.error(err.toString()); });


    








});