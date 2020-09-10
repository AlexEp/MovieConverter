import { Injectable } from '@angular/core';
import { Subject, Observable } from 'rxjs';
import { AppEvent } from '../shared/app-event.model';

@Injectable({
  providedIn: 'root'
})
export class EventsService {
  private subjectEvent = new Subject<AppEvent>()
  constructor() { }

  public addEvent(event: AppEvent) {
    this.subjectEvent.next(event);
  }

  public getEvent(): Observable<AppEvent> {
    return this.subjectEvent.asObservable();
  } 
  
}
