import {ModuleWithProviders} from '@angular/core';
import {Routes, RouterModule} from '@angular/router';
import {NewsListComponent} from '../news/components/news-list/news-list.component';
import {CanActivateAuth} from '../shared/services/auth/can-activate';

const newsRoutes: Routes = [
    { path: 'news', component: NewsListComponent, canActivate: [CanActivateAuth] }
];

export const NewsRoutingProviders:any[] = [];
export const NewsRouting: ModuleWithProviders = RouterModule.forRoot(newsRoutes);