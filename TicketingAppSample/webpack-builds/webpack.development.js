module.exports = () =>({
    module: {
        rules: [
            {
                test: /\.css$/,
                use:['style-loader', 'css-loader']
            }
        ]
    },
    devtool: 'source-map'
})