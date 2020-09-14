import { Action } from '@ngrx/store'
import { User } from '../../user.model'



export const ADD_USER = "[User] ADD_USER"
export const ADD_USERS = "[User] ADD_USERS"
export const UPDATE_USER = "[User] UPDATE_PERSON"
export const DELETE_USER = "[User] DELETE_USER"


export class AddUserAction implements Action {
    readonly type: string = ADD_USER;

    constructor(public payload  : User) {}
}

export class UpdateUserAction implements Action {
    readonly type: string = ADD_USERS;

    constructor(public payload  :{ user: User, userId : number}) {  }
}

export type UserAction = UpdateUserAction | AddUserAction; 

