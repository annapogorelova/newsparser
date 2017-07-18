import {CanActivate, Router, ActivatedRouteSnapshot, RouterStateSnapshot} from '@angular/router';
import {Observable} from 'rxjs';
import {AuthProviderService} from './auth-provider.service';
import {Inject} from '@angular/core';

export class CanActivatePrivate implements CanActivate {
    constructor(@Inject(Router) private router:Router,
                @Inject(AuthProviderService) private authProvider:AuthProviderService) {
    }

    canActivate(route:ActivatedRouteSnapshot, state:RouterStateSnapshot):Observable<boolean>|Promise<boolean>|boolean {
        if (this.authProvider.isAuthenticated()) {
            return true;
        }

        this.router.navigate(['/sign-in']);
        return false;
    }
}