import { NgModule } from '@angular/core';
import { ApiService } from './services/api/api.service';
import { AuthService } from './services/auth/auth.service';
import { PageNotFoundComponent } from './components/page-not-found/page-not-found.component';
import { CanActivateAuth } from './services/auth/can-activate';

@NgModule({
    providers: [ApiService, AuthService, CanActivateAuth],
    declarations: [PageNotFoundComponent],
    exports: [PageNotFoundComponent]
})

export class SharedModule {}