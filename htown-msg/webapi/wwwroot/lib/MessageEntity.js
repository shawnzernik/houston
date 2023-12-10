class Message {
    constructor(guid, toUser, created, content) {
        this.guid = guid;
        this.toUSer = toUser;
        this.created = created;
        this.content = content;
    }

    guid = null;
    toUser = null;
    created = new Date(Date.now);
    content = null;

    copyFrom(original) {
        this.guid = origin.guid;
        this.toUser = original.toUser;
        this.created = original.created;
        this.content = original.content;
    }

    static loadAll() {
        return fetch("/messages")
            .then((response) => {
                if (!response.ok)
                    throw new Error("Status " + response.status + ": " + response.statusText);

                return response.json();
            })
            .then((data) => {
                if (!Array.isArray(data))
                    throw new Error("Load all messages did not return an array!");

                let ret = [];
                data.forEach((row) => {
                    let newRow = new MessageEntity();
                    newRow.copyFrom(row);
                    ret.push(newRow);
                });

                return ret;
            })
    }
    save() { }
    remove() { }
}