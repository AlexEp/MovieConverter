import { Injectable } from '@angular/core';
import { HttpClient, HttpEvent, HttpErrorResponse, HttpEventType, HttpHeaders } from '@angular/common/http';
import { environment } from '../../environments/environment';

/* Rxjs */
import { Observable, of, throwError } from 'rxjs';
import { map, retry, catchError } from 'rxjs/operators';
import { UploadedFile } from '../shared/file.model';


@Injectable({
  providedIn: 'root'
})
export class ServerApiService {

  SERVER_API_URL: string = `${environment.backend.apiUrl}/api/v1`;

  constructor(private http: HttpClient) { }


  public uploadFile(data: FormData) {
    const uploadURL = `${this.SERVER_API_URL}/file`;

    return this.http.post<{ message: string , progress: number, body: {} }>(uploadURL, data, {
      reportProgress: true,
      observe: 'events'
    }).pipe(map((event) => {

      switch (event.type) {

        case HttpEventType.UploadProgress:
          const progress = Math.round(100 * event.loaded / event.total);
          return { message: 'progress', progress: progress, body : null };

        case HttpEventType.Response:
          return  { message: 'done', progress: 100, body :  event.body };
        default:
          return  { message: 'error', progress: 0, body : null };
      }
    }),
    catchError(err => {
      //... Some ganeric code to deal with error
      return throwError(err);
    }),
    );
  }


  getFile(amout : number) : Observable<UploadedFile[]> {
    const url = `${this.SERVER_API_URL}/file/${amout}`;
 
    return this.http.get<UploadedFile[]>(url).pipe(
      map(res => res)
    )
  }
  
  handleError(handleError: any): import("rxjs").OperatorFunction<string, any> {
    throw new Error("Method not implemented.");
  }
  
}






//  this.http.post("URL",fileData, { reportProgress: true, observe: 'events' })
//  .subscribe( event => {
//    if( event === HttpEventType.UploadProgress){
//     console.log(event.loaded / event.total * 100)
//    } 
//    else if ( event === HttpEventType.Sent){
//     console.log(event)
//    }
//  });