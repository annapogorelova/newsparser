import {NgModule} from '@angular/core';
import {FormsModule} from '@angular/forms';
import {BrowserModule} from '@angular/platform-browser';
import {InfiniteScrollModule} from 'angular2-infinite-scroll';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';
import {NewsListComponent} from './components';
import {NewsRouting, NewsRoutingProviders} from './news.routing';
import {SharedModule} from '../shared';
import {NewsSourcesModule} from '../news-sources';

@NgModule({
    imports: [
        NewsRouting,
        BrowserModule,
        InfiniteScrollModule,
        SharedModule,
        FormsModule,
        NewsSourcesModule,
        NgbModule
    ],
    declarations: [NewsListComponent],
    providers: [NewsRoutingProviders]
})
export class NewsModule {
}