import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { AuthService } from 'src/AuthService';
import * as signalR from '@microsoft/signalr';
import { environment } from 'src/environments/environment';
import { Message } from './Messages';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css']
})
export class ChatComponent implements OnInit, OnDestroy {
  public messages: Message[] = [];
  form = new FormGroup({
    message: new FormControl('', { nonNullable: true }),
  });

  private hubConnection: signalR.HubConnection | undefined = undefined;

  constructor(
    private authService: AuthService,
    private http: HttpClient
  ) { }

  userName: string = "";
  userEmail: string = "";

  async ngOnInit() {
    this.getMessages().subscribe((response: Message[]) => {
      this.messages = response;
    });

    let token = this.authService.getToken();
    this.userName = this.authService.getUserName();
    this.userEmail = this.authService.getUserEmail();

    this.hubConnection = new signalR.HubConnectionBuilder()
      .configureLogging(signalR.LogLevel.Information)
      .withUrl(environment.baseUrl + 'chat?access_token=' + token, )
      .build();

    await this.hubConnection.start().then(function () {
      console.log('SignalR Connected!');
    }).catch(function (err) {
      return console.error(err.toString());
    });

    this.hubConnection.on("messageReceived", (userName, message) => {
      let mess: Message = {
        userName: userName,
        message: message
      };
      this.messages.push(mess);
    });
  }

  ngOnDestroy() {
  }

  sendMessage() {
    let message = this.form.controls.message.value;
    if (message) {
      let userName = this.authService.getUserName();
      this.hubConnection?.send("PostMessage", userName, message);
    }
    this.form.controls.message.setValue("");
  }

  getMessages(): Observable<Message[]> {
    let token = this.authService.getToken();
    let httpOptions = {
      headers: new HttpHeaders({ 'Content-Type': 'application/json', 'Authorization': "Bearer " + token })
    };
    return this.http.get<Message[]>(environment.baseUrl + 'api/Messages', httpOptions).pipe();
  }

  logout() {
    this.authService.logout();
  }
}
