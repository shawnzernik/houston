class Message {
    constructor(guid, toUser, created, content) {
        this.#guid = guid;
        this.#toUSer = toUser;
        this.#created = created;
        this.#content = content;
    }

    get guid() { return this.#guid; }
    set guid(value) { this.#guid = value; }

    get toUser() { return this.#toUser; }
    set toUser(value) { this.#toUser = value; }

    get created() { return this.#created; }
    set created(value) { this.#created = value; }

    get content() { return this.#content; }
    set content(value) { this.#content = value; }

    static loadAll() {
        
    }
    save() { }
    remove() { }
}