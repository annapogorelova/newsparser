import {Injectable, Inject} from '@angular/core';
import {ApiService} from '../api/api.service';
import {Router} from '@angular/router';
import {CacheService} from '../cache/cache.service';

@Injectable()
export class AuthService {
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

    isAuthenticated = function (): boolean {
        return this.getAuth() !== null && this.getAuth() !== undefined;
    };

    signOut = function (): void {
        localStorage.clear();
        this.router.navigate('/sign-in');
    };
}