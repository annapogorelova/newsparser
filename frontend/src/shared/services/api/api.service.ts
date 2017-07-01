import {Injectable} from '@angular/core';
import {Http, Headers, RequestOptions, URLSearchParams} from '@angular/http';

/**
 * Class is a wrapper for HTTP GET, PUT, POST, DELETE methods
 */
@Injectable()
export class ApiService {
    constructor(protected http:Http,
                protected apiUrl:string,
                protected onResponseSuccess:any,
                protected onResponseError:any,
                protected provideDefaultHeaders:any = null) {
    }

    /**
     * HTTP GET
     * @param url - request url
     * @param params - request params
     * @param headers - custom request headers
     * @returns {Promise<any>}
     */
    get = (url:string, params:any = null, headers:any = null, refresh:boolean = false) => {
        var absoluteUrl = this.getAbsoluteUrl(url);
        if (params) {
            params.refresh = refresh;
        }

        return this.request(absoluteUrl, 'GET', null, params, headers);
    };

    /**
     * HTTP POST method
     * @param url - request url
     * @param params - request body
     * @param headers - custom request headers
     * @returns {Promise<any>}
     */
    post = (url:string, params:any, headers:any = null) => {
        return this.request(this.getAbsoluteUrl(url), 'POST', params, null, headers);
    };

    /**
     * HTTP PUT method
     * @param url - request url
     * @param params - request body
     * @param headers - cusrom request headers
     * @returns {Promise<any>}
     */
    put = (url:string, params:any, headers:any = null) => {
        return this.request(this.getAbsoluteUrl(url), 'PUT', params, null, headers);
    };

    /**
     * HTTP DELETE method
     * @param url - request url
     * @param id - entity to delete id
     * @param headers - custom request headers
     * @returns {Promise<any>}
     */
    delete = (url:string, headers:any = null) => {
        var requestUrl = this.getAbsoluteUrl(url);

        return this.request(requestUrl, 'DELETE', null, null, headers);
    };

    /**
     * General HTTP request
     * @param url
     * @param method
     * @param body
     * @param params
     * @param headers
     * @returns {Promise<any|T>|Promise<any>}
     */
    private request = (url:string,
                       method:string,
                       body:any = null,
                       params:any = null,
                       headers:any = null):Promise<any> => {
        var requestOptions = new RequestOptions({
            method: method,
            headers: this.initializeHeaders(headers),
            body: this.initializeBody(body),
            search: this.initializeParams(params, params ? params.refresh : false)
        });
        return this.http.request(url, requestOptions)
            .toPromise()
            .then(this.onResponseSuccess)
            .catch(this.onResponseError)
            .catch((error:any) => this.afterError(error, url, method, body, params, headers));
    };

    /**
     * Retries the request if required
     * @param response
     * @param url
     * @param method
     * @param body
     * @param params
     * @param headers
     * @returns {any}
     */
    private afterError = (error:any,
                          url:string,
                          method:string,
                          body:any = null,
                          params:any = null,
                          headers:any = null):Promise<any> => {
        if (error.retry) {
            return Promise.resolve(this.request(url, method, body, params, headers));
        }

        return Promise.reject(error);
    };

    /**
     * Get absolute API endpoint url
     * @param route - API resource route
     * @returns {string} - absolute url
     */
    private getAbsoluteUrl(route:string):string {
        return this.apiUrl + route;
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
    private initializeHeaders(headers?:any):Headers {
        var initializedHeaders = this.getDefaultHeaders();
        headers = headers || {};
        for (let name in headers) {
            initializedHeaders.set(name, headers[name]);
        }
        return initializedHeaders;
    };

    protected getDefaultHeaders():Headers {
        const defaultHeaders = new Headers;
        const rawDefaultHeaders = this.getRawDefaultHeaders();
        for (let name in rawDefaultHeaders) {
            if (rawDefaultHeaders.hasOwnProperty(name)) {
                defaultHeaders.append(name, rawDefaultHeaders[name]);
            }
        }
        return defaultHeaders;
    }

    /**
     * Get default request headers
     * @returns {{}}
     */
    protected getRawDefaultHeaders():any {
        return typeof (this.provideDefaultHeaders) === 'function'
            ? this.provideDefaultHeaders()
            : {};
    }

    /**
     * Initializes GET search params
     * @param params
     * @returns {URLSearchParams}
     */
    private initializeParams = (params:any, refresh:boolean = false):URLSearchParams => {
        var searchParams = new URLSearchParams();

        for (var key in params) {
            searchParams.set(key, params[key]);
        }

        if (refresh) {
            searchParams.set('autotimestamp', Date.now().toString());
        }

        return searchParams;
    };
}