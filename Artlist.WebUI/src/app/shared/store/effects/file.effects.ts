import { Injectable } from '@angular/core';
import { Actions, createEffect, Effect, ofType } from '@ngrx/effects';
import { tap, mergeMap, map } from 'rxjs/operators';
import { Router } from '@angular/router';
import { defer } from 'rxjs';
import * as fromFileActions from '../actions/file.actions';
import { ServerApiService } from 'src/app/services/server-api.service';


@Injectable()
export class UploadedFileEffects {


//   @Effect({ dispatch: false })
//   loadFiles$ = this.actions$
//     .pipe(
//       //ofType<Action>(type)
//       ofType<fromFileActions.AllUploadedFilesRequestedAction>(fromFileActions.UploadedFileActionTypes.AllUploadedFilesRequested),
//       mergeMap(action => this.apiService.getFile(10)),
//       map(files => new fromFileActions.FileLoaded({files}))
//   );

  @Effect({ dispatch: false })
  loadAllFiles$ = this.actions$
    .pipe(
      ofType<fromFileActions.UploadedFileRequestedAction>(fromFileActions.UploadedFileActionTypes.UploadedFileRequested),
      mergeMap(action => this.apiService.getFile(10)),
      map(files => new fromFileActions.AllUploadedFilesLoadedAction({files}))
  );

 // @Effect()
  //init$ = defer(() => new LoadOrdersRequested());

  constructor(private actions$: Actions, private router: Router, private apiService : ServerApiService) { }

 }
