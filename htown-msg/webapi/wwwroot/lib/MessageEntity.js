import { WebApiResponse } from "/lib/WebApiResponse.js";

export class MessageEntity {
    constructor(guid, toUser, created, content) {
        this.guid = guid;
        this.toUser = toUser;
        this.created = created ?? new Date().toISOString();
        this.content = content;
    }

    guid = null;
    toUser = null;
    created = new Date(Date.now);
    content = null;

    copyFrom(original) {
        this.guid = original.guid;
        this.toUser = original.toUser;
        this.created = original.created;
        this.content = original.content;
    }

    save() {
        let request = {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(this)
        };

        return fetch("/message", request)
            .then((response) => {
                if (!response.ok)
                    throw new Error("Status " + response.status + ": " + response.statusText);

                return response.json();
            })
            .then((json) => {
                let response = new WebApiResponse(json);
                if (response.error)
                    throw new Error(response.getMessages());

                return response.content;
            });
    }

    static loadAll() {
        return fetch("/messages")
            .then((response) => {
                if (!response.ok)
                    throw new Error("Status " + response.status + ": " + response.statusText);

                return response.json();
            })
            .then((json) => {
                let response = new WebApiResponse(json);
                if (response.error)
                    throw new Error(response.getMessages());

                if (!Array.isArray(response.content))
                    throw new Error("Load all messages did not return an array!");

                let ret = [];
                response.content.forEach((row) => {
                    let newRow = new MessageEntity();
                    newRow.copyFrom(row);
                    ret.push(newRow);
                });

                return ret;
            });
    }
    static loadGuid(guid) {
        return fetch("/message/" + guid)
            .then((response) => {
                if (!response.ok)
                    throw new Error("Status " + response.status + ": " + response.statusText);

                return response.json();
            })
            .then((json) => {
                let response = new WebApiResponse(json);
                if (response.error)
                    throw new Error(response.getMessages());

                let ret = new MessageEntity();
                ret.copyFrom(response.content);
                return ret;
            });
    }
    static remove(guid) {
        if (!guid)
            return Promise.resolve().then(() => { return false; })

        return fetch("/message/" + guid, { method: "DELETE" })
            .then((response) => {
                if (!response.ok)
                    throw new Error("Status " + response.status + ": " + response.statusText);

                return response.json();
            })
            .then((json) => {
                let response = new WebApiResponse(json);
                if (response.error)
                    throw new Error(response.getMessages());

                return response.content;
            });
    }
}