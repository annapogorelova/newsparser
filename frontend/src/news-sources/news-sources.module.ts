import {NgModule} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';
import {SharedModule} from '../shared/shared.module';
import {UserSubscriptionsComponent} from './components/user-subscriptions/user-subscriptions.component';
import {NewsSourcesRouting, NewsSourcesRoutingProviders} from './news-sources.routing';
import {AddNewsSourceComponent} from './components/add-news-source/add-news-source.component';
import {FormsModule} from '@angular/forms';
import {AvailableNewsSourcesComponent} from './components/available-news-sources-list/available-news-sources.component';
import {InfiniteScrollModule} from 'angular2-infinite-scroll';

@NgModule({
    imports: [NewsSourcesRouting, BrowserModule, SharedModule, FormsModule, InfiniteScrollModule],
    declarations: [UserSubscriptionsComponent, AddNewsSourceComponent, AvailableNewsSourcesComponent],
    providers: [NewsSourcesRoutingProviders]
})
export class NewsSourcesModule {}