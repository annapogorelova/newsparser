var webpackMerge = require('webpack-merge');
var webpack = require('webpack');
var commonConfig = require('./webpack.common.js');
var helpers = require('./helpers');
const WebpackShellPlugin = require('webpack-shell-plugin');

module.exports = webpackMerge(commonConfig, {
    devtool: 'cheap-module-eval-source-map',

    output: {
        path: helpers.root('./dist'),
        publicPath: '/',
        filename: '[name].js',
        chunkFilename: '[id].chunk.js'
    },

    devServer: {
        historyApiFallback: true,
        stats: 'minimal'
    },

    plugins: [
        new WebpackShellPlugin({
            onBuildStart:['npm run clean']
        })
    ]
});
