import { Component, OnInit } from '@angular/core';
import { Store, select } from '@ngrx/store';
import * as fromAppState from '../../shared/store/AppState';

import { AddUserAction } from 'src/app/shared/store/actions/user.actions';
import { User } from 'src/app/shared/user.model';
import { Observable } from 'rxjs';
import { usersNames, usersAges } from 'src/app/shared/store/selectors/users.selector';
import { map } from 'rxjs/operators';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.scss']
})
export class UsersComponent implements OnInit {
  usersNames$ : Observable<string[]>;
  usersAges$ : Observable<number[]>;
  users$ : Observable<User[]>;

  constructor(
    private store: Store<fromAppState.AppState>
  ) {}
 
  ngOnInit(){

      this.users$ =  this.store.pipe(map(s => s.users.users));
 

    this.usersNames$ = this.store.pipe(select(usersNames));
    this.usersAges$ = this.store.pipe(select(usersAges));
  }

  addUser(){
    //this.store.dispatch(new AddUserAction(new User(3,"Yosi",45)));
  }

}
