import { Component, OnInit, OnDestroy, ViewChild, ElementRef, AfterViewChecked } from '@angular/core';
import { SignalrService } from '../_services/signalr.service';
import { SharedResourcesService } from '../_services/shared-resources.service';
import { UserService } from '../_services/user.service';
import { Message } from '../_models/message';
import { User } from '../_models/user';
import { Subscription } from 'rxjs';
import { FormsModule } from "@angular/forms";
import {DatePipe, NgClass, NgForOf, NgIf} from "@angular/common";
import {SearchbarComponent} from "../searchbar/searchbar.component";
import {LocalTimePipe} from "../pipes/localtime.pipe";
import {Chat} from "../_models/chat";
import {ChatService} from "../_services/chat.service";


@Component({
  selector: 'app-chat',
  standalone: true,
  templateUrl: './chat.component.html',
  imports: [
    FormsModule,
    NgIf,
    NgForOf,
    NgClass,
    DatePipe,
    SearchbarComponent,
    LocalTimePipe
  ],
  styleUrls: ['./chat.component.css']
})
export class ChatComponent implements OnInit, OnDestroy {
  public users: User[] = [];
  public currentUser!: User;
  public recipientUser: string = '';
  public messages: Message[] = [];
  public newMessage: string = '';
  public chats: Chat[] = [];
  private chatId: string = '';
  private subscriptions: Subscription = new Subscription();

  @ViewChild('messageList') private messageList: ElementRef;

  constructor(
    private signalrService: SignalrService,
    private sharedResourcesService: SharedResourcesService,
    private userService: UserService,
    private chatService: ChatService
  ) {}

  ngOnInit(): void {
    this.loadCurrentUser();
    this.initializeSubscriptions();
    this.chatService.loadChats(this.currentUser.username);
  }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }

  ngAfterViewChecked(){
    this.scrollToBottom();
  }

  private loadCurrentUser(): void {
    this.currentUser = this.userService.getCurrentUser();
    if (!this.currentUser) {
      console.error('User not found in UserService');
    }
  }

  private initializeSubscriptions(): void {
    this.subscriptions.add(
      this.sharedResourcesService.currentResource.subscribe({
        next: users => this.users = users,
        error: err => console.error('Error fetching users:', err),
      })
    );

    this.subscriptions.add(
      this.signalrService.messages$.subscribe({
        next: messages => this.messages = messages,
        error: err => console.error('Error fetching messages:', err),
      })
    );

    this.subscriptions.add(
      this.chatService.chats$.subscribe({
        next: chats => {
          this.chats = chats;
        },
        error: err => console.error('Error fetching chats:', err),
      })
    );
  }

  public openChat(recipient: string): void {
    this.recipientUser = recipient;
    this.chatId = this.generateChatId();
    this.signalrService.openChat(this.currentUser.username, recipient);
  }

  public sendMessage(): void {
    if (this.isMessageValid(this.newMessage)) {
      const message = this.createMessage();
      this.signalrService.sendMessage(this.chatId, message);
      this.newMessage = '';
    }
  }

  private isMessageValid(content: string): boolean {
    return content.trim().length > 0;
  }

  private createMessage(): Message {
    return {
      chatId: this.chatId,
      senderId: this.currentUser.id,
      content: this.newMessage,
      timestamp: new Date(),
      senderUsername: this.currentUser.username,
    };
  }

  private generateChatId(): string {
    const participants = [this.currentUser.username, this.recipientUser].sort();
    return `${participants[0]}_${participants[1]}_chat`;
  }

  private scrollToBottom(): void {
    if (this.messageList) {
      try {
        this.messageList.nativeElement.scrollTop = this.messageList.nativeElement.scrollHeight;
      } catch (err) {
        console.error("Failed to scroll message:", err);
      }
    }
  }
}
