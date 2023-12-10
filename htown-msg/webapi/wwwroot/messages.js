import { UserEntity } from "./lib/UserEntity.js";
import { MessageEntity } from "/lib/MessageEntity.js";

window.onload = () => {
    let promises = [];

    let userMap = {};
    let messagesArray = [];

    promises.push(
        MessageEntity.loadAll()
            .then((messages) => {
                messagesArray = messages;
            })
    );
    promises.push(
        UserEntity.loadAll()
            .then((users) => {
                users.forEach((user) => {
                    userMap[user.guid] = user;
                });
            })
    )

    Promise.all(promises)
        .then(() => {
            let html = document.getElementById("template").outerHTML;

            messagesArray.forEach((message) => {
                let txt = message.content;
                if (message.content && message.content.length > 100)
                    txt = message.content.substring(0, 100);

                let row = html
                    .replaceAll("{guid}", message.guid)
                    .replaceAll("{toUser}", userMap[message.toUser].name)
                    .replaceAll("{created}", message.created.toString())
                    .replaceAll("{content}", txt)
                    .replaceAll("style=\"visibility: hidden\"", "")
                    .replaceAll("id=\"template\"", "");

                document.getElementById("target").insertAdjacentHTML("beforeend", row);
            });

            document.getElementById("template").remove();
        })
        .catch((err) => {
            alert(err.message);
        });

    document.getElementById("homeButton").onclick = () => {
        window.location.assign("index.html");
    };
    document.getElementById("addButton").onclick = () => {
        window.location.assign("message.html");
    }
    document.getElementById("deleteButton").onclick = () => {
        let selections = document.getElementsByClassName("selectCheck");

        let promises = [];
        for (let cnt = 0; cnt < selections.length; cnt++) {
            let checkBox = selections.item(cnt);
            if (checkBox.checked)
                promises.push(MessageEntity.remove(checkBox.value));
        }

        Promise.all(promises)
            .catch((err) => {
                alert(err.message);
            })
            .finally(() => {
                window.location.reload();
            })
    }
    document.getElementById("checkAll").onclick = () => {
        let selectAll = document.getElementById("checkAll");

        let selections = document.getElementsByClassName("selectCheck");
        for (let cnt = 0; cnt < selections.length; cnt++) {
            let checkBox = selections.item(cnt);
            checkBox.checked = selectAll.checked;
        }
    }
};