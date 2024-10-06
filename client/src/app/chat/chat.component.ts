import { ChangeDetectorRef, Component, OnInit, OnDestroy } from '@angular/core';
import { SignalrService } from '../_services/signalr.service';
import { Message } from '../_models/message';
import { FormsModule } from "@angular/forms";
import { DatePipe, NgForOf } from "@angular/common";
import { Subject, takeUntil } from 'rxjs';
import {User} from "../_models/user";

@Component({
  selector: 'app-chat',
  standalone: true,
  templateUrl: './chat.component.html',
  imports: [
    FormsModule,
    NgForOf,
    DatePipe
  ],
  styleUrls: ['./chat.component.css']
})
export class ChatComponent implements OnInit {
  public newMessage: string = '';
  public messages: Message[] = [];
  public chatId: string = '';
  public userObj: User = JSON.parse(localStorage.getItem('user'));
  public currentUsername: string = this.userObj.username;
  public chats: string[] = [];
  constructor(private signalrService: SignalrService) {}

  ngOnInit() {
    this.loadChats();

    // Subscribe to messages from SignalR, ensuring only messages for the selected chat are loaded
    this.signalrService.messages$.pipe().subscribe((messages) => {
      this.messages = messages;
    });
  }

  // Load available chats (this could be fetched from your API)
  loadChats() {
    // Example data; replace with actual chat fetching logic
    this.chats = []; // Example chat IDs
    this.chatId = this.chats[0]; // Default to the first chat
    this.loadMessages(this.chatId); // Load messages for the default chat
  }

  // Switch between chats
  switchChat(chatId: string) {
    this.chatId = chatId; // Set the current chat ID
    this.loadMessages(chatId); // Load messages for the selected chat
  }

  // Handle sending a message
  sendMessage() {
    if (this.newMessage.trim()) {
      this.signalrService.sendMessage(this.chatId, this.currentUsername, this.newMessage);
      this.newMessage = ''; // Clear input after sending
    }
  }

  // Load messages for the selected chat
  loadMessages(chatId: string) {
    // Filter messages for the selected chat
    this.signalrService.messages$.pipe().subscribe((messages) => {
      this.messages = messages.filter(message => message.chatId === chatId);
    });
  }
}
