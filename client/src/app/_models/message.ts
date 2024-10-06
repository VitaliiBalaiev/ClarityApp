export interface Message {
  chatId: string;
  userId: string;
  content: string;
  timestamp: Date;
  userName?: string;
}
