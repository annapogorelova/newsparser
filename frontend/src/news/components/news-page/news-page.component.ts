import {Component, OnInit, ViewChild} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {NavigatorService, CacheService} from '../../../shared';
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
    @ViewChild('newsListComponent') newsListComponent:any;

    selectedTags:Array<string> = [];
    selectedSourcesIds:Array<number> = [];
    initialPage:number;
    initialSearch:string;

    defaultMarginLeft:number = 0;
    marginLeft:number = this.defaultMarginLeft;

    constructor(private navigator:NavigatorService,
                private route:ActivatedRoute,
                private cacheService:CacheService) {
        this.marginLeft = AppSettings.SIDEBAR_WIDTH_PX;
    }

    ngOnInit() {
        this.route.queryParams
            .map((queryParams) => queryParams['page'])
            .subscribe((page:string) => {
                this.initialPage = !isNaN(parseInt(page)) ? parseInt(page) : 1;
            });

        this.route.queryParams
            .map((queryParams) => queryParams['search'])
            .subscribe((search:string) => this.initialSearch = search);

        this.route.queryParams
            .map((queryParams) => queryParams['tags'])
            .subscribe((tags:string) => this.initializeSelectedTags(tags));

        this.route.queryParams
            .map((queryParams) => queryParams['sources'])
            .subscribe((sources:string) => this.initializeSelectedSources(sources));
    };

    private initializeSelectedSources(sources:string) {
        var sourcesIdsFromUrl = sources ? sources.split(',').map(id => parseInt(id)) : [];
        var sourcesIdsFromCache = this.cacheService.get('selectedSourcesIds') || [];

        if (sourcesIdsFromUrl.length) {
            this.selectedSourcesIds = sourcesIdsFromUrl;
            this.cacheService.set('selectedSourcesIds', this.selectedSourcesIds);
        } else if (sourcesIdsFromCache.length) {
            this.selectedSourcesIds = sourcesIdsFromCache;
            this.navigator.setQueryParam('sources', this.selectedSourcesIds.join(','));
        }
    };

    private initializeSelectedTags(tags:string) {
        var tagsFromUrl = tags ? tags.split(',') : [];
        var tagsFromCache = this.cacheService.get('selectedTags') || [];

        if (tagsFromUrl.length) {
            this.selectedTags = tagsFromUrl;
            this.cacheService.set('selectedTags', this.selectedTags);
        } else if (tagsFromCache.length) {
            this.selectedTags = tagsFromCache;
            this.navigator.setQueryParam('tags', this.selectedTags.join(','));
        }
    };

    onSidebarHidden() {
        this.marginLeft = this.defaultMarginLeft;
    };

    onSidebarShown() {
        this.marginLeft = AppSettings.SIDEBAR_WIDTH_PX;
    };

    onSourceSelected(event:any) {
        this.selectedSourcesIds.push(event.source.id);

        var selectedSources = this.cacheService.get('selectedSourcesIds') || [];
        selectedSources.push(event.source.id);
        this.cacheService.set('selectedSourcesIds', selectedSources);

        this.navigator.setQueryParam('sources', this.selectedSourcesIds.join(','));
        this.reloadNews();
    };

    onSourceDeselected(event:any) {
        this.selectedSourcesIds = this.selectedSourcesIds.filter(function (item) {
            return item !== event.source.id;
        });

        var selectedSources = this.cacheService.get('selectedSourcesIds');
        if (selectedSources) {
            selectedSources = selectedSources.filter(function (item:number) {
                return item !== event.source.id;
            });
            this.cacheService.set('selectedSourcesIds', selectedSources);
        }

        this.navigator.setQueryParam('sources', this.selectedSourcesIds.join(','));
        this.reloadNews();
    };

    onSourcesCleared(event:any) {
        this.selectedSourcesIds = [];
        this.cacheService.set('selectedSourcesIds', null);
        this.navigator.setQueryParam('sources', this.selectedSourcesIds.join(','));
        this.reloadNews();
    };

    onTagSelected(event:any) {
        this.selectedTags.push(event.tag);

        var selectedTags = this.cacheService.get('selectedTags') || [];
        selectedTags.push(event.tag);
        this.cacheService.set('selectedTags', selectedTags);

        this.navigator.setQueryParam('tags', this.selectedTags.join(','));
        this.reloadNews();
    };

    onTagDeselected(event:any) {
        this.selectedTags = this.selectedTags.filter(function (selectedTag) {
            return selectedTag !== event.tag;
        });

        var selectedTags = this.cacheService.get('selectedTags');
        if (selectedTags) {
            selectedTags = selectedTags.filter(function (item:number) {
                return item !== event.tag;
            });
            this.cacheService.set('selectedTags', selectedTags);
        }

        this.navigator.setQueryParam('tags', this.selectedTags.join(','));
        this.reloadNews();
    };

    onTagsCleared() {
        this.selectedTags = [];
        this.cacheService.set('selectedTags', null);
        this.navigator.setQueryParam('tags', null);
        this.reloadNews();
    };

    onSearch(event:any) {
        this.navigator.setQueryParam('search', event.search);
    };

    onPageChanged(event:any) {
        this.navigator.setQueryParam('page', event.page);
    };

    reloadNews(refresh:boolean = false) {
        this.newsListComponent.reload(refresh, this.selectedSourcesIds, this.selectedTags);
    };

    showLockScreen():boolean {
        return this.newsListComponent.requestLocker.isLocked();
    };
}