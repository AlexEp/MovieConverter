import { ProcessStatusType, ProcessRequestType } from './constans';

export interface ProccesEvent {
    type: ProcessStatusType;
    status: ProcessRequestType;
    fileId: string;
    percent: number;
    massege?: null;
    data: ProcessThumbnailsResponse | ProcessConvertedFileResponse;
  }

  export interface ProcessConvertedFileResponse {
    uploadedFileId: string;
    request: Request;
    status: number;
    uploadedFile: UploadedFile;
    convertedFile: ConvertedFile;
    errorMassege?: null;
    errorCode: number;
    id: string;
    processType: number;
    callBackURL?: null;
  }
  
  export interface ProcessThumbnailsResponse {
    uploadedFileId: string;
    request: Request;
    status: number;
    uploadedFile: UploadedFile;
    thumbnail: Thumbnail;
    errorMassege?: null;
    errorCode: number;
    id: string;
    processType: number;
    callBackURL?: null;
  }
  export interface Request {
    uploadFileId: string;
    miliseconds: number;
    id: string;
    processType: number;
    callBackURL: string;
  }
  export interface UploadedFile {
    id: string;
    filename: string;
    hashed?: null;
    created: string;
    convertedFiles?: ConvertedFile[] | null;
    thumbnails?: Thumbnail[] | null;
  }
  export interface Thumbnail {
    id: string;
    filename: string;
    sourceFileId: string;
    frameNum: number;
    created: string;
    sourceFiles?: null;
  }
  
  export interface ConvertedFile {
    id: string;
    sourceFilesId: string;
    created: string;
    codec: string;
    sourceFiles?: null;
  }