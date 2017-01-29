"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
var core_1 = require('@angular/core');
var http_1 = require('@angular/http');
var app_settings_1 = require('../../../app/app.settings');
require('rxjs/Rx');
var Observable_1 = require('rxjs/Observable');
var ApiService = (function () {
    function ApiService(http) {
        var _this = this;
        this.http = http;
        this.get = function (url, params, options) {
            return _this.http.get(_this.getAbsoluteUrl(url))
                .map(_this.extractData)
                .catch(_this.handleError);
        };
        this.post = function (url, params, options) {
            return _this.http.post(_this.getAbsoluteUrl(url), _this.initializeBody(params))
                .map(_this.extractData)
                .catch(_this.handleError);
        };
        this.initializeBody = function (body) {
            return body || {};
        };
    }
    ApiService.prototype.extractData = function (response) {
        var body = response.json();
        return body.data || {};
    };
    ;
    ApiService.prototype.handleError = function (response) {
        return Observable_1.Observable.throw(new Error(response.toString()));
    };
    ;
    ApiService.prototype.getAbsoluteUrl = function (url) {
        return app_settings_1.AppSettings.API_ENDPOINT + url;
    };
    ;
    ApiService = __decorate([
        core_1.Injectable(), 
        __metadata('design:paramtypes', [http_1.Http])
    ], ApiService);
    return ApiService;
}());
exports.ApiService = ApiService;
//# sourceMappingURL=api.service.js.map