import { Injectable } from "@angular/core";
import * as signalR from "@microsoft/signalr";
import { Message } from "../_models/message";
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SignalrService {
  connection: signalR.HubConnection;

  // Using BehaviorSubject to store and emit messages
  private messageSubject = new BehaviorSubject<Message[]>([]);
  public messages$ = this.messageSubject.asObservable(); // Expose as observable

  constructor() {
    this.connection = new signalR.HubConnectionBuilder()
      .withUrl("http://localhost:5045/chathub", {
        accessTokenFactory: () => localStorage.getItem("jwtToken")
      })
      .build();

    this.connection.on("ReceiveMessage", (username: string, content: string) => {
      console.log("Message received:", username, content); // Debug log
      const message: Message = {
        chatId: 'yourChatId',
        userId: username,
        content: content,
        timestamp: new Date(),
        userName: username
      };
      // Get the current messages and add the new one
      const currentMessages = this.messageSubject.value;
      this.messageSubject.next([...currentMessages, message]);
    });

    this.startConnection();
  }

  private startConnection() {
    this.connection.start()
      .then(() => console.log("SignalR connected"))
      .catch(err => console.error("Error while starting SignalR connection: ", err));
  }

  public sendMessage(chatId: string, user: string, message: string) {
    if (this.connection.state === signalR.HubConnectionState.Connected) {
      this.connection.invoke("SendMessage", user, message)
        .catch(err => console.error("Error while sending message: ", err))
    } else {
      console.error("SignalR connection is not established. Cannot send message.");
    }
  }
}
