import { EntityState, EntityAdapter, createEntityAdapter } from '@ngrx/entity';
import * as UserActions from '../actions/user.actions'
import { UploadedFile } from '../../file.model';


export interface UploadedFileState extends EntityState<UploadedFile> {
    allLoaded:boolean;
}
  
  export const adapter : EntityAdapter<UploadedFile> =
    createEntityAdapter<UploadedFile>();
  
  export const initialState: UploadedFileState = adapter.getInitialState({
    allLoaded: false
  });
  

 export function FileReducer(state : UploadedFileState = initialState, action : UserActions.UserAction ) : UploadedFileState {
    
    switch (action.type) {
        
        default:
            return state;
            break;
     }
 }