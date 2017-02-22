import {Component} from '@angular/core';
import {ApiService} from '../../../shared/services/api/api.service';

/**
 * A component for adding a new news source
 */
@Component({
    selector: 'add-news-source',
    templateUrl: 'add-news-source.component.html',
    styles: [require('./add-news-source.component.css').toString()]
})

export class AddNewsSourceComponent {
    public rssUrl: string = null;
    public addingInProgress: boolean = false;

    constructor(private apiService: ApiService){ }

    addNewsSource = () => {
        if(!this.rssUrl){
            return;
        }
        this.addingInProgress = true;
        this.apiService.post('newsSources', {rssUrl: this.rssUrl})
            .then(response => this.handleResponse(response))
            .catch(error => this.handleError(error));
    };

    handleResponse = (response: any) => {
        this.addingInProgress = false;
        this.reset();
        alert('News source added');
    };

    handleError = (error: any) => {
        this.addingInProgress = false;
        this.reset();
        alert('Failed to add a news source');
    };

    reset = () => {
        this.rssUrl = null;
    };
}