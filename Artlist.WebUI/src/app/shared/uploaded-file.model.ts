export class UploadedFile {
    id : string;
    filename : string;
    created? : Date;
    convertedFiles: any[];
    thumbnails: [];
    hashed: null;

    constructor(){
          this.id = "";
          this.filename = "";
          this.created = null;
    }
}

