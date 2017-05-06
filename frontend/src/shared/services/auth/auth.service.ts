import {Injectable} from '@angular/core';
import {ApiService} from '../api/api.service';
import {AuthProviderService} from './auth-provider.service';

@Injectable()
export class AuthService {
    private supportedExternalProviders: Array<string> = ['facebook', 'google'];

    constructor(private apiService: ApiService,
                private authProvider: AuthProviderService) {
    }

    loadUser = (refresh: boolean = false) => {
        return this.apiService.get('account', null, null, refresh)
            .then((response: any) => this.authProvider.setUser(response.data));
    };

    signIn = function (username: string, password: string): Promise<any> {
        var requestBody = `grant_type=password&username=${encodeURIComponent(username)}&password=${encodeURIComponent(password)}`;
        return this.postToken(requestBody)
            .then((auth: any) => this.authProvider.setAuth(auth))
            .then((auth: any) => this.loadUser(true));
    };

    externalSignIn = function (accessToken: string, provider: string): Promise<any> {
        if(this.supportedExternalProviders.indexOf(provider.toLowerCase()) === -1){
            throw new Error(`${provider} external auth provider is no supported`);
        }
        return this.postExternalAuth(accessToken, provider.toLowerCase())
            .then((auth: any) => this.authProvider.setAuth(auth))
            .then((auth: any) => this.loadUser(true));
    };

    signOut = function (): Promise<any> {
        return new Promise((resolve, reject) => {
            if (this.authProvider.isAuthenticated()) {
                let user:any = this.authProvider.getUser();
                this.authProvider.setAuth(null);
                this.authProvider.setUser(null);
                resolve(user);
            } else {
                reject();
            }
        });
    };

    private postToken = (requestBody: string) => {
        var requestHeaders = {'Content-Type': 'application/x-www-form-urlencoded'};
        return this.apiService.post('token', requestBody, requestHeaders);
    };

    private postExternalAuth = (accessToken: string, provider: string) => {
        var requestBody = `grant_type=urn:ietf:params:oauth:grant-type:${provider}_access_token&assertion=${encodeURIComponent(accessToken)}`;
        return this.postToken(requestBody);
    };

}