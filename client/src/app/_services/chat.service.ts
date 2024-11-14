import {Injectable, OnInit} from '@angular/core';
import {Chat} from "../_models/chat";
import {BehaviorSubject} from "rxjs";
import {HttpClient} from "@angular/common/http";
import {UserService} from "./user.service";

@Injectable({
  providedIn: 'root'
})
export class ChatService {
  baseApiUrl: string = 'http://localhost:5045/api';
  private chatsSubject = new BehaviorSubject<Chat[]>([]);
  chats$ = this.chatsSubject.asObservable();

  constructor(private http: HttpClient) {
  }

  loadChats(username: string) {
    this.http.get<string[]>(`${this.baseApiUrl}/chats/${username}`).subscribe({
      next: (usernames) => {
        // Transform the list of usernames into an array of Chat objects
        const chats = usernames.map((username) => ({ username })); // Wrap each string in a Chat object
        console.log('Fetched chats (transformed):', chats); // Log the transformed chats
        this.chatsSubject.next(chats);  // Update the subject with the transformed data
      },
      error: (err) => {
        console.error('Error fetching chats:', err);
      },
    });
  }

}


