import * as signalR from "@microsoft/signalr";

class SignalRService {
    connection = null;

    async start() {
        this.connection = new signalR.HubConnectionBuilder()
            .withUrl("https://localhost:5001/gamehub")
            .withAutomaticReconnect()
            .build();

        await this.connection.start();
    }

    async sendToAll(message) {
        await this.connection.invoke("SendToAll", message);
    }

    async sendToGroup(groupId, message) {
        await this.connection.invoke("SendToGroup", groupId, message);
    }

    async sendToUser(userId, message) {
        await this.connection.invoke("SendToUser", userId, message);
    }

    onReceiveMessage(callback) {
        this.connection.on("ReceiveMessage", callback);
    }

    offReceiveMessage() {
        this.connection.off("ReceiveMessage");
    }
}

export default new SignalRService();