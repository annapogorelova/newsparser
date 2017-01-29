import { NgModule } from '@angular/core';
import { ApiService } from './services/api/api.service';
import {AuthService, CanActivateAuth} from './services/auth/auth.service';
import {PageNotFoundComponent} from './components/page-not-found/page-not-found.component';

@NgModule({
    providers: [ApiService, AuthService, CanActivateAuth],
    declarations: [PageNotFoundComponent],
    exports: [PageNotFoundComponent]
})

export class SharedModule {}