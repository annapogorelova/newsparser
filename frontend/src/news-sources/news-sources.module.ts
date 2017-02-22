import {NgModule} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';
import {SharedModule} from '../shared/shared.module';
import {UserSubscriptionsComponent} from './components/user-subscriptions/user-subscriptions.component';
import {NewsSourcesRouting, NewsSourcesRoutingProviders} from './news-sources.routing';
import {AddNewsSourceComponent} from './components/add-news-source/add-news-source.component';
import {FormsModule} from '@angular/forms';

@NgModule({
    imports: [NewsSourcesRouting, BrowserModule, SharedModule, FormsModule],
    declarations: [UserSubscriptionsComponent, AddNewsSourceComponent],
    providers: [NewsSourcesRoutingProviders]
})
export class NewsSourcesModule {}