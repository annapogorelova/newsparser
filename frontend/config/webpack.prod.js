var webpack = require('webpack');
var webpackMerge = require('webpack-merge');
var commonConfig = require('./webpack.common.js');
const UglifyJSPlugin = require('uglifyjs-webpack-plugin');
var helpers = require('./helpers');

module.exports = webpackMerge(commonConfig, {
    output: {
        path: helpers.root('../dist/frontend'),
        publicPath: '/',
        filename: '[name].[hash].js',
        chunkFilename: '[id].[hash].chunk.js'
    },

    plugins: [
        new webpack.NoEmitOnErrorsPlugin(),
        new webpack.LoaderOptionsPlugin({
            htmlLoader: {
                minimize: false // workaround for ng2
            }
        }),
        new UglifyJSPlugin()
    ]
});