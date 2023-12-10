import { MessageEntity } from "/lib/MessageEntity.js";

window.onload = () => {
    let promises = [];

    promises.push(
        MessageEntity.loadAll()
            .then()
    );
};