import { UserEntity } from "/lib/UserEntity.js";

window.onload = () => {
    let guidInput = document.getElementById("guidInput");
    let nameInput = document.getElementById("nameInput");

    let entity = new UserEntity();

    let loadControlsFromEntity = () => {
        guidInput.value = entity.guid ?? "";
        nameInput.value = entity.name ?? "";
    };
    let loadEntityFromControls = () => {
        entity.name = nameInput.value;
    };

    let promises = [];

    let guid = new URLSearchParams(window.location.search).get("guid");
    if (guid && guid.length === 36)
        promises.push(
            UserEntity.load(guid)
                .then((user) => {
                    entity = user;
                })
        );

    Promise.all(promises)
        .then(() => {
            loadControlsFromEntity();
        })
        .catch((err) => {
            alert(err.message);
            window.location.assign("users.html");
        });

    document.getElementById("saveButton").onclick = () => {
        loadEntityFromControls();
        entity.save()
            .then((isChanged) => {
                if (isChanged)
                    window.location.assign("users.html");
                else
                    alert("No changes saved!");
            })
            .catch((err) => {
                alert(err.message);
            });
    };
    document.getElementById("deleteButton").onclick = () => {
        UserEntity.remove(entity.guid)
            .then((isChanged) => {
                if (isChanged)
                    window.location.assign("users.html");
                else
                    alert("Nothing deleted!");
            })
            .catch((err) => {
                alert(err.message);
            });
    };
    document.getElementById("backButton").onclick = () => {
        window.location.assign("users.html");
    };
};