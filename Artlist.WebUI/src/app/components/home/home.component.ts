import { Component, OnInit, OnDestroy } from '@angular/core';
import { environment } from '../../../environments/environment';

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
import { ProcessConvertedFileResponse, ProcessThumbnailsResponse } from 'src/app/shared/file.model';


interface fileStatus {
  UploadFileId: string | null,
  UploadedStatus: ProcessStatusType | null,
  ConvertedStatus: ProcessStatusType | null,
  Thumbnails1sStatus: ProcessStatusType | null,
  Thumbnails3sStatus: ProcessStatusType | null

  ThumbnailsResponse1s : ProcessThumbnailsResponse,
  ThumbnailsResponse3s : ProcessThumbnailsResponse,
  ConvertedResponse : ProcessConvertedFileResponse,
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

    // this.serverApi.getFile(this.showTop).subscribe(r =>  this.UploadedStatusFiles = r);
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
          this.fileStatus.UploadedStatus = event.status;
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
        this.fileStatus.ConvertedStatus = event.status;
        if(event.status == ProcessStatusType.Failed){
          this.toastr.error(event.massege, "Convert Failed")
        }
      }
        break;
      case ProcessRequestType.CreateThumbnails:
      {
         if(isNumber(event.data?.request?.miliseconds) ){
            const num = <Number>event.data.request.miliseconds;

            if(+num === 1000) {
              this.fileStatus.Thumbnails1sStatus = event.status;
              this.fileStatus.ThumbnailsResponse1s = event.data;
            } 
            else if (+num === 3000){
              this.fileStatus.Thumbnails3sStatus = event.status;
              this.fileStatus.ThumbnailsResponse3s = event.data;
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

   getFileUrl(fileId: string | null): string{
    if(!fileId)
      return "";

    return `${environment.backend.apiUrl}\\resource\\${fileId}`
  }

  clearFileStatus() {
    this.fileStatus = {
      UploadedStatus: null, ConvertedStatus: null,UploadFileId : null,
      Thumbnails1sStatus: null, Thumbnails3sStatus: null,
      ConvertedResponse : null, ThumbnailsResponse1s: null, ThumbnailsResponse3s:null
    }
  }
  ngOnDestroy(): void {
    this.subscriptionEvent.unsubscribe();
  }
}
