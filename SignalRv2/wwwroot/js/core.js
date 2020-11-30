let chat_body = document.querySelector('.chat-body');

function GetDialogs(dialogs) {
    dialogs.forEach((e) => {
        let li = document.createElement("li");
        li.className = "group-item";
        li.innerHTML = `
                    <div class="dialogId" hidden="true">` + e.dialogId + `</div>
                    <div class="senderId" hidden="true">` + e.senderId + `</div>
                    <div class="userName" hidden="true">` + e.recieverUsername + `</div>
                    <figure class="avatar-status-active">
                    <img src="/img/avatars/women_avatar5.jpg" alt="user-avatar" class="avatar avatar-circle">
                </figure>
                <div class="user-list-body">
                    <h5 class="user-name">` + e.recieverName +
            `</h5>
                    <p>`+ e.lastMessage + `</p>
                    <small class="message-time">` + e.lastActivity + `</small>
                </div>
                <div class="user-list-actions">
                    <div class="new-message-count">`+ e.unreadMessage +
            `</div>
                </div>
            `;
        let sidebarBody = document.querySelector(".list-group");
        sidebarBody.append(li)
    });
}

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
                             <h5 class="reciever-name">`+ name + `</h5>
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
    if (message.trim() !== "") {
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

function AddOutgoingMessage(message) {
    let content = message.content;
    content = content.replace(/<\/?[^>]+(>|$)/g, "");
    if (content.trim() !== "") {
        let msgData = document.createElement("div");
     //   let time = message.when;
       // time = time.toLocaleTimeString([], { timeStyle: 'short' });
    //    time = time.replace(/<\/?[^>]+(>|$)/g, "");
        

        msgData.className = "message-item outgoing-message";
        msgData.innerHTML = `<div class="message">
                            <div class="dialog-user-title">
                                <div class="time">` + message.when + `<i class="double-checke-message"></i></div>
                                <h5 class="sender-name">You</h5>
                            </div>
                            <div class="message-content">`+ content + `</div>
                        </div>
                        <figure class="avatar">
                            <img src="/img/avatars/man_avatar1.jpg" class="avatar avatar-circle" alt="dialog-avatar">
                        </figure>`;


        let msgs = document.querySelector(".messages");           
        msgs.append(msgData);
        chat_body.scrollTop = chat_body.scrollHeight;
    }
}

function AddIncomingMessage(message) {
    let msgData = document.createElement("div");
    let content = message.content;
    content = content.replace(/<\/?[^>]+(>|$)/g, "");
    if (content.trim() !== "") {     
        msgData.className = "message-item";
        msgData.innerHTML = `<figure class="avatar">
                             <img src="/img/avatars/man_avatar4.jpg" class="avatar avatar-circle" alt="dialog-avatar">
                             </figure>
                          <div class="message">
                            <div class="dialog-user-title">
                             <h5 class="reciever-name">`+ message.user.userName + `</h5>
                             <div class="time">` + message.when + `<i class="double-checke-message"></i></div>                        
                            </div>
                            <div class="message-content">
                                `+ content + ` 
                          </div>
                        </div>`;
        let msgs = document.querySelector(".messages");
        msgs.append(msgData);

        chat_body.scrollTop = chat_body.scrollHeight;
    }
}

export { RecieveMessage, AddMessage, GetDialogs, AddOutgoingMessage, AddIncomingMessage};