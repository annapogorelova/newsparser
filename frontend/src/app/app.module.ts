import { NgModule } from '@angular/core';
import { HttpModule } from '@angular/http';
import { BrowserModule } from '@angular/platform-browser';
import { SharedModule } from '../shared/shared.module';
import { AppComponent }  from './components/app.component';
import { NewsModule } from '../news/news.module';
import { AppRoutingProviders, AppRouting } from './app.routing';
import { AccountModule } from '../account/account.module';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import {NewsSourcesModule} from '../news-sources/news-sources.module';


@NgModule({
  imports: [ BrowserModule, SharedModule, HttpModule, NewsModule, NewsSourcesModule, AccountModule, AppRouting, NgbModule.forRoot()],
  declarations: [ AppComponent ],
  bootstrap:    [ AppComponent ],
  providers: [ AppRoutingProviders ]
})
export class AppModule { }