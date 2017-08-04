import {Injectable} from '@angular/core';

@Injectable()
export class WindowProviderService {
    constructor() {}

    getNativeWindow() {
        return window;
    };
}