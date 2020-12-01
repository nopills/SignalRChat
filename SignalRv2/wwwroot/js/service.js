import * as core from '../js/core.js';

function setActiveDialog(connection) {
    let dialogList = document.querySelectorAll('.group-item');
   
    dialogList.forEach(e => {
        e.addEventListener('click', () => {
            e.preventDefaut;        
            
            document.querySelectorAll('.message-item').forEach(x => x.parentElement.removeChild(x));

            dialogList.forEach(i =>  i.classList.remove('group-item-active'));
            
            e.classList.add('group-item-active');

            let userList = e.querySelector('.user-list-actions');          
            userList.style.visibility = 'hidden';

            let messageCounter = e.querySelector('.new-message-count');
            messageCounter.style.visibility = 'hidden';
            messageCounter.innerHTML = 0;

            let dialogId = e.querySelector('.dialogId').innerHTML;
            let senderId = e.querySelector('.senderId').innerHTML;
            connection.invoke("GetMessages", dialogId).then((msg) => {
                msg.forEach(m => {
                    m.userId == senderId ? core.AddOutgoingMessage(m) : connection.invoke("MarkAsRead", core.AddIncomingMessage(m)); 
                });
            });
            
        });
    });   
}


export { setActiveDialog };