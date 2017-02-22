import {ModuleWithProviders} from '@angular/core';
import {Routes, RouterModule} from '@angular/router';
import {CanActivateAuth} from '../shared/services/auth/can-activate';
import {UserSubscriptionsComponent} from './components/user-subscriptions/user-subscriptions.component';

const newsRoutes: Routes = [
    {path: 'news-sources', component: UserSubscriptionsComponent, canActivate: [CanActivateAuth]}
];

export const NewsSourcesRoutingProviders: any[] = [];
export const NewsSourcesRouting: ModuleWithProviders = RouterModule.forRoot(newsRoutes);