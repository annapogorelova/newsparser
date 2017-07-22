var devConfig = require('./webpack.dev.js');
var prodConfig = require('./webpack.prod.js');

module.exports = process.env.ENV === 'production' ? prodConfig : devConfig;