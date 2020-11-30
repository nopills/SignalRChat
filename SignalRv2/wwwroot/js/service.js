import * as core from '../js/core.js';

function setActiveDialog(connection) {
    let dialogList = document.querySelectorAll('.group-item');
   
    dialogList.forEach(e => {
        e.addEventListener('click', () => {
            e.preventDefaut;        

            document.querySelectorAll('.message-item').forEach(x => x.parentElement.removeChild(x));

            dialogList.forEach(i =>  i.classList.remove('group-item-active'));
           
            e.classList.add('group-item-active');
            let dialogId = e.querySelector('.dialogId').innerHTML;
            let senderId = e.querySelector('.senderId').innerHTML;
            connection.invoke("GetMessages", dialogId).then((msg) => {
                msg.forEach(m => {
                    m.userId == senderId ? core.AddOutgoingMessage(m) : core.AddIncomingMessage(m);
                });
            });
        });
    });   
}


export { setActiveDialog };