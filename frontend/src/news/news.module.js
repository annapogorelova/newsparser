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
var news_list_component_1 = require("./components/news-list/news-list.component");
var NewsModule = (function () {
    function NewsModule() {
    }
    NewsModule = __decorate([
        core_1.NgModule({
            declarations: [news_list_component_1.NewsListComponent],
            exports: [news_list_component_1.NewsListComponent]
        }), 
        __metadata('design:paramtypes', [])
    ], NewsModule);
    return NewsModule;
}());
exports.NewsModule = NewsModule;
//# sourceMappingURL=news.module.js.map