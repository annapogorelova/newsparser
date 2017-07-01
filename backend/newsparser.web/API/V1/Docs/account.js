/**
 * @api {get} /api/account Get account
 * @apiName GetAccount
 * @apiGroup Account
 * @apiVersion  1.0.0
 * @apiHeader {String} Authorization Bearer eyJhbGciOiJIUzNiIsInR5...
 *
 * @apiSuccess (Success 200) {Number} id Id of the account
 * @apiSuccess (Success 200) {String} email Email of the account
 * @apiSuccess (Success 200) {Boolean} hasPassword Flag indicating whether the account has a password set
 *
 * @apiSuccessExample {json} Success-Response:
   HTTP/1.1 200 OK
  {
    "data": {
      "id" : 1,
      "email" : "johndoe@gmail.com",
      "hasPassword" : true
    }
  }
}
 *
 */

/**
 * 
 * @api {post} /api/account Create account
 * @apiName PostAccount
 * @apiGroup Account
 * @apiVersion  1.0.0
 * @apiHeader {String} Content-type application/json
 * 
 * @apiParam  {String{1..255}} email Email
 * @apiParam  {String{8..255}} password Password
 * 
 * 
 * @apiParamExample  {json} Request-Example:
   {
       "email" : "johndoe@gmail.com",
       "password" : "12345678"
   }
 * 
 * 
 * @apiSuccessExample {json} Success-Response:
   HTTP/1.1 201 Created
   {
       "message" : "Account was created."
   }
 * 
 * @apiError (500) InternalServerError Failed to create the account.
 * 
 */

/**
 * 
 * @api {post} /api/account/{email}/confirmation Confirm account
 * @apiName PostAccountConfirmation
 * @apiGroup Account
 * @apiVersion  1.0.0
 * @apiHeader {String} Content-type application/json
 * 
 * @apiParam  {String{1..255}} email Email
 * @apiParam  {String{1..255}} confirmationToken Confirmation token
 * 
 * 
 * @apiParamExample  {json} Request-Example:
   {
       "confirmationToken" : "1Hysn28873vx73bx738msm3"
   }
 * 
 * 
 * @apiSuccessExample {json} Success-Response:
   HTTP/1.1 200 OK
   {
       "message" : "Email was successfully confirmed."
   }
 *
 * @apiError (404) NotFound User was not found.
 * @apiError (400) BadRequest Email has already been confirmed.
 * @apiError (500) InternalServerError Failed to confirm the email. 
 * 
 */

/**
 * 
 * @api {post} /api/account/passwordRecovery Request password reset
 * @apiName PostPasswordRecovery
 * @apiGroup Account
 * @apiVersion  1.0.0
 * @apiHeader {String} Content-type application/json
 * 
 * @apiParam  {String{1..255}} email Email
 * 
 * 
 * @apiParamExample  {json} Request-Example:
   {
       "email" : "johndoe@gmail.com"
   }
 * 
 * 
 * @apiSuccessExample {json} Success-Response:
   HTTP/1.1 200 OK
   {
       message : "Password reset email was sent."
   }
 * 
 * 
 */

/**
 * 
 * @api {post} /api/account/{email}/passwordRecovery Reset password
 * @apiName PostPasswordReset
 * @apiGroup Account
 * @apiVersion  1.0.0
 * @apiHeader {String} Content-type application/json
 * 
 * @apiParam  {String{1..255}} passwordResetToken Password reset token
 * @apiParam  {String{8..255}} newPassword New password
 *  
 * 
 * @apiParamExample  {json} Request-Example:
   {
       "passwordResetToken" : "1Hysn28873vx73bx738msm3",
       "newPassword": "12345678"
   }
 * 
 * 
 * @apiSuccessExample {json} Success-Response:
   HTTP/1.1 200 OK
   {
       "message" : "Password was reset. You can sign in now."
   }
 * 
 * 
 */

/**
 * 
 * @api {put} /api/account/put Update account
 * @apiName PutAccount
 * @apiGroup Account
 * @apiVersion  1.0.0
 * @apiHeader {String} Authorization Bearer eyJhbGciOiJIUzNiIsInR5...
 * @apiHeader {String} Content-type application/json
 * 
 * @apiParam  {String{1..255}} email Email
 * 
 * 
 * @apiParamExample  {json} Request-Example:
   {
       "email" : "johndoe1@gmail.com"
   }
 * 
 * 
 * @apiSuccessExample {json} Success-Response:
   HTTP/1.1 200 OK  
   {
       "message" : "Account was updated."
   }
 * 
 * 
 */

/**
 * 
 * @api {put} /api/account/passwordChange Change password
 * @apiName PasswordChange
 * @apiGroup Account
 * @apiVersion  1.0.0
 * @apiHeader {String} Authorization Bearer eyJhbGciOiJIUzNiIsInR5...
 * @apiHeader {String} Content-type application/json
 * 
 * @apiParam  {String{8..255}} currentPassword Current password
 * @apiParam  {String{8..255}} newPassword New password
 * 
 * @apiParamExample  {json} Request-Example:
   {
       "currentPassword" : "12345678",
       "newPassword" : "87654321"
   }
 * 
 * 
 * @apiSuccessExample {json} Success-Response:
   HTTP/1.1 200 OK
   {
       "message" : "Password was successfully changed."
   }
 * 
 * 
 */

/**
 * 
 * @api {post} /api/account/passwordCreation Create password
 * @apiName PasswordCreation
 * @apiGroup Account
 * @apiVersion  1.0.0
 * @apiHeader {String} Authorization Bearer eyJhbGciOiJIUzNiIsInR5...
 * @apiHeader {String} Content-type application/json
 * 
 * @apiParam  {String{8..255}} password Password
 * 
 * 
 * @apiParamExample  {json} Request-Example:
   {
       "password" : "12345678"
   }
 * 
 * 
 * @apiSuccessExample {json} Success-Response:
   HTTP/1.1 200 OK
   {
       "message" : "Password was successfully created."
   }
 * 
 * 
 */