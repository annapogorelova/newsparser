import {Routes, RouterModule} from '@angular/router';
import {ModuleWithProviders} from '@angular/core';
import {CanActivateAuth, PageNotFoundComponent} from '../shared';

const appRoutes:Routes = [
    {path: '', redirectTo: '/news', pathMatch: 'full', canActivate: [CanActivateAuth]},
    {path: '**', component: PageNotFoundComponent}
];

export const AppRoutingProviders:any[] = [];
export const AppRouting:ModuleWithProviders = RouterModule.forRoot(appRoutes);