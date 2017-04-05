import {Injectable, Inject} from '@angular/core';
import {ApiService} from '../api/api.service';
import {Router} from '@angular/router';
import {CacheService} from '../cache/cache.service';

@Injectable()
export class AuthService {
    private supportedExternalProviders: Array<string> = ['facebook', 'google'];

    constructor(@Inject(ApiService) private apiService: ApiService,
                @Inject(Router) private router: Router,
                private cacheService: CacheService) {
    }

    getAuth = function (): string {
        return this.cacheService.get('auth');
    };

    signIn = function (username: string, password: string): Promise<any> {
        return this.apiService.getAuth(username, password);
    };

    externalSignIn = function (accessToken: string, provider: string): Promise<any> {
        if(this.supportedExternalProviders.indexOf(provider.toLowerCase()) === -1){
            throw new Error(`${provider} external auth provider is no supported`);
        }
        return this.apiService.getExternalAuth(accessToken, provider.toLowerCase());
    };

    isAuthenticated = function (): boolean {
        return this.getAuth() !== null && this.getAuth() !== undefined;
    };

    signOut = function (): void {
        localStorage.clear();
        this.router.navigate('/sign-in');
    };
}