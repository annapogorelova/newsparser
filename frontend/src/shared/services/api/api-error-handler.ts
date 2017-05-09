import {Response} from '@angular/http';
import {NavigatorService} from '../navigator/navigator.service';
import {Injectable} from '@angular/core';
import {HttpError} from './http-error';
import {AuthService} from '../auth/auth.service';
import {AuthProviderService} from '../auth/auth-provider.service';
import {AuthRefreshLocker} from '../auth/auth-refresh-locker.service';

/**
 * Class contains functionality to handle http request errors
 */
@Injectable()
export class ApiErrorHandler {
    constructor(private navigator: NavigatorService,
                private authProvider: AuthProviderService,
                private authService: AuthService,
                private authRefreshLocker: AuthRefreshLocker){}

    /**
     * General method for handling http errors
     * @param response - Response object
     * @returns {any}
     */
    onRequestFailed(response: Response): Promise<any> {
        switch(response.status){
            case 401:
                return this.handleUnauthorizedError(response);
        }

        let body = response.json();
        return Promise.reject(new HttpError(body.message, response.status, body.validationErrors));
    };

    /**
     * Handles the 401 status
     */
    private handleUnauthorizedError(response: Response): Promise<any> {
        if (this.authProvider.hasAuth()) {
            return this.authRefreshLocker.lock(() =>
                this.authService.refreshAuth()
                    .catch((error: any) => this.onAuthRefreshFailed(error)))
                    .catch((error: any) => this.onAuthRefreshLockFailed(error))
                    .then(() => this.retryRequest());
        } else {
            return this.onAuthRefreshFailed(response)
        }
    };

    private retryRequest(){
        return Promise.reject({retry : true});
    };

    private onAuthRefreshFailed(response: Response){
        if(response.status === 401 || response.status === 400){
            this.authService.signOut().then(() => this.navigator.navigate(['/sign-in']));
        }
        return Promise.reject(response);
    };

    private onAuthRefreshLockFailed(error: any){
        if(typeof error === 'AuthRefreshError'){
            return Promise.resolve(error.authRefreshPromise);
        }

        return Promise.reject(error);
    };
}