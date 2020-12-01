let chat_body = document.querySelector('.chat-body');

function NewMessageNotification(senderName, message) {
    let listGroup = document.querySelectorAll('.group-item');
    let notifySenderName = Array.from(listGroup).find((u) => {
        let username = u.querySelector('.userName').innerHTML;
        if (username === senderName) {
            let counter = parseInt(u.querySelector('.new-message-count').innerHTML);

     
            let time = new Date();
            time = time.toLocaleString('en-US', { hour: 'numeric', minute: 'numeric', hour12: true })
           

            u.querySelector('p').innerHTML = message;
            u.querySelector('.message-time').innerHTML = time;


            let userList = u.querySelector('.user-list-actions');
            userList.style.visibility = 'visible';

            let messCount = u.querySelector('.new-message-count');
            messCount.style.visibility = 'visible';
            u.querySelector('.new-message-count').innerHTML = ++counter;
        }    
    });
}

function GetDialogs(dialogs) {
    dialogs.forEach((e) => {
        let unreadMessageCount = parseInt(e.unreadMessage) > 0 ? `<div class="new-message-count">` + e.unreadMessage + `</div>` : `<div style="visibility: hidden;" class="new-message-count">0</div>`;

        let li = document.createElement("li");
        li.className = "group-item";
        li.innerHTML = `
                    <div class="dialogId" hidden="true">` + e.dialogId + `</div>
                    <div class="senderId" hidden="true">` + e.senderId + `</div>
                    <div class="userName" hidden="true">` + e.recieverUsername + `</div>
                    <figure class="avatar-status-active">
                    <img src="/img/avatars/default-avatar.png" alt="user-avatar" class="avatar avatar-circle">
                </figure>
                <div class="user-list-body">
                    <h5 class="user-name">` + e.recieverName +
            `</h5>
                    <p>`+ e.lastMessage + `</p>
                    <small class="message-time">` + e.lastActivity + `</small>
                </div>
                <div class="user-list-actions">`+ unreadMessageCount +`</div>`;
        let sidebarBody = document.querySelector(".list-group");
        sidebarBody.append(li)
    });
}

function RecieveMessage(name, message) {
    message = message.replace(/<\/?[^>]+(>|$)/g, "");
    if (message.trim() !== "") {
        let time = new Date();
        time = time.toLocaleString('en-US', { hour: 'numeric', minute: 'numeric', hour12: true })
        time = time.replace(/<\/?[^>]+(>|$)/g, "");

        name = name.replace(/<\/?[^>]+(>|$)/g, "");

        let msgData = document.createElement("div");
        msgData.className = "message-item";
        msgData.innerHTML = `<figure class="avatar">
                             <img src="/img/avatars/default-avatar.png" class="avatar avatar-circle" alt="dialog-avatar">
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

        let active_dialog = document.querySelector('.group-item-active');
        active_dialog.querySelector('p').innerHTML = message;
        active_dialog.querySelector('.message-time').innerHTML = time;

        chat_body.scrollTop = chat_body.scrollHeight;
    }
}
function AddMessage(message) {
    if (message.trim() !== "") {
        let time = new Date();
        let msgData = document.createElement("div");

        time = time.toLocaleString('en-US', { hour: 'numeric', minute: 'numeric', hour12: true })
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
                            <img src="/img/avatars/default-avatar.png" class="avatar avatar-circle" alt="dialog-avatar">
                        </figure>`;


        let msgs = document.querySelector(".messages");
        msgs.append(msgData);

        let active_dialog = document.querySelector('.group-item-active');
        active_dialog.querySelector('p').innerHTML = message;
        active_dialog.querySelector('.message-time').innerHTML = time;

        chat_body.scrollTop = chat_body.scrollHeight;
    }
}

function AddOutgoingMessage(message) {
    let content = message.content;
    content = content.replace(/<\/?[^>]+(>|$)/g, "");
    if (content.trim() !== "") {
        let msgData = document.createElement("div");

        let time = new Date(message.when);
        time = time.toLocaleString('en-US', { hour: 'numeric', minute: 'numeric', hour12: true })
        time = time.replace(/<\/?[^>]+(>|$)/g, "");

        msgData.className = "message-item outgoing-message";
        msgData.innerHTML = `<div class="message">
                            <div class="dialog-user-title">
                                <div class="time">` + time + `<i class="double-checke-message"></i></div>
                                <h5 class="sender-name">You</h5>
                            </div>
                            <div class="message-content">`+ content + `</div>
                        </div>
                        <figure class="avatar">
                            <img src="/img/avatars/default-avatar.png" class="avatar avatar-circle" alt="dialog-avatar">
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

    let name = message.user.userName;
    name = name.replace(/<\/?[^>]+(>|$)/g, "");

    let time = new Date(message.when);
    time = time.toLocaleString('en-US', { hour: 'numeric', minute: 'numeric', hour12: true })
    time = time.replace(/<\/?[^>]+(>|$)/g, "");


    if (content.trim() !== "") {     
        msgData.className = "message-item";
        msgData.innerHTML = `<figure class="avatar">
                             <img src="/img/avatars/default-avatar.png" class="avatar avatar-circle" alt="dialog-avatar">
                             </figure>
                          <div class="message">
                            <div class="dialog-user-title">
                             <h5 class="reciever-name">`+ name + `</h5>
                             <div class="time">` + time + `<i class="double-checke-message"></i></div>                        
                            </div>
                            <div class="message-content">
                                `+ content + ` 
                          </div>
                        </div>`;
        let msgs = document.querySelector(".messages");
        msgs.append(msgData);

        chat_body.scrollTop = chat_body.scrollHeight;

        let unreadMessages = [];
        if (message.isRead == false) {
            unreadMessages.push(message.id);
            return unreadMessages;
        }
            
    }
}

export { RecieveMessage, AddMessage, GetDialogs, AddOutgoingMessage, AddIncomingMessage, NewMessageNotification};