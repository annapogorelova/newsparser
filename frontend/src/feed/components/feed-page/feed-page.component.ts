import {Component, OnInit, ViewChild} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {
    NavigatorService,
    CacheService,
    PageTitleService
} from '../../../shared';
import {AppSettings} from '../../../app/app.settings';

@Component({
    selector: 'feed-page',
    templateUrl: './feed-page.component.html',
    styleUrls: ['./feed-page.component.css']
})

/**
 * A container component for feed list and feed sidebar
 */
export class FeedPageComponent implements OnInit {
    @ViewChild('feedListComponent') feedListComponent:any;

    selectedTags:Array<string> = [];
    selectedChannelsIds:Array<number> = [];
    initialPage:number;
    initialSearch:string;

    defaultMarginLeft:number = 0;
    marginLeft:number = this.defaultMarginLeft;

    constructor(private navigator:NavigatorService,
                private route:ActivatedRoute,
                private cacheService:CacheService,
                private pageTitleService:PageTitleService) {
        this.marginLeft = AppSettings.SIDEBAR_WIDTH_PX;
    }

    ngOnInit() {
        this.pageTitleService.appendTitle('Feed');

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
            .map((queryParams) => queryParams['channels'])
            .subscribe((channels:string) => this.initializeSelectedChannels(channels));
    };

    private initializeSelectedChannels(channels:string) {
        var channelsIdsFromUrl = channels ? channels.split(',').map(id => parseInt(id)) : [];
        var channelsIdsFromCache = this.cacheService.get('selectedChannelsIds') || [];

        if (channelsIdsFromUrl.length) {
            this.selectedChannelsIds = channelsIdsFromUrl;
            this.cacheService.set('selectedChannelsIds', this.selectedChannelsIds);
        } else if (channelsIdsFromCache.length) {
            this.selectedChannelsIds = channelsIdsFromCache;
            this.navigator.setQueryParam('channels', this.selectedChannelsIds.join(','));
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

    onChannelSelected(event:any) {
        this.selectedChannelsIds.push(event.channel.id);

        var selectedChannels = this.cacheService.get('selectedChannelsIds') || [];
        selectedChannels.push(event.channel.id);
        this.cacheService.set('selectedChannelsIds', selectedChannels);

        this.navigator.setQueryParam('channels', this.selectedChannelsIds.join(','));
        this.reloadFeed(true);
    };

    onChannelDeselected(event:any) {
        this.selectedChannelsIds = this.selectedChannelsIds.filter(function (item) {
            return item !== event.channel.id;
        });

        var selectedChannels = this.cacheService.get('selectedChannelsIds');
        if (selectedChannels) {
            selectedChannels = selectedChannels.filter(function (item:number) {
                return item !== event.channel.id;
            });
            this.cacheService.set('selectedChannelsIds', selectedChannels);
        }

        this.navigator.setQueryParam('channels', this.selectedChannelsIds.join(','));
        this.reloadFeed(true);
    };

    onChannelsCleared(event:any) {
        this.selectedChannelsIds = [];
        this.cacheService.set('selectedChannelsIds', null);
        this.navigator.setQueryParam('channels', this.selectedChannelsIds.join(','));
        this.reloadFeed(true);
    };

    onTagSelected(event:any) {
        this.selectedTags.push(event.tag);

        var selectedTags = this.cacheService.get('selectedTags') || [];
        selectedTags.push(event.tag);
        this.cacheService.set('selectedTags', selectedTags);

        this.navigator.setQueryParam('tags', this.selectedTags.join(','));
        this.reloadFeed(true);
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
        this.reloadFeed(true);
    };

    onTagsCleared() {
        this.selectedTags = [];
        this.cacheService.set('selectedTags', null);
        this.navigator.setQueryParam('tags', null);
        this.reloadFeed(true);
    };

    onSearch(event:any) {
        this.navigator.setQueryParam('search', event.search);
    };

    onPageChanged(event:any) {
        this.navigator.setQueryParam('page', event.page);
    };

    reloadFeed(refresh:boolean = false) {
        this.feedListComponent.reload(refresh, this.selectedChannelsIds, this.selectedTags);
    };

    showLockScreen():boolean {
        return this.feedListComponent.requestLocker.isLocked();
    };
}