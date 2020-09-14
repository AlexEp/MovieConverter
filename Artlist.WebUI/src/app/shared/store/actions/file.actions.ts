import { Action } from '@ngrx/store';
import { UploadedFile } from '../../file.model';

export enum UploadedFileActionTypes {
      UploadedFileRequested = '[View File Page] UploadedFile Requested',
      AllUploadedFilesRequested = '[View File Page] All Uploaded Files Requested',
       AllUploadedFilesLoaded = '[API] All UploadedFiles Loaded',
    // AllCoursesRequested = '[Courses Home Page] All Courses Requested',
    // AllCoursesLoaded = '[Courses API] All Courses Loaded',
    // CourseSaved = '[Edit Course Dialog] Course Saved',
    // LessonsPageRequested = '[Course Landing Page] Lessons Page Requested',
    // LessonsPageLoaded = '[Courses API] Lessons Page Loaded',
    // LessonsPageCancelled = '[Courses API] Lessons Page Cancelled'
  }


  export class UploadedFileRequestedAction implements Action {
    readonly type = UploadedFileActionTypes.UploadedFileRequested;
  
    constructor(public payload: { courseId: number }) {}
  }

  export class AllUploadedFilesRequestedAction implements Action {
    readonly type = UploadedFileActionTypes.UploadedFileRequested;
    
  }

  export class AllUploadedFilesLoadedAction implements Action {
    readonly type = UploadedFileActionTypes.UploadedFileRequested;

    constructor(public payload: { files: UploadedFile[] }) {}
    
  }


  
  export type UploadedFileActions =
  UploadedFileRequestedAction 
  | AllUploadedFilesRequestedAction 
 // | AllUploadedFilesLoaded ;