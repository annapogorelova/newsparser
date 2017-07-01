/**
 * 
 * @api {get} /api/feed?pageSize=&pageIndex=&search=&sources=&tags= Get feed
 * @apiName GetFeed
 * @apiGroup Feed
 * @apiVersion  1.0.0
 * @apiHeader Authorization Bearer eyJhbGciOiJIUzNiIsInR5...
 * 
 * 
 * @apiParam  {Number} [pageSize=5] Page size
 * @apiParam  {Number} [pageIndex=0] Page size
 * @apiParam  {String} [search] Search
 * @apiParam  {String[]} [sources] Comma-separated list of sources ids
 * @apiParam  {String[]} [tags] Comma-separated list of tags ids
 * 
 * 
 * @apiSuccessExample {json} Success-Response:
   HTTP/1.1 200 OK
   {
       "data": [
           "id": 1643,
            "title": "Popular Design News of the Week: June 12, 2017 – June 18, 2017",
            "description": "Every week users submit a lot of interesting stuff on our sister site Webdesigner News, highlighting great content from around the web that can be of interest to web designers.  The best way to keep track of all the great stories and news being posted is simply to check out the Webdesigner News site, however, [&#8230;]",
            "datePublished": "2017-06-18T13:45:39Z",
            "linkToSource": "https://www.webdesignerdepot.com/2017/06/popular-design-news-of-the-week-june-12-2017-june-18-2017/",
            "imageUrl": "https://www.webdesignerdepot.com/cdn-origin/uploads/2017/06/featured-3.jpg",
            "channels": [
                {
                    "id": 52,
                    "name": "Webdesigner Depot"
                }
            ],
            "tags": [
                "css",
                "design",
                "freelancing",
                "google"
            ]
        },
        {
            "id": 1641,
            "title": "The $1,000 Podcasting Setup",
            "description": "I figure between (as I write) the 267 episodes of ShopTalk, 134 episodes of CodePen Radio, 154 video screencasts (and many hundreds more as part of the different series), and all my guest podcast apperances, I'm edging on 1,000 things I've voice-recorded for public consumption.  98% of that was with the Rode Podcaster, the same exact microphone I documented using in 2008. I figured it was about time for an upgrade, as I plan to continue &#8230;  The $1,000 Podcasting Setup is a post from...",
            "datePublished": "2017-06-18T13:18:54Z",
            "linkToSource": "https://css-tricks.com/1000-podcasting-setup/",
            "imageUrl": "https://cdn.css-tricks.com/wp-content/uploads/2017/06/shure-SM7b.jpg",
            "channels": [
                {
                    "id": 3,
                    "name": "CSS-Tricks"
                }
            ],
            "tags": [
                "article"
            ]
        }
       ]
   }
 * 
 * 
 */