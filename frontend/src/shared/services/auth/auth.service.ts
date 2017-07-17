import {Injectable} from '@angular/core';
import {AuthProviderService} from './auth-provider.service';
import {Http, Headers, RequestOptions, Response, URLSearchParams} from '@angular/http';
import {AppSettings} from '../../../app/app.settings';
import {CacheService} from '../cache/cache.service';

@Injectable()
export class AuthService {
    private supportedExternalProviders:Array<string> = ['facebook', 'google'];

    constructor(private http:Http,
                private authProvider:AuthProviderService,
                private cacheService:CacheService) {
    }

    loadUser(refresh:boolean = false):Promise<any> {
        if (!this.authProvider.hasAuth()) {
            return Promise.reject('User is not authenticated');
        }

        var headers = new Headers;
        headers.append('Content-Type', 'application/json');
        headers.append('Authorization', `Bearer ${this.authProvider.getAuthToken()}`);

        var requestOptions = new RequestOptions({headers: headers});

        if (refresh) {
            requestOptions.params = new URLSearchParams;
            requestOptions.params.set('autotimestamp', Date.now().toString());
        }

        return this.http.get(this.getAbsoluteUrl('account'), requestOptions)
            .timeout(AppSettings.TIMEOUT['GET'])
            .toPromise()
            .then((response:any) => this.extractData(response))
            .then((response:any) => this.authProvider.setUser(response.data))
            .catch((error:any) => this.onAccountRequestFailed(error))
            .catch((error:any) => {
                if (error.retry) {
                    return this.loadUser(refresh);
                }

                return Promise.reject(error);
            });
    };

    signIn(username:string, password:string):Promise<any> {
        return new Promise((resolve, reject) => {
            var requestBody = `grant_type=password&username=${encodeURIComponent(username)}&password=${encodeURIComponent(password)}&scope=offline_access`;
            return this.postToken(requestBody)
                .then((auth:any) => this.authProvider.setAuth(auth))
                .then((auth:any) => resolve(this.loadUser(true)))
                .catch((error:any) => reject(error));
        });
    };

    externalSignIn(accessToken:string, provider:string):Promise<any> {
        if (this.supportedExternalProviders.indexOf(provider.toLowerCase()) === -1) {
            throw new Error(`${provider} external auth provider is no supported`);
        }
        return this.postExternalAuth(accessToken, provider.toLowerCase())
            .then((auth:any) => this.authProvider.setAuth(auth))
            .then((auth:any) => this.loadUser(true));
    };

    signOut():Promise<any> {
        return new Promise((resolve, reject) => {
            if (this.authProvider.isAuthenticated()) {
                let user:any = this.authProvider.getUser();
                this.cacheService.clear();
                resolve(user);
            } else {
                reject();
            }
        });
    };

    refreshAuth():Promise<any> {
        return new Promise((resolve, reject) => {
            var refreshToken = this.authProvider.getRefreshToken();
            if (!refreshToken) {
                reject();
            }
            var requestBody = `grant_type=refresh_token&refresh_token=${refreshToken}&scope=offline_access`;
            return this.postToken(requestBody)
                .then((auth:any) => this.authProvider.setAuth(auth))
                .then((response:any) => resolve(response))
                .catch((error:any) => reject(error));
        });
    };

    private postToken(requestBody:string) {
        return new Promise((resolve, reject) => {
            var requestHeaders = new Headers({'Content-Type': 'application/x-www-form-urlencoded'});
            var requestOptions = new RequestOptions({headers: requestHeaders});
            return this.http.post(this.getAbsoluteUrl('token'), requestBody, requestOptions)
                .timeout(AppSettings.TIMEOUT['POST'])
                .toPromise()
                .then((response:any) => resolve(this.extractData(response)))
                .catch((error:any) => reject(error));
        });
    };

    private postExternalAuth(accessToken:string, provider:string) {
        var requestBody = `grant_type=urn:ietf:params:oauth:grant-type:${provider}_access_token&assertion=${encodeURIComponent(accessToken)}`;
        return this.postToken(requestBody);
    };

    private onAccountRequestFailed(error:any) {
        if (error.status === 401) {
            return this.handleUnauthorizedError();
        }
        return Promise.reject(error);
    };

    private handleUnauthorizedError() {
        return new Promise((resolve, reject) => {
            if (this.authProvider.isAuthenticated()) {
                return this.refreshAuth()
                    .then(() => {
                        var response = {retry: true};
                        reject(response);
                    })
                    .catch((error:any) => reject(error));
            }

            reject();
        });
    };

    private getAbsoluteUrl(route:string):string {
        return AppSettings.API_ENDPOINT + route;
    };

    private extractData(response:Response) {
        return response.json() || {};
    };
}