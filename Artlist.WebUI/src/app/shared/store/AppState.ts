
import { ActionReducerMap } from '@ngrx/store';
import * as fromUser from './reducers/user.reducer';
import * as fromFile from './reducers/file.reducer';


export interface AppState  {
    users : fromUser.IState,
    files : fromFile.UploadedFileState
}

export const reducers : ActionReducerMap<AppState> = {
    users: fromUser.UserReducer,
    files : fromFile.FileReducer
}