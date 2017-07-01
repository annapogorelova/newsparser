import {ModuleWithProviders} from '@angular/core';
import {Routes, RouterModule} from '@angular/router';
import {FeedPageComponent} from './components';
import {CanActivateAuth} from '../shared/services/auth/can-activate';

const feedRoutes:Routes = [
    {path: 'feed', component: FeedPageComponent, canActivate: [CanActivateAuth]}
];

export const FeedRoutingProviders:any[] = [];
export const FeedRouting:ModuleWithProviders = RouterModule.forRoot(feedRoutes);