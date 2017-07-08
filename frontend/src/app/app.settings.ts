export class AppSettings {
    public static API_ENDPOINT = process.env.API_ENDPOINT;
    public static DEFAULT_PAGE_SIZE = 10; // number of items to load for collection GET
    public static CHANNELS_PAGE_SIZE = 15; // number of feed channels per page
    public static DEFAULT_SEARCH_PLACEHOLDER_TEXT = 'Type to search...';
    public static DEFAULT_CACHE_DURATION_SECONDS = 300;

    // External auth
    public static GOOGLE_CLIENT_ID = process.env.GOOGLE_CLIENT_ID;
    public static FACEBOOK_CLIENT_ID = process.env.FACEBOOK_CLIENT_ID;
    public static FACEBOOK_API_VERSION = 'v2.8';
    
    public static DEFAULT_ERROR_MESSAGE = 'Something went wrong';
    
    public static SIDEBAR_WIDTH_PX = 350;
}