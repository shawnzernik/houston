export class WebApiResponse {
    constructor(json) {
        this.content = json.content;
        this.error = json.error;
        this.trace = json.trace;

        if (json.inner)
            this.inner = new WebApiResponse(json.inner);
    }

    content = null;
    error = null;
    trace = null;
    inner = null;

    getMessages() {
        let msg = this.error;

        if (this.inner)
            msg = msg += "\n\n " + this.inner.getMessages();

        return msg;
    }
}
