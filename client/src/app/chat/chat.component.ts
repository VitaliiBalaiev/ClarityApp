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
    SearchbarComponent
  ],
  styleUrls: ['./chat.component.css']
})
export class ChatComponent implements OnInit, OnDestroy {
  public users: User[] = [];
  public currentUser!: User;
  public recipientUser: string = '';
  public messages: Message[] = [];
  public newMessage: string = '';
  private chatId: string = '';
  private subscriptions: Subscription = new Subscription();

  @ViewChild('messageList') private messageList: ElementRef;


  constructor(
    private signalrService: SignalrService,
    private sharedResourcesService: SharedResourcesService,
    private userService: UserService
  ) {}

  ngOnInit(): void {
    this.loadCurrentUser();
    this.initializeSubscriptions();
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

  private scrollToBottom(){
    try{
      this.messageList.nativeElement.scrollTop = this.messageList.nativeElement.scrollHeight;
    } catch(err){
      console.error("Failed to scroll message: ", err);
    }
  }
}
