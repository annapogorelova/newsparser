import {ModuleWithProviders} from '@angular/core';
import {Routes, RouterModule} from '@angular/router';
import {CanActivateAuth} from '../shared/services/auth/can-activate';
import {SubscriptionsComponent} from './components/subscriptions/subscriptions.component';

const newsRoutes: Routes = [
    {path: 'news-sources', component: SubscriptionsComponent, canActivate: [CanActivateAuth]}
];

export const NewsSourcesRoutingProviders: any[] = [];
export const NewsSourcesRouting: ModuleWithProviders = RouterModule.forRoot(newsRoutes);