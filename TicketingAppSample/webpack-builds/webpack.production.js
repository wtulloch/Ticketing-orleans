const webpack = require('webpack');

const plugins = [
    new webpack.DefinePlugin({
        'SERVICE_URL': JSON.stringify(process.env.SERVICE_URL || 'http://ticketapi.localtest.me')
      })
];

module.exports = () =>({
    module: {
        rules: [
            {
                test: /\.css$/,
                use:['style-loader', 'css-loader']
            }
        ]
    },
    plugins
});