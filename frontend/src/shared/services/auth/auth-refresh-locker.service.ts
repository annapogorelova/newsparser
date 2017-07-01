import {Injectable} from '@angular/core';
import {BaseLocker} from '../../abstract';

/**
 * A wrapper for the process of refreshing the authentication
 */
@Injectable()
export class AuthRefreshLocker extends BaseLocker {
    lock(callback:any):Promise<any> {
        if (super.isLocked()) {
            return Promise.reject(new AuthRefreshError('Refresh already in progress', this.callbackPromise));
        }

        return super.lock(callback);
    }
}

/**
 * The Error type to be thrown when auth refresh failed
 */
export class AuthRefreshError extends Error {
    constructor(message:string, public authRefreshPromise:Promise<any>) {
        super(message);
    }
}