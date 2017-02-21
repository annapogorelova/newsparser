import {Injectable} from '@angular/core';
import {Http, Headers, Response, RequestOptions, URLSearchParams} from '@angular/http';
import {AppSettings} from '../../../app/app.settings';
import {LocalStorageService} from 'angular-2-local-storage';
import {ApiErrorHandler} from './api-error-handler';

/**
 * Class is a wrapper for HTTP GET, PUT, POST, DELETE methods
 */
@Injectable()
export class ApiService{
    constructor(private http: Http, private localStorageService: LocalStorageService, private errorHandler: ApiErrorHandler) { }

    /**
     * HTTP GET
     * @param url - request url
     * @param params - request params
     * @param headers - custom request headers
     * @returns {Promise<any>}
     */
    get = (url: string, params: any = null, headers: any = null) => {
        var requestHeaders = this.initializeHeaders(headers);
        var requestOptions = new RequestOptions({ headers: requestHeaders, search: this.initializeParams(params) });

        return this.http.get(this.getAbsoluteUrl(url), requestOptions)
                .map(this.extractData)
                .catch(this.errorHandler.handleResponse).toPromise();
    };

    /**
     * HTTP GET customized to get auth token
     * @param username
     * @param password
     * @returns {Promise<any>}
     */
    getAuth = (username: string, password: string) => {
        var requestBody = `grant_type=password&username=${encodeURIComponent(username)}&password=${encodeURIComponent(password)}`;
        var requestHeaders = {'Content-Type': 'application/x-www-form-urlencoded'};
        return this.post('token', requestBody, requestHeaders);
    };

    /**
     * HTTP POST method
     * @param url - request url
     * @param params - request body
     * @param headers - cusrom request headers
     * @returns {Promise<any>}
     */
    post = (url: string, params: any, headers: any = null) =>{
        var requestHeaders = this.initializeHeaders(headers);
        var requestOptions = new RequestOptions({ headers: requestHeaders });

        return this.http.post(this.getAbsoluteUrl(url), this.initializeBody(params), requestOptions)
                .map(this.extractData)
                .catch(this.errorHandler.handleResponse)
                .toPromise();
    };

    /**
     * HTTP DELETE method
     * @param url - request url
     * @param id - entity to delete id
     * @param headers - custom request headers
     * @returns {Promise<any>}
     */
    delete = (url: string, id: number, headers: any = null) => {
        var requestHeaders = this.initializeHeaders(headers);
        var requestOptions = new RequestOptions({ headers: requestHeaders });
        var requestUrl = this.getAbsoluteUrl(url) + '?id=' + id;

        return this.http.delete(requestUrl, requestOptions)
            .map(this.extractData)
            .catch(this.errorHandler.handleResponse)
            .toPromise();
    };

    /**
     * Extracts response data
     * @param response - Response
     * @returns {any|{}}
     */
    private extractData(response: Response) {
        let body = response.json();
        return body || { };
    };

    /**
     * Get absolute API endpoint url
     * @param route - API resource route
     * @returns {string} - absolute url
     */
    private getAbsoluteUrl(route: string): string {
        return AppSettings.API_ENDPOINT + route;
    };

    /**
     * Initializes a request body
     * @param body
     * @returns {any|{}}
     */
    private initializeBody = (body?:any) => {
        return body || {};
    };

    /**
     * Initializes request headers
     * @param customHeaders
     * @returns {Headers}
     */
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

    /**
     * Initializes GET search params
     * @param params
     * @returns {URLSearchParams}
     */
    private initializeParams = (params: any): URLSearchParams => {
        var searchParams = new URLSearchParams();

        for(var key in params){
            searchParams.set(key, params[key]);
        }

        return searchParams;
    };
}