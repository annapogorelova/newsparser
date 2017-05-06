import {Injectable} from '@angular/core';
import {CacheService} from '../cache/cache.service';

@Injectable()
export class AuthProviderService {
    constructor(protected cacheService: CacheService) {
    }

    setAuth(auth:any):any {
        this.cacheService.set('auth', auth);
        return auth;
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

    setUser(user:any):any {
        this.cacheService.set('user', user);
        return user;
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