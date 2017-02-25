import {ModuleWithProviders} from '@angular/core';
import {Routes, RouterModule} from '@angular/router';
import {CanActivateAuth} from '../shared/services/auth/can-activate';
import {NewsSourcesSettingsComponent} from './components/news-sources-settings/news-sources-settings.component';

const newsRoutes: Routes = [
    {path: 'news-sources', component: NewsSourcesSettingsComponent, canActivate: [CanActivateAuth]}
];

export const NewsSourcesRoutingProviders: any[] = [];
export const NewsSourcesRouting: ModuleWithProviders = RouterModule.forRoot(newsRoutes);