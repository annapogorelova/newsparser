import {Routes, RouterModule} from '@angular/router';
import {ModuleWithProviders} from '@angular/core';
import {PageNotFoundComponent} from '../shared';
import {LandingComponent} from './components';

const appRoutes:Routes = [
    {path: '', component: LandingComponent},
    {path: '**', component: PageNotFoundComponent}
];

export const AppRoutingProviders:any[] = [];
export const AppRouting:ModuleWithProviders = RouterModule.forRoot(appRoutes);