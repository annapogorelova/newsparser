/**
 * 
 * @api {post} /api/token Post authorization
 * @apiName PostToken
 * @apiGroup Authorization
 * @apiVersion  1.0.0
 * @apiHeader {String} Content-type application/x-www-form-urlencoded
 * 
 * @apiParam  {String} [username] Email (required only for the password auth)
 * @apiParam  {String} [password] Password (required only for the password auth)
 * @apiParam  {String="password","urn:ietf:params:oauth:grant-type:facebook_access_token","urn:ietf:params:oauth:grant-type:google_access_token"} grant_type Grant type
 * @apiParam  {String} [assetrion] The external provider access token (required only for the external auth)
 * @apiParam  {String="offline_access"} [scope] If set value "offline_access" the refresh token will be included in the response
 * 
 * 
 * @apiParamExample  {x-www-form-urlencoded} Password Request-Example:
   {
       "username" : "johndoe@gmail.com",
       "password" : "12345678",
       "grant_type" : "password" 
   }
 * 
 *
 * @apiParamExample  {x-www-form-urlencoded} Facebook access token Request-Example:
   {
       "assertion" : "EAAHT0W2CsRcBAC3zhXynwbZB....",
       "grant_type" : "urn:ietf:params:oauth:grant-type:facebook_access_token",
       "scope" : "open_id" 
   }
 *
 * 
 * @apiSuccessExample {json} Success-Response:
   HTTP/1.1 200 OK
   {
       "token_type" : "Bearer",
       "access_token" : "eyJhbGciOIUzI1NiIsInR5c...",
       "refresh_token" : "eyJhbGciOIUzI1NiIsInR5c...",
       "expires_in": 43200
   }
 *
 * @apiError (400) BadRequest Invalid username or password.
 * 
 * @apiErrorExample (400) Error-Response
   HTTP/1.1 400 Bad Request  
   {
       "message": "Invalid username or password."
   }
*
* @apiError (400) BadRequest Invalid request format.
*
* @apiErrorExample (400) Error-Response
   HTTP/1.1 400 Bad Request  
   {
       "error": "invalid_request",
       "error_description": "The mandatory 'username' and/or 'password' parameters are missing."
   }
 *
 * 
 */