import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';

import { HttpClientModule } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

//Plugins
import { ToastrModule } from 'ngx-toastr';
import { FileUploadModule } from 'ng2-file-upload'

import { ServerApiService } from './services/server-api.service';
import { NavBarComponent } from './components/nav-bar/nav-bar.component';
import { HomeComponent } from './components/home/home.component';
import { SanitizerPipe } from './pipes/sanitizer.pipe';
import { FileuploaderComponent } from './components/fileuploader/fileuploader.component';
import { FileStatusComponent } from './components/file-status/file-status.component';

@NgModule({
  declarations: [
    AppComponent,
    NavBarComponent,
    HomeComponent,
    SanitizerPipe,
    FileuploaderComponent,
    FileStatusComponent
  ],
  imports: [
    BrowserAnimationsModule,
    AppRoutingModule,
    HttpClientModule,

    ToastrModule.forRoot(), 
    FileUploadModule
  ],
  providers: [ServerApiService],
  bootstrap: [AppComponent]
})
export class AppModule { }
