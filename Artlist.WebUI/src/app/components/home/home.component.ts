import { Component, OnInit, OnDestroy } from '@angular/core';

//plugins
import { ToastrService } from 'ngx-toastr';

//App services
import { SignalRService } from 'src/app/services/signal-r.service';
import { Subscription } from 'rxjs';
import { ProccesEvent } from 'src/app/shared/procces-event.model';
import { EventsService } from 'src/app/services/events.service';
import { AppEventTypes  } from 'src/app/shared/app-event.model';
import { ProcessRequestType, ProcessStatusType } from 'src/app/shared/constans';
import { isNumber } from 'util';

interface fileStatus {
  UploadFileId: string | null,
  Uploaded: ProcessStatusType | null,
  Converted: ProcessStatusType | null,
  Thumbnails1s: ProcessStatusType | null,
  Thumbnails3s: ProcessStatusType | null
};



@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit, OnDestroy {

  fileStatus: fileStatus = null;

  constructor(
    private toastr: ToastrService,
    public signalRService: SignalRService,
    private eventsService: EventsService) { }

  subscriptionEvent: Subscription;
  subscriptionAppEvent: Subscription;

  ngOnInit(): void {

    // this.serverApi.getFile(this.showTop).subscribe(r =>  this.uploadedFiles = r);
    this.clearFileStatus();
    this.signalRService.startConnection();
    this.subscriptionEvent = this.signalRService.getEvent().subscribe(event => {
      this.handleEvent(event);
    });
    this.subscriptionAppEvent = this.eventsService.getEvent().subscribe(event => {
      if (event.type == AppEventTypes.FileUploadStart) {
        this.clearFileStatus()
      }else if (event.type == AppEventTypes.FileUploadEnd){
        // if(event && event.data && event.data.Id) {
        //     this.fileStatus.UploadFileId = event.data.Id;
        // }
      }
    });

  }
  
  isServerConnected() : boolean{
    return this.signalRService.isConnected();
  }
  handleEvent(event: ProccesEvent) {

    //if the message belong to the current file
    if(event.type !==  ProcessRequestType.FileUpload && this.fileStatus.UploadFileId !== event.fileId)
       return;

    switch (event.type) {
      case ProcessRequestType.FileUpload:
        {
          this.fileStatus.Uploaded = event.status;
          if(event.status == ProcessStatusType.Failed){
              this.toastr.error(event.massege, "Upload Failed")
          }
          else if (event.status == ProcessStatusType.Completed){
            this.fileStatus.UploadFileId = event.fileId
          }
        }
        break;
      case ProcessRequestType.ConvertFile:
      {
        this.fileStatus.Converted = event.status;
        if(event.status == ProcessStatusType.Failed){
          this.toastr.error(event.massege, "Convert Failed")
        }
      }
        break;
      case ProcessRequestType.CreateThumbnails:
      {
         if(isNumber(event.data) ){
            const num = <Number>event.data;

            if(+num === 1000) {
              this.fileStatus.Thumbnails1s = event.status;
            } 
            else if (+num === 3000){
              this.fileStatus.Thumbnails3s = event.status;
            }

            if(event.status == ProcessStatusType.Failed){
              this.toastr.error(event.massege, "Thumbnail Failed")
            }
         }
      }
        //this.fileStatus. = event.status;
        break;
      case ProcessRequestType.FullProcess:
      
        break;
      default:
    
    }
  }

  clearFileStatus() {
    this.fileStatus = {
      Uploaded: null, Converted: null,UploadFileId : null,
      Thumbnails1s: null, Thumbnails3s: null
    }
  }
  ngOnDestroy(): void {
    this.subscriptionEvent.unsubscribe();
  }
}
