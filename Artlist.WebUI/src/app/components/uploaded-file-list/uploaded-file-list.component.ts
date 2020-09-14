import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-uploaded-file-list',
  templateUrl: './uploaded-file-list.component.html',
  styleUrls: ['./uploaded-file-list.component.scss']
})
export class UploadedFileListComponent implements OnInit {

  columns = [{ prop: 'name' }, { name: 'Gender' }, { name: 'Company' }];
  rows = [
    { name: 'Austin', gender: 'Male', company: 'Swimlane' },
    { name: 'Dany', gender: 'Male', company: 'KFC' },
    { name: 'Molly', gender: 'Female', company: 'Burger King' }
  ];

  constructor() { }

  ngOnInit(): void {
  }

}
