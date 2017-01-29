"use strict";
var router_1 = require('@angular/router');
var news_list_component_1 = require('../news/components/news-list/news-list.component');
var sign_in_component_1 = require('../account/sign-in/sign-in.component');
var appRoutes = [
    { path: '', redirectTo: '/news', pathMatch: 'full' },
    { path: 'news', component: news_list_component_1.NewsListComponent },
    { path: 'sign-in', component: sign_in_component_1.SignInComponent }
];
exports.AppRoutingProviders = [];
exports.AppRouter = router_1.RouterModule.forRoot(appRoutes);
//# sourceMappingURL=app.router.js.map