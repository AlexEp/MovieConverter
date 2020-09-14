import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { UploadedFileListComponent } from './components/uploaded-file-list/uploaded-file-list.component';
import { UsersComponent } from './components/users/users.component';


const routesConfigs : Routes = [
   { path: '', pathMatch: 'full', redirectTo: 'home' },
  //{ path: '', pathMatch: 'full', redirectTo: 'main' },
   { path: 'home', component: HomeComponent },
   { path: 'reports/uploadedFiles', component: UploadedFileListComponent },
   { path: 'users', component: UsersComponent },
  // { path: 'reports', component: ReportComponent },
  // { path: 'management', component: ManagementComponent },
  // { path: '**', component: PageNotFoundComponent }
];


@NgModule({
  imports: [RouterModule.forRoot(routesConfigs, {
    initialNavigation: 'enabled'
})],
  exports: [RouterModule]
})
export class AppRoutingModule { }
