import { UserEntity } from "/lib/UserEntity.js";

function windowLoaded() {
    UserEntity.loadAll()
        .then((users) => {
            let html = document.getElementById("template").outerHTML;

            users.forEach((user) => {
                let row = html.
                    replaceAll("{guid}", user.guid).
                    replaceAll("{name}", user.name).
                    replaceAll("style=\"visibility: hidden\"", "").
                    replaceAll("id=\"templare\"", "");

                document.getElementById("target").insertAdjacentHTML("beforeend", row);
            });
        })
        .catch((err) => {
            alert(err.message);
        });

}

window.onload = windowLoaded;