import {Injectable} from '@angular/core';

/**
 * A wrapper for the process of refreshing the authentication
 */
@Injectable()
export class AuthRefreshLocker {
    private authRefreshPromise: Promise<any> = null;

    isLocked(){
        return this.authRefreshPromise != null;
    }

    lock(callback: any): Promise<any>{
        if (this.isLocked()) {
            return Promise.reject(new AuthRefreshError('Refresh already in progress', this.authRefreshPromise));
        }
        this.authRefreshPromise = new Promise((resolve, reject) => {
            resolve(callback());
        }).then(this.unlock).catch(this.unlock);

        return this.authRefreshPromise;
    }

    unlock = () => {
        this.authRefreshPromise = null;
    }
}

export class AuthRefreshError extends Error {
    constructor(message: string, public authRefreshPromise: Promise<any>){
        super(message);
    }
}