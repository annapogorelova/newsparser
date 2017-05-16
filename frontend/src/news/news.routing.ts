import {ModuleWithProviders} from '@angular/core';
import {Routes, RouterModule} from '@angular/router';
import {NewsPageComponent} from './components/news-page/news-page.component';
import {CanActivateAuth} from '../shared/services/auth/can-activate';

const newsRoutes: Routes = [
    { path: 'news', component: NewsPageComponent, canActivate: [CanActivateAuth] }
];

export const NewsRoutingProviders:any[] = [];
export const NewsRouting: ModuleWithProviders = RouterModule.forRoot(newsRoutes);