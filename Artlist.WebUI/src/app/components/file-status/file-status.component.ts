import { Component, OnInit, Input } from '@angular/core';
import { ProcessStatusType } from 'src/app/shared/constans';

@Component({
  selector: 'app-file-status',
  templateUrl: './file-status.component.html',
  styleUrls: ['./file-status.component.scss']
})
export class FileStatusComponent implements OnInit {
  @Input('file-status') fileStatus : ProcessStatusType | null;
  constructor() { }

  ngOnInit(): void {
  }

  isFailed () {
    return this.fileStatus === ProcessStatusType.Failed
  }

  isInProcess () {
    return this.fileStatus === ProcessStatusType.InProcess
  }

  isCompleted () {
    return this.fileStatus === ProcessStatusType.Completed
  }

  isStarted() {
    return this.fileStatus === ProcessStatusType.Started
  }

  isNull() {
    return this.fileStatus === null;
  }

}
