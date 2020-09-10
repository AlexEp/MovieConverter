import { ProcessRequestType, ProcessStatusType } from './constans';

export class ProccesEvent {
    type : ProcessRequestType;
    status : ProcessStatusType
    fileId : string;
    percent? : number;
    massege : string;
    data : any

    constructor(){
          this.type = null;
          this.fileId = "";
          this.status = null;
          this.percent = null;
          this.massege = "";
          this.data = null;
    }
}