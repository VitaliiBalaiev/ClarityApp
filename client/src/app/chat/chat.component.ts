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
  public chatId: string = '';
  public messages: Message[] = [];
  public newMessage: string = '';
  public groupName: string = '';

  constructor(private signalrService: SignalrService, private sharedResourcesService: SharedResourcesService) {}

  ngOnInit() {
    this.sharedResourcesService.currentResource.subscribe(users => {
      this.users = users;
    })
    this.loadChats();

    // Subscribe to messages from SignalR, ensuring only messages for the selected chat are loaded
    this.signalrService.messages$.pipe().subscribe((messages) => {
      this.messages = messages;
    });
  }

  // Load available chats (this could be fetched from your API)
  loadChats() {
    // Example data; replace with actual chat fetching logic
    //this.chats = []; // Example chat IDs
    //this.chatId = this.chats[0]; // Default to the first chat
    //this.loadMessages(this.chatId); // Load messages for the default chat
    this.chats = this.users.map(user => user.username);
  }

  // Switch between chats
  switchChat(chatId: string) {
    this.chatId = chatId; // Set the current chat ID
    this.loadMessages(chatId); // Load messages for the selected chat
  }

  showChat(initiatorUser: string, recipientUser: string) {
    this.signalrService.showChat(initiatorUser, recipientUser);
    this.recipientUser = recipientUser;
    this.groupName = this.setGroupName();
  }
  // Handle sending a message
  sendMessage() {
    if (this.newMessage.trim()) {
      this.signalrService.sendMessage(this.groupName, this.newMessage);
      this.newMessage = ''; // Clear input after sending
    }
  }

  setGroupName(){
    const users = [this.currentUser, this.recipientUser];
    users.sort();
    return this.groupName = users[0] + "_" + users[1] + "_chat";
  }

  // Load messages for the selected chat
  loadMessages(chatId: string) {
    // Filter messages for the selected chat
    this.signalrService.messages$.pipe().subscribe((messages) => {
      this.messages = messages.filter(message => message.chatId === chatId);
    });
  }
}
