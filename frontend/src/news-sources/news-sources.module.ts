import {NgModule} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';
import {SharedModule} from '../shared/shared.module';
import {NewsSourcesRouting, NewsSourcesRoutingProviders} from './news-sources.routing';
import {AddNewsSourceComponent} from './components/add-news-source/add-news-source.component';
import {FormsModule} from '@angular/forms';
import {InfiniteScrollModule} from 'angular2-infinite-scroll';
import {SubscriptionsComponent} from './components/subscriptions/subscriptions.component';
import {NewsSourcesListComponent} from './components/news-sources-list/news-sources-list.component';

@NgModule({
    imports: [NewsSourcesRouting, BrowserModule, SharedModule, FormsModule, InfiniteScrollModule],
    declarations: [SubscriptionsComponent, AddNewsSourceComponent, NewsSourcesListComponent],
    providers: [NewsSourcesRoutingProviders],
    exports: [NewsSourcesListComponent]
})
export class NewsSourcesModule {}