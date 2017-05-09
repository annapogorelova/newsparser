import {NgModule} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';
import {FormsModule} from '@angular/forms';
import {InfiniteScrollModule} from 'angular2-infinite-scroll';
import {SharedModule} from '../shared';
import {NewsSourcesRouting, NewsSourcesRoutingProviders} from './news-sources.routing';
import {
    SubscriptionsComponent,
    NewsSourcesListComponent,
    AddNewsSourceComponent
} from './components';

@NgModule({
    imports: [NewsSourcesRouting, BrowserModule, SharedModule, FormsModule, InfiniteScrollModule],
    declarations: [SubscriptionsComponent, AddNewsSourceComponent, NewsSourcesListComponent],
    providers: [NewsSourcesRoutingProviders],
    exports: [NewsSourcesListComponent]
})
export class NewsSourcesModule {}