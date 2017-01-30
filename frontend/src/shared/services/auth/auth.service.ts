import { Injectable, Inject } from '@angular/core';
import { ApiService } from '../api/api.service';
import { Router } from '@angular/router';
import { LocalStorageService } from 'angular-2-local-storage';

@Injectable()
export class AuthService {
    constructor(@Inject(ApiService) private apiService: ApiService,
                @Inject(Router) private router: Router,
                private localStorageService: LocalStorageService){
    }

    getAuth = function (): string {
      return this.localStorageService.get('auth');
    };

    signIn = function(username: string, password: string) : Promise<any> {
        return this.apiService.getAuth(username, password);
    };

    isAuthenticated = function (): boolean {
      return this.getAuth() !== null && this.getAuth() !== undefined;
    };

    signOut = function (): void {
        this.localStorageService.clearAll();
        this.router.navigate('/sign-in');
    };
}