import {Injectable} from '@angular/core';
import {Http, Headers, Response, RequestOptions} from '@angular/http';
import {AppSettings} from '../../../app/app.settings';
import {Observable} from 'rxjs/Observable';
import {LocalStorageService} from 'angular-2-local-storage';

@Injectable()
export class ApiService{
    constructor(private http: Http, private localStorageService: LocalStorageService) { }

    get = (url: string, params: any = null, headers: any = null) => {
        var requestHeaders = this.initializeHeaders(headers);
        var requestOptions = new RequestOptions({ headers: requestHeaders, search: params });

        return this.http.get(this.getAbsoluteUrl(url), requestOptions)
                .map(this.extractData)
                .catch(this.handleError).toPromise();
    };

    getAuth = (username: string, password: string) => {
        var requestBody = `grant_type=password&username=${encodeURIComponent(username)}&password=${encodeURIComponent(password)}`;
        var requestHeaders = {'Content-Type': 'application/x-www-form-urlencoded'};
        return this.post('token', requestBody, requestHeaders);
    };

    post = (url: string, params: any, headers: any = null) =>{
        var requestHeaders = this.initializeHeaders(headers);
        var requestOptions = new RequestOptions({ headers: requestHeaders });

        return this.http.post(this.getAbsoluteUrl(url), this.initializeBody(params), requestOptions)
                .map(this.extractData)
                .catch(this.handleError).toPromise();
    };

    private extractData(response: Response) {
        let body = response.json();
        return body || { };
    };

    private handleError (response: Response) {
        if(response.status === 401){
            // this.localStorageService.clearAll();
            // refresh token
        }
        return Observable.throw(new Error(response.toString()));
    };

    private getAbsoluteUrl(url: string): string {
        return AppSettings.API_ENDPOINT + url;
    };

    private initializeBody = (body?:any) => {
        return body || {};
    };

    private initializeHeaders = (customHeaders: Headers): Headers => {
        var headers = new Headers(customHeaders);
        if(!headers.has('Content-Type')){
            headers.append('Content-Type', 'application/json' );
        }

        var authToken = this.localStorageService.get('auth');
        if(authToken){
            headers.append('Authorization', `Bearer ${authToken}`);
        }

        return headers;
    };
}