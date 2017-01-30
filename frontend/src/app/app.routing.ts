import {Routes, RouterModule} from '@angular/router';
import {ModuleWithProviders} from '@angular/core';
import {NewsListComponent} from '../news/components/news-list/news-list.component';
import {SignInComponent} from '../account/sign-in/sign-in.component';
import {CanActivateAuth} from '../shared/services/auth/can-activate';
import {RegisterComponent} from '../account/register/register.component';
import {PageNotFoundComponent} from '../shared/components/page-not-found/page-not-found.component';

const appRoutes: Routes = [
    { path: '', redirectTo: '/news', pathMatch: 'full', canActivate: [CanActivateAuth] },
    { path: 'sign-in', component: SignInComponent },
    { path: 'register', component: RegisterComponent },
    { path: '**', component: PageNotFoundComponent }
];

export const AppRoutingProviders:any[] = [];
export const AppRouting:ModuleWithProviders = RouterModule.forRoot(appRoutes);