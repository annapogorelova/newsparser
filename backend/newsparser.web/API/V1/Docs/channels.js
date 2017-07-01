/**
 * 
 * @api {get} /api/channels/:id Get channel
 * @apiName GetChannel
 * @apiGroup Channels
 * @apiVersion  1.0.0
 * @apiHeader Authorization Bearer eyJhbGciOiJIUzNiIsInR5... 
 * 
 * 
 * @apiSuccessExample {type} Success-Response:
   HTTP/1.1 200 OK
   {
        "data": {
            "id": 43,
            "name": ".NET Tools Blog",
            "feedUrl": "https://blog.jetbrains.com/dotnet/feed/",
            "imageUrl": null,
            "websiteUrl": "https://blog.jetbrains.com/dotnet",
            "description": "JetBrains tools for .NET developers and Visual Studio users",
            "isSubscribed": true,
            "isPrivate": false,
            "subscribersCount": 1,
            "language": "en"
        }
    }
 * 
 * 
 */

/**
 * 
 * @api {get} /api/channels?pageSize=&pageIndex=&search=&subscribed= Get channels
 * @apiName GetChannels
 * @apiGroup Channels
 * @apiVersion  1.0.0
 * @apiHeader Authorization Bearer eyJhbGciOiJIUzNiIsInR5... 
 * 
 * @apiParam  {Number} [pageSize=5] Page size
 * @apiParam  {Number} [pageIndex=0] Page size
 * @apiParam  {String} [search] Search
 * @apiParam  {Boolean} [subscribed=false] Subscribed flag
 * 
 * 
 * @apiSuccessExample {json} Success-Response:
   HTTP/1.1 200 OK
   {
        "data": [
            {
                "id": 32,
                "name": ".NET Blog",
                "feedUrl": "https://blogs.msdn.microsoft.com/dotnet/feed/",
                "imageUrl": "https://msdnshared.blob.core.windows.net/media/2017/02/Microsoft-favicon-cropped3.png",
                "websiteUrl": "https://blogs.msdn.microsoft.com/dotnet",
                "description": "A first-hand look from the .NET engineering teams",
                "isSubscribed": true,
                "isPrivate": false,
                "subscribersCount": 1,
                "language": "en"
            },
            {
                "id": 43,
                "name": ".NET Tools Blog",
                "feedUrl": "https://blog.jetbrains.com/dotnet/feed/",
                "imageUrl": null,
                "websiteUrl": "https://blog.jetbrains.com/dotnet",
                "description": "JetBrains tools for .NET developers and Visual Studio users",
                "isSubscribed": true,
                "isPrivate": false,
                "subscribersCount": 1,
                "language": "en"
            }
        ]
    }
 * 
 * 
 */