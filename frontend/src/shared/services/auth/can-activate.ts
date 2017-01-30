import {CanActivate, Router, ActivatedRouteSnapshot, RouterStateSnapshot} from '@angular/router';
import {Inject} from '@angular/core';
import {AuthService} from './auth.service';
import {Observable} from 'rxjs';

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