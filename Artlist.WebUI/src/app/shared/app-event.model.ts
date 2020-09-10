export class AppEvent {
    type : AppEventTypes ;
    data : any | null;

    constructor(type :AppEventTypes,data : any | null){
          this.type = type;
          this.data = data;
    }
}

export enum AppEventTypes
{
    Void,
    FileUploadStart,
    FileUploadEnd,
}
