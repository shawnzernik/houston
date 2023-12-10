import { MessageEntity } from "/lib/MessageEntity.js";
import { UserEntity } from "/lib/UserEntity.js";

window.onload = () => {
    let guidInput = document.getElementById("guidInput");
    let toUserSelect = document.getElementById("toUserSelect");
    let createdInput = document.getElementById("createdInput");
    let contentText = document.getElementById("contentText");

    let entity = new MessageEntity();
    let users = [];

    let loadControlsFromEntity = () => {
        guidInput.value = entity.guid ?? "";
        createdInput.value = entity.created;
        contentText.value = entity.content ?? "";

        let html = "<option value=\"\">Select a contact...</option>";
        users.forEach((user) => {
            html += "<option value=\"" + user.guid + "\" ";
            if (entity.toUser === user.guid)
                html += "selected";
            html += " >" + user.name + "</option>";
        });
        toUserSelect.innerHTML = html;
    }
    let loadEntityFromControls = () => {
        entity.toUser = toUserSelect.value;
        //entity.created = createdInput.value;
        entity.content = contentText.value;
    }

    let promises = [];

    promises.push(
        UserEntity.loadAll()
            .then((list) => {
                users = list;
            })
    );

    let guid = new URLSearchParams(window.location.search).get("guid");
    if (guid && guid.length === 36)
        promises.push(
            MessageEntity.loadGuid(guid)
                .then((message) => {
                    entity = message;
                })
        );

    Promise.all(promises)
        .then(() => {
            loadControlsFromEntity();
        })
        .catch((err) => {
            alert(err.message);
            window.location.assign("messages.html");
        });

    document.getElementById("saveButton").onclick = () => {
        loadEntityFromControls();
        entity.save()
            .then((isChanged) => {
                if (isChanged)
                    window.location.assign("messages.html");
                else
                    alert("No changes saved!");
            })
            .catch((err) => {
                alert(err.message);
            });
    };
    document.getElementById("deleteButton").onclick = () => {
        MessageEntity.remove(entity.guid)
            .then((isChanged) => {
                if (isChanged)
                    window.location.assign("messages.html");
                else
                    alert("Nothing deleted!");
            })
            .catch((err) => {
                alert(err.message);
            });
    };
    document.getElementById("backButton").onclick = () => {
        window.location.assign("messages.html");
    };

};