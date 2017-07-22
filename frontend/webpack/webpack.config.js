var devConfig = require('./webpack.dev.js');
var prodConfig = require('./webpack.prod.js');

module.exports = process.env.NODE_ENV === 'production' ? prodConfig : devConfig;