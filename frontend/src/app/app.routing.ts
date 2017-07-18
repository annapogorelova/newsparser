import {Routes, RouterModule} from '@angular/router';
import {ModuleWithProviders} from '@angular/core';
import {CanActivatePrivate, PageNotFoundComponent} from '../shared';

const appRoutes:Routes = [
    {path: '', redirectTo: '/feed', pathMatch: 'full', canActivate: [CanActivatePrivate]},
    {path: '**', component: PageNotFoundComponent}
];

export const AppRoutingProviders:any[] = [];
export const AppRouting:ModuleWithProviders = RouterModule.forRoot(appRoutes);