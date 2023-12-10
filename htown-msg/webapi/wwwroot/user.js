import { UserEntity } from "/lib/UserEntity.js";

window.onload = () => {
    let guidInput = document.getElementById("guidInput");
    let nameInput = document.getElementById("nameInput");

    let userEntity = new UserEntity();

    let loadControlsFromEntity = () => {
        guidInput.value = userEntity.guid ?? "";
        nameInput.value = userEntity.name ?? "";
    };
    let loadEntityFromControls = () => {
        userEntity.name = nameInput.value;
    };

    let guid = new URLSearchParams(window.location.search).get("guid");
    if (typeof (guid) === "string" && guid.length === 36)
        UserEntity.load(guid)
            .then((user) => {
                userEntity = user;
                loadControlsFromEntity();
            })
            .catch((err) => {
                alert(err.message);
                window.location.assign("users.html");
            });
    else
        loadControlsFromEntity();

    document.getElementById("saveButton").onclick = () => {
        loadEntityFromControls();
        userEntity.save()
            .then((ret) => {
                window.location.assign("users.html");
            })
            .catch((err) => {
                alert(err.message);
            });
    };
    document.getElementById("deleteButton").onclick = () => {
        UserEntity.remove(userEntity.guid)
            .then((ret) => {
                window.location.assign("users.html");
            })
            .catch((err) => {
                alert(err.message);
            });
    };
};