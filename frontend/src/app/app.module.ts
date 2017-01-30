import { NgModule } from '@angular/core';
import { HttpModule } from '@angular/http';
import { BrowserModule } from '@angular/platform-browser';
import { SharedModule } from '../shared/shared.module';
import { AppComponent }  from './components/app.component';
import { NewsModule } from '../news/news.module';
import { AppRoutingProviders, AppRouting } from './app.routing';
import { AccountModule } from '../account/account.module';
import { LocalStorageModule } from 'angular-2-local-storage';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';


@NgModule({
  imports: [ BrowserModule, SharedModule, HttpModule, NewsModule, AccountModule, AppRouting,
    LocalStorageModule.withConfig({
      prefix: 'news-app',
      storageType: 'localStorage'
    }), NgbModule.forRoot()],
  declarations: [ AppComponent ],
  bootstrap:    [ AppComponent ],
  providers: [ AppRoutingProviders ]
})
export class AppModule { }