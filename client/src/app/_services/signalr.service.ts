import { Injectable } from "@angular/core";
import * as signalR from "@microsoft/signalr";
import { Message } from "../_models/message";
import { BehaviorSubject } from 'rxjs';
import { HttpClient } from "@angular/common/http";
import {ChatComponent} from "../chat/chat.component";
import {Chat} from "../_models/chat";

@Injectable({
  providedIn: 'root'
})
export class SignalrService {
  connection: signalR.HubConnection;
  baseApiUrl: string = 'http://localhost:5045/api/';
  private userObj = JSON.parse(localStorage.getItem('user'));
  private messageSubject = new BehaviorSubject<Message[]>([]);
  public messages$ = this.messageSubject.asObservable();

  constructor(private http: HttpClient) {
    this.connection = new signalR.HubConnectionBuilder()
      .withUrl("http://localhost:5045/chathub", {
        accessTokenFactory: () => this.userObj.token
      })
      .build();

    this.connection.on("ReceiveMessage", (username: string, content: string) => {
      console.log("Message received:", username, content);
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

  public sendMessage(user: string, message: string) {
    if (this.connection.state === signalR.HubConnectionState.Connected) {
      this.connection.invoke("SendMessage", user, message)
        .catch(err => console.error("Error while sending message: ", err));
      const Message: Message = {
        chatId: "123",
        userName: user,
        content: message,
        timestamp: new Date(),
        userId: "1"
      }
    } else {
      console.error("SignalR connection is not established. Cannot send message.");
    }
  }
}
