/**
 * 
 * @api {post} /api/subscription/:id Create subscription
 * @apiName CreateSubscription
 * @apiGroup Subscription
 * @apiVersion  1.0.0
 * @apiHeader Authorization Bearer eyJhbGciOiJIUzNiIsInR5...
 * 
 * @apiSuccess (201) {json} Created Subscription was created.
 * 
 * @apiSuccessExample {type} Success-Response:
   HTTP/1.1 201 Created
   {
       "message" : "Successfully subscribed to channel."
   }
 * 
 *
 * @apiError (404) NotFound Channel was not found.
 * @apiError (400) BadRequest User is already subscribed to this channel.
 * 
 * @apiErrorExample (400) Error-Response
   HTTP/1.1 400 Bad Request  
   {
       "message" : "User is already subscribed to this channel."
   }
 *
 *  
 */

/**
 * 
 * @api {delete} /api/subscription/:id Delete subscription
 * @apiName DeleteSubscription
 * @apiGroup Subscription
 * @apiVersion  1.0.0
 * 
 * 
 * @apiSuccess (200) {json} Created Subscription was deleted.
 * 
 * 
 * @apiSuccessExample {type} Success-Response:
   HTTP/1.1 200 OK
   {
       "message" : "Successfully unsubscribed from channel."
   }
 * 
 *
 * @apiError (404) NotFound Channel was not found.
 * @apiError (400) BadRequest User is not subscribed to this channel.
 * 
 * @apiErrorExample (400) Error-Response
   HTTP/1.1 400 Bad Request  
   {
       "message" : "User is already subscribed to this channel."
   }
 *
 *  
 */