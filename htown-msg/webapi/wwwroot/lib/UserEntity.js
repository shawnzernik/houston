import { WebApiResponse } from "/lib/WebApiResponse.js";

export class UserEntity {
    constructor(guid, name) {
        this.guid = guid;
        this.name = name;
    }

    guid = null;
    name = null;

    copyFrom(original) {
        this.guid = original.guid;
        this.name = original.name;
    }

    save() {
        let request = {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(this)
        };

        return fetch("/user", request)
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
        return fetch("/users")
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
                    throw new Error("Load all users did not return an array!");

                let ret = [];
                response.content.forEach((row) => {
                    let newRow = new UserEntity();
                    newRow.copyFrom(row);
                    ret.push(newRow);
                });

                return ret;
            });
    }
    static load(guid) {
        return fetch("/user/" + guid)
            .then((response) => {
                if (!response.ok)
                    throw new Error("Status " + response.status + ": " + response.statusText);

                return response.json();
            })
            .then((json) => {
                let response = new WebApiResponse(json);
                if (response.error)
                    throw new Error(response.getMessages());

                let ret = new UserEntity();
                ret.copyFrom(response.content);
                return ret;
            });
    }
    static remove(guid) {
        if (!guid)
            return Promise.resolve().then(() => { return false; })

        return fetch("/user/" + guid, { method: "DELETE" })
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