import {ModuleWithProviders} from '@angular/core';
import {Routes, RouterModule} from '@angular/router';
import {CanActivateAuth} from '../shared/services/auth/can-activate';
import {NewsSourcesListComponent} from './components/news-sources-list/news-sources-list.component';

const newsRoutes: Routes = [
    {path: 'news-sources', component: NewsSourcesListComponent, canActivate: [CanActivateAuth]}
];

export const NewsSourcesRoutingProviders: any[] = [];
export const NewsSourcesRouting: ModuleWithProviders = RouterModule.forRoot(newsRoutes);