import { createApp } from 'vue'
import App from './App.vue'
import ChatMonWindow from './components/ChatMonWindow.vue'
import ChatMonAbout from './components/ChatMonAbout.vue';
import ChatMonLogo from './components/ChatMonLogo.vue';
import ChatMonPreLoader from './components/ChatMonPreLoader.vue';
import ChatMonSettings from './components/ChatMonSettings.vue';
import PersonConfigurator from './components/PersonConfigurator.vue';
import PersonSlot from './components/PersonSlot.vue';
import TwitchStatus from './components/TwitchStatus.vue';


import { NButton, NIcon, NProgress, NInput, NTooltip, NForm, NFormItem, NInputGroup, NSlider, NCheckbox, NSelect, NSpace } from 'naive-ui'; // Said buttons

var app = createApp(App);
//app.config.globalProperties.chrome = {};
app.config.globalProperties.chrome = chrome;
app.config.globalProperties.window = window;
app.component('PersonSlot', PersonSlot);
app.component('ChatMonPreLoader', ChatMonPreLoader);
app.component('PersonConfigurator', PersonConfigurator);
app.component('TwitchStatus', TwitchStatus);
app.component('ChatMonWindow', ChatMonWindow);
app.component('ChatMonAbout', ChatMonAbout);
app.component('ChatMonSettings', ChatMonSettings);
app.component('ChatMonLogo', ChatMonLogo);
app.component('NButton', NButton);
app.component('NIcon', NIcon);
app.component('NProgress', NProgress);
app.component('NInput', NInput);
app.component('NTooltip', NTooltip);
app.component('NForm', NForm);
app.component('NFormItem', NFormItem);
app.component('NInputGroup', NInputGroup);
app.component('NSlider', NSlider);
app.component('NCheckbox', NCheckbox);
app.component('NSelect', NSelect);
app.component('NSpace', NSpace);

app.mount('#app')

