import { Action } from '@ngrx/store'
import * as UserActions from '../actions/user.actions'
import { User } from '../../user.model';


export interface IState { users : User[] };

let initialstate : IState = { users : [  {id :1,name: "Alex" ,age : 23 },  {id :2,name: "Haim" ,age : 54 }] };

 export function UserReducer(state : IState = initialstate, action : UserActions.UserAction ) : IState {
    
    switch (action.type) {
        case UserActions.ADD_USER:
            const act = (<UserActions.AddUserAction>action);
            return {
                ...state,
                users : [...state.users, act.payload]
            }
        break;
        case UserActions.UPDATE_USER:
            {   
                const act = (<UserActions.UpdateUserAction>action);
                const user = state.users.find(u => u.id == act.payload.userId)[act.payload.userId];

                const newUser = {
                    ...user,
                    ...action.payload //override
                }
                
                return {
                    ...state,
                }
            }
        break;
        default:
            return state;
            break;
     }
 }