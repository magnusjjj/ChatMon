<template>
    <div class="mainwindow">
        <div style="height: 4em;">
            <n-button class="dragmeh" size="large" type="primary" v-if="ShouldShowUI">
                Drag the window here
                <template #icon>
                    <n-icon><drag-outlined /></n-icon>
                </template>
            </n-button>
            <n-button class="close" size="large" type="primary" v-if="ShouldShowUI" @click="exitchatmon">
                <template #icon>
                    <n-icon><close-outlined /></n-icon>
                </template>
            </n-button>
        </div>
        <div class="content">
            <PersonSlot v-for="(person, index) in PersonList" :key="index" :person="person" :slotnumber="index" @CharacterPictureClicked="OnCharacterClicked" :message="MessageList[index]" />
            <PersonConfigurator :show="ShouldShowConfigurator" :personslot="ConfiguratorSlot" :person="ConfiguratorPerson" @SaveCompleted="SavePersonCompleted" />
            <ChatMonSettings :show="ShouldShowSettings" @SaveCompleted="SaveSettingsCompleted" />
        </div>
        <div class="ShowSettingsButton" v-if="ShouldShowUI">
            <n-button @click="ShowSettings" size="large" type="primary">
                Settings
                <template #icon>
                    <n-icon><setting-outlined /></n-icon>
                </template>
            </n-button>

            <ChatMonAbout />
            <TwitchStatus />

        </div>

    </div>
</template>

<script setup>
    import { ref, reactive } from 'vue';

    const ShouldShowUI = ref(true);

    // We can't use the :focus css, because then we can't use the --app-region drag, due to OS-level issues. Unfixable.
    // We want to be able to use --app-region drag, so that we can drag around the window with the dragme box.
    // This means that we need to hide and show the UI via this hack instead.
    addEventListener("blur", (event) => {
        ShouldShowUI.value = false;
    });
    addEventListener("focus", (event) => {
        ShouldShowUI.value = true;
    });

    import PersonSlot from './components/PersonSlot.vue'
    import PersonConfigurator from './components/PersonConfigurator.vue';
    import TwitchStatus from './components/TwitchStatus.vue';
    import ChatMonAbout from './components/ChatMonAbout.vue';
    import SaveDataHandler from './handlers/savedatahandler.js';
    import ChatMonSettings from './components/ChatMonSettings.vue';

    import { SettingOutlined, DragOutlined, CloseOutlined } from '@vicons/antd';
    import { NButton, NIcon } from 'naive-ui';
    import ChatHandler from './handlers/chathandler.js';
    import TTSHandler from './handlers/ttshandler';
    import WebView2IntegrationHandler from './handlers/webview2integrationhandler';

    var PersonList = reactive([{}, {}, {}, {}, {}, {}]);
    var MessageList = reactive(["", "", "", "", "", ""]);
    var ConfiguratorPerson = ref({});
    var ConfiguratorSlot = ref(0);

    const ShouldShowConfigurator = ref(false);
    const ShouldShowSettings = ref(false);
    

    function OnCharacterClicked(slotnumber) {
        ConfiguratorSlot.value = slotnumber;
        ConfiguratorPerson.value = PersonList[slotnumber];
        ShouldShowConfigurator.value = true;
    }

    // eslint-disable-next-line no-unused-vars
    function SavePersonCompleted(slotnumber, data) {
        ShouldShowConfigurator.value = false;
    }

    function ShowSettings() {
        ShouldShowSettings.value = true;
    }

    function SaveSettingsCompleted() {
        ShouldShowSettings.value = false;
    }

    SaveDataHandler.onCharacterChanged((slot, data) => {
        console.log("character saved", slot, data);
        PersonList.splice(slot, 1, data);
    });

    SaveDataHandler.onValidationError(() => {
        ShouldShowSettings.value = true;
    });

    ChatHandler.onTalk((slot, displayname, message) => {
        if (displayname != null && displayname != PersonList[slot].displayname) {
            PersonList[slot].displayname = displayname;
            SaveDataHandler.SaveCharacter(slot, PersonList[slot]);
        }
        MessageList[slot] = message;

        TTSHandler.speak(message, PersonList[slot].voice);
    });

    SaveDataHandler.onSettingsChanged(function (settings) {
        WebView2IntegrationHandler.SendMessage({ "type": "ChatMonSettings", "settings": settings })
    });

    WebView2IntegrationHandler.onMessage('ShutUp', () => {
        TTSHandler.shutUp();

        for (let i = 0; i < MessageList.length; i++) {
            MessageList[i] = "";
        }
    });

    function exitchatmon() {
        window.close();
    }
    
</script>

<style>
    .dragmeh {
        --app-region: drag;
        -webkit-app-region: drag;
        margin-left: 1em;
        float: left;
    }
</style>

<style scoped="true">
    .mainwindow {
       
    }


    .mainwindow:hover .ShowSettingsButton {
        /*display: block;
        */
    }

    .ShowSettingsButton {
        clear: both;
        position: relative;
        top: 1em;
        /*display: none;*/
    }

    .close {
        float: right;
    }
</style>
