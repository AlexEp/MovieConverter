import { Injectable, OnDestroy } from '@angular/core';
import { Subject, Observable, Subscription } from 'rxjs';
import { environment } from '../../environments/environment';
import * as signalR from "@aspnet/signalr";
import { ProccesEvent } from '../shared/procces-event.model';

@Injectable({
  providedIn: 'root'
})
export class SignalRService implements  OnDestroy {
  private subjectProccesEvent = new Subject<ProccesEvent>();
  private ConnectionStateEvent = new Subject<signalR.HubConnectionState>();
  subscriptionConnectionEvent: Subscription;
  constructor() { }

  private hubConnection: signalR.HubConnection

  public startConnection = () => {
    const self : SignalRService  = this;

    self.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${environment.backend.apiUrl}/apphub`)
      .build();

      self.startSignalR();

      self.hubConnection.onclose(function () {
      console.log('SignalR Connection disconnected');
      self.setConnectionStateEvent(self.hubConnection.state)
    });
  }

  private startSignalR() {
    this.hubConnection
      .start()
      .then(() => {
        console.log('SignalR Connection started');
        this.setConnectionStateEvent(this.hubConnection.state);
      })
      .catch(err => {
        console.log('Error while starting connection: ' + err);
        this.setConnectionStateEvent(this.hubConnection.state)});

        
      this.hubConnection.on('procces_event', (data) => {
          this.addEvent(data);
    });

      this.subscriptionConnectionEvent = this.getConnectionStateEvent().subscribe(state =>{
        if(state === signalR.HubConnectionState.Disconnected){
            this.reconnectsignalR(this);
        }  
      })    
  }

  private reconnectsignalR(caller) {

    if (!caller.isConnected()) {
      caller.hubConnection
        .start()
        .then(() => {
          console.log('SignalR Connection started');
        })
        .catch(err => {
          console.log('Error while starting connection: ' + err)
          setTimeout(function () {
            caller.reconnectsignalR(caller);
          }, 5000); // Restart connection after 5 seconds.
        })
    }



  }

  ngOnDestroy(): void {
    this.subscriptionConnectionEvent.unsubscribe();
  }

  public isConnected() : boolean {
    return this.hubConnection.state == signalR.HubConnectionState.Connected
  }

  //State events
  private setConnectionStateEvent(state : signalR.HubConnectionState): void {
    this.ConnectionStateEvent.next(state);
  }

 public getConnectionStateEvent(): Observable<signalR.HubConnectionState> {
   return this.ConnectionStateEvent.asObservable();
 }

  //Client events
  public addEvent(event: ProccesEvent) :  void {
    this.subjectProccesEvent.next(event);
  }

  public getEvent(): Observable<ProccesEvent> {
    return this.subjectProccesEvent.asObservable();
  }
}
