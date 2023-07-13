import { createApp } from 'vue'
import App from './App.vue'

var app = createApp(App);
//app.config.globalProperties.chrome = {};
app.config.globalProperties.chrome = chrome;
app.config.globalProperties.window = window;
app.mount('#app')

