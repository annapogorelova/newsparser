import {Component, Inject, Input} from '@angular/core';
import {ApiService} from '../../../shared/services/api/api.service';

@Component({
    selector: 'news-sources',
    templateUrl: './news-sources-list.component.html',
    styles: [require('./news-sources-list.component.css').toString()]
})

/**
 * Component for displaying the list of news sources
 */
export class NewsSourcesListComponent {
    public newsSources: any = [{name: 'All', id: null}];
    public selectedSourceId: number;

    @Input() selectHandler: any = null;
    @Input() initialSelectedSourceId: any = null;

    constructor(private apiService: ApiService){
        this.loadNewsSources();
    }

    loadNewsSources = () => {
        this.apiService.get('subscription').then(newsSources => this.handleLoadedNewsSources(newsSources));
    };

    handleLoadedNewsSources = (data: any) => {
        this.newsSources = this.newsSources.concat(data);
        this.selectedSourceId = this.initialSelectedSourceId;
    };

    onSelect = (source: any) => {
        this.selectedSourceId = source.id;

        if(this.selectHandler){
            this.selectHandler(source);
        }
    };
}