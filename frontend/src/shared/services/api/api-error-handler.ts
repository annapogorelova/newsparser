import {Observable} from 'rxjs';
import {Response} from '@angular/http';
import {NavigatorService} from '../navigator/navigator.service';
import {Inject} from '@angular/core';
import {CacheService} from '../cache/cache.service';
import {HttpError} from "./http-error";

/**
 * Class contains functionality to handle http request errors
 */
export class ApiErrorHandler {
    constructor(@Inject(NavigatorService) private navigator: NavigatorService,
                @Inject(CacheService) private cacheService: CacheService){}

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
        return Observable.throw(new HttpError(body.message, response.status, body.validationErrors));
    };

    /**
     * Handles the 401 status
     */
    private handleUnauthorizedError = () => {
        this.cacheService.clear();
        this.navigator.navigate(['/sign-in']);
    };
}