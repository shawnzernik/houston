export class WebApiResponse {
    constructor(json) {
        this.content = json.content;
        this.error = json.error;
        this.trace = json.trace;
        this.inner = json.inner;
    }

    content = null;
    error = null;
    trace = null;
    inner = null;
}
