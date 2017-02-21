import {Observable} from 'rxjs';
import {Response} from '@angular/http';
import {LocalStorageService} from 'angular-2-local-storage';
import {NavigatorService} from '../navigator/navigator.service';
import {Inject} from '@angular/core';

/**
 * Class contains functionality to handle http request errors
 */
export class ApiErrorHandler {
    constructor(@Inject(LocalStorageService) private localStorageService: LocalStorageService,
                @Inject(NavigatorService) private navigator: NavigatorService){}

    /**
     * General method for handling http errors
     * @param response - Response object
     * @returns {any}
     */
    handleResponse = (response: Response) => {
        switch(response.status){
            case 401:
                this.handleUnauthorizedError();
                break;
        }

        let body = response.json();
        return Observable.throw(new Error(body.message));
    };

    /**
     * Handles the 401 status
     */
    private handleUnauthorizedError = () => {
        this.localStorageService.clearAll();
        this.navigator.navigate(['/sign-in']);
    };
}