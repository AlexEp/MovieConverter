import { Component, OnInit, OnDestroy, Input } from '@angular/core';
import { environment } from '../../../environments/environment';

//plugins
import { ToastrService } from 'ngx-toastr';
import { FileUploader, ParsedResponseHeaders, FileItem } from 'ng2-file-upload';
import { EventsService } from 'src/app/services/events.service';
import { Subscription } from 'rxjs';
import { AppEvent, AppEventTypes } from 'src/app/shared/app-event.model';
import { UploadedFile } from 'src/app/shared/uploaded-file.model';

@Component({
  selector: 'app-fileuploader',
  templateUrl: './fileuploader.component.html',
  styleUrls: ['./fileuploader.component.scss']
})
export class FileuploaderComponent implements OnInit {
  @Input('upload-enabled') uploadEnabled : boolean;

  public uploader: FileUploader = new FileUploader({ url: `${environment.backend.apiUrl}/api/v1/file` });
  subscriptionAppEvent: Subscription;

  constructor( private toastr: ToastrService, private eventsService : EventsService) { }

  ngOnInit(): void {
    this.uploader.onErrorItem = ((item: FileItem, response: string, status: number, headers: ParsedResponseHeaders): any => {
      this.toastr.error(response, 'ERROR!')
    });

    this.uploader.onCompleteItem = ((item: FileItem, response: string, status: number, headers: ParsedResponseHeaders): any =>{
      var obj = JSON.parse(response);

      if(obj){
      
        this.eventsService.addEvent(new AppEvent(AppEventTypes.FileUploadEnd,  <UploadedFile>obj));
      }
      this.eventsService.addEvent(new AppEvent(AppEventTypes.FileUploadEnd,null));
    });

    this.uploader.onBeforeUploadItem = ((fileItem: FileItem): any => {
      this.eventsService.addEvent(new AppEvent(AppEventTypes.FileUploadStart,null));
    });

  }

}
