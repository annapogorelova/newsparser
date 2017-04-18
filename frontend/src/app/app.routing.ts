import {Routes, RouterModule} from '@angular/router';
import {ModuleWithProviders} from '@angular/core';
import {SignInComponent} from '../account/components/sign-in/sign-in.component';
import {CanActivateAuth} from '../shared/services/auth/can-activate';
import {RegisterComponent} from '../account/components/register/register.component';
import {PageNotFoundComponent} from '../shared/components/page-not-found/page-not-found.component';
import {AccountConfirmationComponent} from "../account/components/confirmation/account-confirmation.component";

const appRoutes: Routes = [
    { path: '', redirectTo: '/news', pathMatch: 'full', canActivate: [CanActivateAuth] },
    { path: 'sign-in', component: SignInComponent },
    { path: 'register', component: RegisterComponent },
    { path: 'confirmation', component: AccountConfirmationComponent },
    { path: '**', component: PageNotFoundComponent }
];

export const AppRoutingProviders:any[] = [];
export const AppRouting:ModuleWithProviders = RouterModule.forRoot(appRoutes);