import {Injectable, Inject} from '@angular/core';
import {ApiService} from '../api/api.service';
import {Router} from '@angular/router';
import {CacheService} from '../cache/cache.service';

@Injectable()
export class AuthService {
    private supportedExternalProviders: Array<string> = ['facebook', 'google'];
    private user: any;

    constructor(@Inject(ApiService) private apiService: ApiService,
                @Inject(Router) private router: Router,
                private cacheService: CacheService) {
    }

    getAuth = function (): string {
        return this.cacheService.get('auth');
    };

    loadUser = (refresh: boolean = false) => {
        return this.apiService.get('account', null, null, refresh)
            .then((response: any) => this.setUser(response.data));
    };

    getUser = () => {
        return this.user;
    };

    private setUser = (data: any) => {
        this.user = data;
        return Promise.resolve(data);
    };

    signIn = function (username: string, password: string): Promise<any> {
        return this.apiService.getAuth(username, password)
            .then((response: any) => this.handleAuth(response));
    };

    private handleAuth = (response: any) => {
        this.cacheService.set('auth', response.access_token);
        this.loadUser(true);
        return Promise.resolve(response);
    };

    externalSignIn = function (accessToken: string, provider: string): Promise<any> {
        if(this.supportedExternalProviders.indexOf(provider.toLowerCase()) === -1){
            throw new Error(`${provider} external auth provider is no supported`);
        }
        return this.apiService.getExternalAuth(accessToken, provider.toLowerCase())
            .then((response: any) => this.handleAuth(response));
    };

    isAuthenticated = function (): boolean {
        return this.getAuth() !== null && this.getAuth() !== undefined;
    };

    signOut = function (): void {
        localStorage.clear();
        this.router.navigate('/sign-in');
    };
}