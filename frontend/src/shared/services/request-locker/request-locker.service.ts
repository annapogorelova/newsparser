import {BaseLocker} from '../../abstract';

/**
 * Locker service for requests
 */
export class RequestLockerService extends BaseLocker
{
    lock(callback:any):Promise<any> {
        if (this.isLocked()) {
            return Promise.reject(this.callbackPromise);
        }

        return super.lock(callback);
    };
}