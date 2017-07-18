import {ModuleWithProviders} from '@angular/core';
import {Routes, RouterModule} from '@angular/router';
import {FeedPageComponent} from './components';
import {CanActivatePrivate} from '../shared/services/auth/can-activate-private';

const feedRoutes:Routes = [
    {path: 'feed', component: FeedPageComponent, canActivate: [CanActivatePrivate]}
];

export const FeedRoutingProviders:any[] = [];
export const FeedRouting:ModuleWithProviders = RouterModule.forRoot(feedRoutes);