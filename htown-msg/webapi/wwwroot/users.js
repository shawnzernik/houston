import { UserEntity } from "/lib/UserEntity.js";

window.onload = () => {
    UserEntity.loadAll()
        .then((users) => {
            let html = document.getElementById("template").outerHTML;

            users.forEach((user) => {
                let row = html
                    .replaceAll("{guid}", user.guid)
                    .replaceAll("{name}", user.name)
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
        window.location.assign("user.html");
    };
    document.getElementById("deleteButton").onclick = () => {
        let selections = document.getElementsByClassName("selectCheck");

        let promises = [];
        for (let cnt = 0; cnt < selections.length; cnt++) {
            let checkBox = selections.item(cnt);
            if (checkBox.checked)
                promises.push(UserEntity.remove(checkBox.value));
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
    };
};