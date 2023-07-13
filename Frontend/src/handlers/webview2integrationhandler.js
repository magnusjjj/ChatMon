export default class WebView2IntegrationHandler {
    static onmessagehandlers = {};

    static {
        window.chrome.webview.addEventListener('message', (message) => {
            console.log("Got message from webview2", message)
            if (message.data.type in this.onmessagehandlers) {
                this.onmessagehandlers[message.data.type].forEach((callback) => callback(message.data));
            }
        });
    }

    

    static SendMessage(data) {
        chrome.webview.postMessage(data);
    }

    static onMessage(type, callback) {
        if (!(type in this.onmessagehandlers)) {
            this.onmessagehandlers[type] = [];
        }

        this.onmessagehandlers[type].push(callback);
    }
}