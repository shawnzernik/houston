export class UserEntity {
    constructor(guid, name) {
        this.#guid = guid;
        this.#name = name;
    }

    #guid = null;
    get guid() { return this.#guid; }
    set guid(value) { this.#guid = value; }

    #name = null;
    get name() { return this.#name; }
    set name(value) { this.#name = value; }

    copyFrom(original) {
        this.guid = original.guid;
        this.name = original.name;
    }

    static loadAll() {
        return fetch("/users").then((resp) => {
            if (!resp.ok)
                throw new Error("Status " + resp.status + ": " + resp.statusText);

            return resp.json();
        }).then((data) => {
            if (!Array.isArray(data))
                throw new Error("Load all users (/users) did not return an array!");

            let ret = [];
            data.forEach((row) => {
                let newUser = new UserEntity();
                newUser.copyFrom(row);
                ret.push(newUser);
            });

            return ret;
        })
    }
    save() { }
    remove() { }

}