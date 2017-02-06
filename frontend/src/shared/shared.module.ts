import { NgModule } from '@angular/core';
import { ApiService } from './services/api/api.service';
import { AuthService } from './services/auth/auth.service';
import { PageNotFoundComponent } from './components/page-not-found/page-not-found.component';
import { CanActivateAuth } from './services/auth/can-activate';
import {GoTopComponent} from './components/go-top-button/go-top-button.component';
import {BrowserModule} from '@angular/platform-browser';

@NgModule({
    imports: [ BrowserModule ],
    providers: [ ApiService, AuthService, CanActivateAuth ],
    declarations: [ PageNotFoundComponent, GoTopComponent ],
    exports: [ PageNotFoundComponent, GoTopComponent ]
})

export class SharedModule {}