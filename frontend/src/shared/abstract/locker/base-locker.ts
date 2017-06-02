/**
 * Base locker class
 */
export class BaseLocker {
    protected callbackPromise:Promise<any> = null;

    isLocked() {
        return this.callbackPromise != null;
    };

    lock(callback:any) {
        this.callbackPromise = this.execute(callback);
        return this.callbackPromise
            .then((data:any) => this.onCallbackSuccess(data))
            .catch((error:any) => this.onCallbackError(error));
    };

    execute(callback:any):Promise<any> {
        return new Promise((resolve) => {
            resolve(callback());
        });
    };

    onCallbackSuccess(data:any) {
        this.unlock();
        return data;
    };

    onCallbackError(error:any) {
        this.unlock();
        throw error;
    };

    unlock() {
        this.callbackPromise = null;
    };
}