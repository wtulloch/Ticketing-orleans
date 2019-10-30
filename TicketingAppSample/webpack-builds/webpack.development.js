const webpack = require('webpack');

const plugins = [
    new webpack.DefinePlugin({
        'SERVICE_URL': JSON.stringify('http://localhost:5000')
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
    devtool: 'source-map',
    plugins
})