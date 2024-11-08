import {Injectable, OnInit} from "@angular/core";
import * as signalR from "@microsoft/signalr";
import { Message } from "../_models/message";
import { BehaviorSubject } from 'rxjs';
import { HttpClient } from "@angular/common/http";
import { UserService } from './user.service';

@Injectable({
  providedIn: 'root'
})
export class SignalrService implements OnInit{
  private connection: signalR.HubConnection;
  private hubApiUrl: string = 'http://localhost:5045/chathub';
  private messageSubject = new BehaviorSubject<Message[]>([]);
  public messages$ = this.messageSubject.asObservable();

  constructor(private http: HttpClient, private userService: UserService) {
    this.initializeConnection();
  }

  ngOnInit(){
    this.initializeConnection();
  }

  private initializeConnection(): void {
    const userObj = this.userService.getCurrentUser();
    if (userObj) {
      this.connection = new signalR.HubConnectionBuilder()
        .withUrl(this.hubApiUrl, {
          accessTokenFactory: () => userObj.token
        })
        .build();

      this.registerMessageHandler();
      this.startConnection();
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

  public openChat(initiatorUser: string, recipientUser: string): void {
    this.connection.invoke("ShowChat", initiatorUser, recipientUser)
      .then(() => {
        console.log(`Chat initialized between ${initiatorUser} and ${recipientUser}`);
        this.loadChatMessages(this.generateChatId(initiatorUser, recipientUser));
      })
      .catch(err => console.error("Error while initializing chat:", err));
  }

  private loadChatMessages(chatId: string): void {
    this.http.get<Message[]>(`http://localhost:5045/api/message/${chatId}`).subscribe({
      next: messages => {
        this.messageSubject.next(messages);
      },
      error: err => console.error('Error loading chat messages:', err),
    });
  }

  public sendMessage(groupName: string, message: Message): void {
    this.connection.invoke("SendMessage", groupName, message)
      .catch(err => console.error("Error while sending message: ", err));
  }

  private generateChatId(user1: string, user2: string): string {
    const participants = [user1, user2].sort();
    return `${participants[0]}_${participants[1]}_chat`;
  }
}
