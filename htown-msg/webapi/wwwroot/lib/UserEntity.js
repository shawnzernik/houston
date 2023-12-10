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
                let webApiResponse = new WebApiResponse(json);
                if (webApiResponse.error)
                    throw new Error(webApiResponse.getMessages());

                return webApiResponse.content;
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
                let webApiResponse = new WebApiResponse(json);
                if (webApiResponse.error)
                    throw new Error(webApiResponse.getMessages());

                let ret = new UserEntity();
                ret.copyFrom(webApiResponse.content);
                return ret;
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
                let webApiResponse = new WebApiResponse(json);
                if (webApiResponse.error)
                    throw new Error(webApiResponse.getMessages());

                if (!Array.isArray(webApiResponse.content))
                    throw new Error("Load all users did not return an array!");

                let ret = [];
                webApiResponse.content.forEach((row) => {
                    let newRow = new UserEntity();
                    newRow.copyFrom(row);
                    ret.push(newRow);
                });

                return ret;
            })
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
                let webApiResponse = new WebApiResponse(json);
                if (webApiResponse.error)
                    throw new Error(webApiResponse.getMessages());

                return WebApiReponse.content;
            });
    }
}