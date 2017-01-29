import {Component, Inject} from '@angular/core';
import {AuthService} from '../shared/services/auth/auth.service';
import {LocalStorageService} from 'angular-2-local-storage';
import {Router} from '@angular/router';

@Component({
  selector: 'app',
  templateUrl: './app.component.html',
})
export class AppComponent  {

  constructor(private authService: AuthService, private localStorageService: LocalStorageService,
              @Inject(Router) private router: Router){}

  signOut = () => {
    this.localStorageService.clearAll();
    this.router.navigate(['/sign-in']);
  };
}
