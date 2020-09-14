import { createSelector} from '@ngrx/store';
import { AppState } from '../AppState';


export const selectorUser = (state : AppState) => state;

export const usersNames = createSelector(
   selectorUser,
   users =>  users.users.users.map( u => u.name)
);

export const usersAges = createSelector(
    selectorUser,
    users =>  users.users.users.map( u => u.age)
 );
 