//import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';

import { HttpClientModule } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

//Plugins
import { ToastrModule } from 'ngx-toastr';
import { FileUploadModule } from 'ng2-file-upload'
import {NgbModalModule} from '@ng-bootstrap/ng-bootstrap';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';

//App Service/Component
import { ServerApiService } from './services/server-api.service';
import { NavBarComponent } from './components/nav-bar/nav-bar.component';
import { HomeComponent } from './components/home/home.component';
import { SanitizerPipe } from './pipes/sanitizer.pipe';
import { FileuploaderComponent } from './components/fileuploader/fileuploader.component';
import { FileStatusComponent } from './components/file-status/file-status.component';
import { UploadFilePageComponent } from './components/upload-file-page/upload-file-page.component';
import { UploadedFileListComponent } from './components/uploaded-file-list/uploaded-file-list.component';
import { UsersComponent } from './components/users/users.component';
import { StoreModule } from '@ngrx/store';
import * as fromApp from './shared/store/AppState';
import { environment } from 'src/environments/environment';
import { EffectsModule } from '@ngrx/effects';
import { BrowserModule } from '@angular/platform-browser';
import { ServiceWorkerModule } from '@angular/service-worker';




@NgModule({
  declarations: [
    AppComponent,
    NavBarComponent,
    HomeComponent,
    SanitizerPipe,
    FileuploaderComponent,
    FileStatusComponent,
    UploadFilePageComponent,
    UploadedFileListComponent,
    UsersComponent,
  ],
  imports: [
   // BrowserAnimationsModule,
    BrowserModule.withServerTransition({ appId: 'serverApp' }),
    AppRoutingModule,
    HttpClientModule,

    ToastrModule.forRoot(), 
    FileUploadModule,
    NgbModalModule,
    NgxDatatableModule,

    StoreModule.forRoot(fromApp.reducers),
    !environment.production ? StoreDevtoolsModule.instrument() : [],
    ServiceWorkerModule.register('ngsw-worker.js', { enabled: environment.production }),
    //EffectsModule.forRoot([UserEffects]),
  ],
  providers: [ServerApiService],
  bootstrap: [AppComponent]
})
export class AppModule { }
