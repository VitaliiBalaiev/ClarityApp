import { Injectable } from "@angular/core";
import * as signalR from "@microsoft/signalr";
import { Message } from "../_models/message";
import { BehaviorSubject } from 'rxjs';
import { HttpClient } from "@angular/common/http";
import {User} from "../_models/user";

@Injectable({
  providedIn: 'root'
})
export class SignalrService {
  connection: signalR.HubConnection;
  hubApiUrl: string = 'http://localhost:5045/chathub';
  private userObj: User = JSON.parse(localStorage.getItem('user'));
  public messageSubject = new BehaviorSubject<Message[]>([]);
  public messages$ = this.messageSubject.asObservable();

  constructor(private http: HttpClient) {
    this.connection = new signalR.HubConnectionBuilder()
      .withUrl(this.hubApiUrl, {
        accessTokenFactory: () => this.userObj.token
      })
      .build();

    this.startConnection();

    this.connection.on("ReceiveMessage", (message: Message) => {
      console.log("Message received:", message.senderUsername, message.content);
      // Get the current messages and add the new one
      const currentMessages = this.messageSubject.value;
      this.messageSubject.next([...currentMessages, message]);
    });
  }

  private startConnection() {
    this.connection.start()
      .then(() => console.log("SignalR connected"))
      .catch(err => console.error("Error while starting SignalR connection: ", err));
  }

  public sendMessage(groupName: string, message: Message) {
    this.connection.invoke("SendMessage", groupName, message)
      .catch(err => console.error("Error while sending message: ", err));
  }

  public showChat(initiatorUser: string, recipientUser: string) {
    this.connection.invoke("ShowChat", initiatorUser, recipientUser)
      .then(() => console.log(`Chat initialized between ${initiatorUser} and ${recipientUser}`))
      .catch(err => console.error("Error while initializing chat:", err));
  }

  public getMessagesForChat(chatId: string){
    return this.http.get<Message[]>(`http://localhost:5045/api/message/${chatId}`);
  }
}
