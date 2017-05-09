import {Injectable} from '@angular/core';
import {CacheService} from '../cache/cache.service';

@Injectable()
export class AuthProviderService {
    constructor(protected cacheService: CacheService) {
    }

    setAuth(data: any): any {
        this.cacheService.set('auth', data);
        return data;
    }
    
    clearAuth(): void {
        this.cacheService.remove('auth');
    }

    getAuth():any {
        return this.cacheService.get('auth');
    }

    hasAuth():boolean {
        return this.getAuth() !== null;
    }

    getAuthToken():string {
        let auth = this.getAuth();
        return auth ? auth.access_token : null;
    }

    getRefreshToken():string {
        let auth = this.getAuth();
        return auth ? auth.refresh_token : null;
    }

    setUser(data: any): any {
        this.cacheService.set('user', data, data.expires_in);
        return data;
    }

    clearUser(): void {
        this.cacheService.remove('user');
    }

    getUser():any {
        return this.cacheService.get('user');
    }

    hasUser():boolean {
        return this.getUser() !== null;
    }

    isAuthenticated():boolean {
        return this.hasAuth() && this.hasUser();
    }
}