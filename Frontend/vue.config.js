const { defineConfig } = require('@vue/cli-service')
const LicenseWebpackPlugin = require('license-webpack-plugin').LicenseWebpackPlugin;
const path = require("path");
module.exports = defineConfig({
    transpileDependencies: true,
    outputDir: path.resolve(__dirname, "../publish/html/"),
    devServer: {
        static: {
            directory: path.join(__dirname, '../publish/html/'),
        },
    },
    configureWebpack: {
        plugins: [
            new LicenseWebpackPlugin({
                excludedPackageTest: (packageName) => {
//                    console.log(packageName);
                    // TODO(@philippfromme): workaround for https://github.com/camunda/camunda-modeler/issues/3249
                    // cf. https://github.com/xz64/license-webpack-plugin/issues/124
                    return packageName === 'frontend';
                }
            })
        ],
    }
})
