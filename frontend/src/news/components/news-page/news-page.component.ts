import {Component, OnInit, ViewChild} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {NavigatorService} from '../../../shared/services/navigator/navigator.service';
import {AppSettings} from '../../../app/app.settings';

@Component({
    selector: 'news-page',
    templateUrl: './news-page.component.html',
    styleUrls: ['./news-page.component.css']
})

/**
 * A container component for news list and news sidebar
 */
export class NewsPageComponent implements OnInit {
    @ViewChild('newsListComponent') newsListComponent: any;
    
    selectedTags: Array<string> = [];
    selectedSourcesIds: Array<number> = [];
    initialPage: number;
    initialSearch: string;
    
    defaultMarginLeft: number = 0;
    marginLeft: number = this.defaultMarginLeft;
    
    constructor(private navigator: NavigatorService,
                private route:ActivatedRoute){
        this.marginLeft = AppSettings.SIDEBAR_WIDTH_PX;
    }

    ngOnInit(){
        this.route.queryParams
            .map((queryParams) => queryParams['page'])
            .subscribe((page: string) => {
                this.initialPage = !isNaN(parseInt(page)) ? parseInt(page) : 1;
            });

        this.route.queryParams
            .map((queryParams) => queryParams['search'])
            .subscribe((search: string) => this.initialSearch = search);
        
        this.route.queryParams
            .map((queryParams) => queryParams['tags'])
            .subscribe((tags: string) => this.selectedTags = tags ? tags.split(',') : []);

        this.route.queryParams
            .map((queryParams) => queryParams['sources'])
            .subscribe((sources: string) =>
                this.selectedSourcesIds = sources ? sources.split(',').map(id => parseInt(id)) : []);
    };

    onSidebarHidden(){
        this.marginLeft = this.defaultMarginLeft;
    };

    onSidebarShown(){
        this.marginLeft = AppSettings.SIDEBAR_WIDTH_PX;
    };

    onSourceSelected(event: any){
        this.selectedSourcesIds.push(event.source.id);
        this.navigator.setQueryParam('sources', this.selectedSourcesIds.join(','));
        this.newsListComponent.reload(false, this.selectedSourcesIds, this.selectedTags);
    };

    onSourceDeselected(event: any){
        this.selectedSourcesIds = this.selectedSourcesIds.filter(function (item) {
            return item !== event.source.id;
        });
        this.navigator.setQueryParam('sources', this.selectedSourcesIds.join(','));
        this.newsListComponent.reload(false, this.selectedSourcesIds, this.selectedTags);
    };
    
    onTagSelected(event: any){
        this.selectedTags.push(event.tag);
        this.navigator.setQueryParam('tags', this.selectedTags.join(','));
        this.newsListComponent.reload(false, this.selectedSourcesIds, this.selectedTags);
    };
    
    onTagDeselected(event: any){
        this.selectedTags = this.selectedTags.filter(function(selectedTag){
            return selectedTag !== event.tag;
        });

        this.navigator.setQueryParam('tags', this.selectedTags.join(','));
        this.newsListComponent.reload(false, this.selectedSourcesIds, this.selectedTags);
    };

    onTagsCleared(){
        this.selectedTags = [];
        this.navigator.setQueryParam('tags', null);
        this.newsListComponent.reload(false, this.selectedSourcesIds, this.selectedTags);
    };

    onSearch(event: any){
        this.navigator.setQueryParam('search', event.search);
    };

    onPageChanged(event: any){
        this.navigator.setQueryParam('page', event.page);
    };
}