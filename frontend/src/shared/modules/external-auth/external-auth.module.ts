import { NgModule, ModuleWithProviders } from '@angular/core';
import {IProviders, ExternalAuthService} from './external-auth.service';

declare let gapi: any;
declare let FB: any;

@NgModule()
export class ExternalAuthModule {
    static initWithProviders(config: IProviders): ModuleWithProviders{
        let loadProvidersScripts: Object = {
            google: (info: any) => {
                let d = document, gJs: any, ref = d.getElementsByTagName('script')[0];
                gJs = d.createElement('script');
                gJs.async = true;
                gJs.src = "//apis.google.com/js/platform.js";

                gJs.onload = function() {
                    gapi.load('auth2', function() {
                        gapi.auth2.init({
                            client_id: info["clientId"],
                            scope: 'email'
                        })
                    })
                };
                ref.parentNode.insertBefore(gJs, ref);
            },
            linkedin: (info: any) => {
                let lIN: any, d = document, ref = d.getElementsByTagName('script')[0];
                lIN = d.createElement('script');
                lIN.async = false;
                lIN.src = "//platform.linkedin.com/in.js";
                lIN.text = ("api_key: " + info["clientId"]).replace("\"", "");
                ref.parentNode.insertBefore(lIN, ref);
            },
            facebook: (info: any) => {
                let d = document, fbJs: any, id = 'facebook-jssdk', ref = d.getElementsByTagName('script')[0];
                fbJs = d.createElement('script');
                fbJs.id = id;
                fbJs.async = true;
                fbJs.src = "//connect.facebook.net/en_US/sdk.js";

                fbJs.onload = function() {
                    FB.init({
                        appId: info["clientId"],
                        status: true,
                        cookie: true,
                        xfbml: true,
                        version: info["apiVersion"]
                    });
                };

                ref.parentNode.insertBefore(fbJs, ref);
            }
        };

        Object.keys(config).forEach((provider) => {
            loadProvidersScripts[provider](config[provider]);
        });

        return {
            ngModule: ExternalAuthModule,
            providers: [ExternalAuthService]
        };
    };
}