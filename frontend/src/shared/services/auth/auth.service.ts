import {Injectable, Inject} from '@angular/core';
import {ApiService} from '../api/api.service';
import {CanActivate, Router, ActivatedRouteSnapshot, RouterStateSnapshot} from '@angular/router';
import {Observable} from 'rxjs/Observable';
import { LocalStorageService } from 'angular-2-local-storage';

@Injectable()
export class AuthService {
    constructor(@Inject(ApiService) private apiService: ApiService, private localStorageService: LocalStorageService){
    }

    getAuth = function (): string {
      return this.localStorageService.get('auth');
    };

    postUserCredentials = function(username: string, password: string) : Promise<any> {
        var requestBody = `grant_type=password&username=${encodeURIComponent(username)}&password=${encodeURIComponent(password)}`;
        var requestHeaders = {'Content-Type': 'application/x-www-form-urlencoded'};
        return this.apiService.post('token', requestBody, requestHeaders);
    };

    isAuthenticated = function (): boolean {
      return this.getAuth() as boolean;
    };
}

export class CanActivateAuth implements CanActivate {
    constructor(@Inject(Router) private router: Router, @Inject(AuthService) private authService: AuthService){

    }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean>|Promise<boolean>|boolean {
        if(this.authService.getAuth()){
            return true;
        }

        this.router.navigate(['/sign-in']);
        return false;
    }
}