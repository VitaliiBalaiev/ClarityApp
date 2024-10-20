import { Component, OnInit} from '@angular/core';
import { SignalrService } from '../_services/signalr.service';
import { Message } from '../_models/message';
import { FormsModule } from "@angular/forms";
import {DatePipe, NgForOf, NgOptimizedImage} from "@angular/common";
import {User} from "../_models/user";
import {SearchbarComponent} from "../searchbar/searchbar.component";
import {SharedResourcesService} from "../_services/shared-resources.service";

@Component({
  selector: 'app-chat',
  standalone: true,
  templateUrl: './chat.component.html',
  imports: [
    FormsModule,
    NgForOf,
    DatePipe,
    NgOptimizedImage,
    SearchbarComponent
  ],
  styleUrls: ['./chat.component.css']
})
export class ChatComponent implements OnInit {
  public users: User[] = [];
  public userObj: User = JSON.parse(localStorage.getItem('user'));
  public currentUser: string = this.userObj.username;
  public recipientUser: string;
  public chats: string[] = [];
  public messages: Message[] = [];
  public newMessage: string = '';
  public chatId: string = '';

  constructor(private signalrService: SignalrService, private sharedResourcesService: SharedResourcesService) {}

  ngOnInit() {
    this.sharedResourcesService.currentResource.subscribe(users => {
      this.users = users;
    })

    // Subscribe to messages from SignalR, ensuring only messages for the selected chat are loaded
    this.signalrService.messages$.pipe().subscribe((messages) => {
      this.messages = messages;
    });
  }

  showChat(recipientUser: string) {
    this.signalrService.showChat(this.currentUser, recipientUser);
    this.recipientUser = recipientUser;
    this.chatId = this.setChatId();
  }

  sendMessage() {
    if (this.newMessage.trim()) {
      this.signalrService.sendMessage(this.chatId, this.createMessage());
      this.newMessage = '';
    }
  }

  createMessage(): Message {
    return{
      chatId: this.chatId,
      senderId: "2",
      content: this.newMessage,
      timestamp: new Date(),
      senderUsername: this.currentUser,
    }
  }

  setChatId(){
    const users = [this.currentUser, this.recipientUser];
    users.sort();
    return this.chatId = `${users[0]}_${users[1]}_chat`;
  }

  loadMessages(chatId: string) {
    this.signalrService.messages$.pipe().subscribe((messages) => {
      this.messages = messages.filter(message => message.chatId === chatId);
    });
  }
}
