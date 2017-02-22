import {NgModule} from '@angular/core';
import {NewsListComponent} from './components/news-list/news-list.component';
import {NewsRouting, NewsRoutingProviders} from './news.routing';
import {BrowserModule} from '@angular/platform-browser';
import {InfiniteScrollModule} from 'angular2-infinite-scroll';
import {SharedModule} from '../shared/shared.module';
import {NewsSourcesListComponent} from './components/news-sources-list/news-sources-list.component';
import {FormsModule} from '@angular/forms';

@NgModule({
    imports: [NewsRouting, BrowserModule, InfiniteScrollModule, SharedModule, FormsModule],
    declarations: [NewsListComponent, NewsSourcesListComponent],
    providers: [NewsRoutingProviders]
})
export class NewsModule {
}