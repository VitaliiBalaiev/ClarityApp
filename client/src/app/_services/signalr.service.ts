import { Injectable } from "@angular/core";
import * as signalR from "@microsoft/signalr";
import { Message } from "../_models/message";
import { BehaviorSubject } from 'rxjs';
import { HttpClient } from "@angular/common/http";
import { UserService } from './user.service'; // Import UserService

@Injectable({
  providedIn: 'root'
})
export class SignalrService {
  private connection: signalR.HubConnection;
  private hubApiUrl: string = 'http://localhost:5045/chathub';
  private messageSubject = new BehaviorSubject<Message[]>([]);
  public messages$ = this.messageSubject.asObservable();

  constructor(private http: HttpClient, private userService: UserService) {
    this.initializeConnection();
  }

  private initializeConnection(): void {
    const userObj = this.userService.getCurrentUser(); // Get user data from UserService

    if (userObj) {
      this.connection = new signalR.HubConnectionBuilder()
        .withUrl(this.hubApiUrl, {
          accessTokenFactory: () => userObj.token
        })
        .build();

      this.startConnection();
      this.registerMessageHandler();
    } else {
      console.error("User not found for SignalR connection.");
    }
  }

  private startConnection(): void {
    this.connection.start()
      .then(() => console.log("SignalR connected"))
      .catch(err => console.error("Error while starting SignalR connection: ", err));
  }

  private registerMessageHandler(): void {
    this.connection.on("ReceiveMessage", (message: Message) => {
      console.log("Message received:", message.senderUsername, message.content);
      const currentMessages = this.messageSubject.value;
      this.messageSubject.next([...currentMessages, message]);
    });
  }

  public sendMessage(groupName: string, message: Message): void {
    this.connection.invoke("SendMessage", groupName, message)
      .catch(err => console.error("Error while sending message: ", err));
  }

  public openChat(initiatorUser: string, recipientUser: string): void {
    this.connection.invoke("ShowChat", initiatorUser, recipientUser)
      .then(() => console.log(`Chat initialized between ${initiatorUser} and ${recipientUser}`))
      .catch(err => console.error("Error while initializing chat:", err));
  }

  public getMessagesForChat(chatId: string) {
    return this.http.get<Message[]>(`http://localhost:5045/api/message/${chatId}`);
  }
}
