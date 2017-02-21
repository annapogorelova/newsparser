import {Injectable, Inject} from '@angular/core';
import {Router, ActivatedRoute} from '@angular/router';

/**
 * Class contains url navigation functionality
 */
@Injectable()
export class NavigatorService {
    private queryParams:any = {};

    constructor(@Inject(Router) private router: Router, @Inject(ActivatedRoute) private route: ActivatedRoute){
        this.route.queryParams.subscribe((queryParams) => {
            Object.keys(queryParams).forEach((key) => {
                this.queryParams[key] = queryParams[key];
            });
        });
    }

    /**
     * Sets the query params for the activated route
     * @param name - parameter's name
     * @param value - parameter's value
     * @returns {Promise<boolean>}
     */
    setQueryParam = (name: string, value: any): Promise<boolean> => {
        this.queryParams[name] = value;
        return this.navigate([], {queryParams: this.queryParams});
    };

    /**
     * Navigate to route with a specifies params
     * @param route - route to navigate to
     * @param params - query params
     * @returns {Promise<boolean>}
     */
    navigate = (route: any = [], params: any = {}) => {
        return this.router.navigate(route, params);
    };
}
