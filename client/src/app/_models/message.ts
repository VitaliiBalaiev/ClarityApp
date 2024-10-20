export interface Message {
  chatId: string;
  senderId: string;
  content: string;
  timestamp: Date;
  senderUsername?: string;
}
